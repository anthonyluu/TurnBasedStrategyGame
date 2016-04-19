using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tile : MonoBehaviour {

	public Vector2 gridPosition = Vector2.zero;

	public int movementCost = 1;
	public bool impassible = false;
	public bool occupied = false;

	public List<Tile> neighbours = new List<Tile>();

	// Use this for initialization
	void Awake() {

	}

	void Start () {
		generateNeighbours (GameManager.instance.map);
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// neighbours is a List of Tiles adjacent to this Tile
	public void generateNeighbours(List<List<Tile>> map) {
		// above
		if (gridPosition.y > 0) {
			Vector2 n = new Vector2(gridPosition.x, gridPosition.y - 1);
			neighbours.Add(map[(int)n.x][(int)n.y]);
		}

		// below
		if (gridPosition.y < GameManager.instance.map.Count - 1) {
			Vector2 n = new Vector2(gridPosition.x, gridPosition.y + 1);
			neighbours.Add(map[(int)n.x][(int)n.y]);
		}

		// left
		if (gridPosition.x > 0) {
			Vector2 n = new Vector2(gridPosition.x - 1, gridPosition.y);
			neighbours.Add(map[(int)n.x][(int)n.y]);
		}

		// right
		if (gridPosition.x < GameManager.instance.map.Count - 1) {
			Vector2 n = new Vector2(gridPosition.x + 1, gridPosition.y);
			neighbours.Add(map[(int)n.x][(int)n.y]);
		}
	}

	void OnMouseEnter() {

	}

	void OnMouseExit() {

	}

	void OnMouseDown() {
		if (GameManager.instance.players [GameManager.instance.currentPlayerIndex].moving) {
			GameManager.instance.moveCurrentPlayer (this);
		} else if (GameManager.instance.players [GameManager.instance.currentPlayerIndex].attacking) {
			GameManager.instance.attackWithCurrentPlayer (this);
		} else {
//			Debug.Log (impassible);
//			impassible = impassible ? false : true;
//			if (impassible) {
//				transform.GetComponent<Renderer>().material.color = Color.black;
//			} else {
//				transform.GetComponent<Renderer>().material.color = Color.white;
//			}
		}
	}
}
