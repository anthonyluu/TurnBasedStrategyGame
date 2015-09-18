using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class AIPlayer : Player {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	public override void Update () {
		base.Update ();
	}

	public override void TurnUpdate() {
		if (positionQueue.Count > 0) {
			transform.position += (positionQueue [0] - transform.position).normalized * moveSpeed * Time.deltaTime;

			if (Vector3.Distance (positionQueue [0], transform.position) <= 0.1f) {
				transform.position = positionQueue [0];
				positionQueue.RemoveAt (0);
				if (positionQueue.Count == 0) {
					actionPoints --;
				}
			}
		} else {
			// queue
			// attack if in range and with lowest hp
			List<Tile> attacktilesInRange = TileHighlight.FindHighlight(GameManager.instance.map[(int)gridPosition.x][(int)gridPosition.y], attackRange);
			List<Tile> movementTilesInRange = TileHighlight.FindHighlight(GameManager.instance.map[(int)gridPosition.x][(int)gridPosition.y], movementPerActionPoint + 1000);
			//attack if in range and with lowest HP
			Debug.Log ("attackTiles in range are " + attackRange + " and the return is " + attacktilesInRange.Count);
			if (attacktilesInRange.Where(x => GameManager.instance.players.Where (y => y.GetType() != typeof(AIPlayer) && y.HP > 0 && y != this && y.gridPosition == x.gridPosition).Count() > 0).Count () > 0) {

				var opponentsInRange = attacktilesInRange.Select(x => GameManager.instance.players.Where (y => y.GetType() != typeof(AIPlayer) && y.HP > 0 && y != this && y.gridPosition == x.gridPosition).Count () > 0 ? GameManager.instance.players.Where(y => y.gridPosition == x.gridPosition).First() : null).ToList();
				Player opponent = opponentsInRange.OrderBy (x => x != null ? -x.HP : 1000).First ();
				
				GameManager.instance.removeTileHighlights();
				moving = false;
				attacking = true;
				GameManager.instance.highlightTilesAt(gridPosition, Color.red, attackRange);
				
				GameManager.instance.attackWithCurrentPlayer(GameManager.instance.map[(int)opponent.gridPosition.x][(int)opponent.gridPosition.y]);
			}

			// if not, move towards nearest opponent
			else if (!moving && movementTilesInRange.Where(x => GameManager.instance.players.Where (y => y.GetType() != typeof(AIPlayer) && y.HP > 0 && y != this && y.gridPosition == x.gridPosition).Count() > 0).Count () > 0) {
				var opponentsInRange = movementTilesInRange.Select(x => GameManager.instance.players.Where (y => y.GetType() != typeof(AIPlayer) && y.HP > 0 && y != this && y.gridPosition == x.gridPosition).Count () > 0 ? GameManager.instance.players.Where(y => y.gridPosition == x.gridPosition).First() : null).ToList();
				Player opponent = opponentsInRange.OrderBy (x => x != null ? -x.HP : 1000).ThenBy (x => x != null ? (TilePathFinder.FindPath(GameManager.instance.map[(int)gridPosition.x][(int)gridPosition.y],
				                                                                                                                             GameManager.instance.map[(int)x.gridPosition.x][(int)x.gridPosition.y])) != null ? TilePathFinder.FindPath(GameManager.instance.map[(int)gridPosition.x][(int)gridPosition.y],
				                                                                                                                   GameManager.instance.map[(int)x.gridPosition.x][(int)x.gridPosition.y]).Count() : 1000 : 1000).First ();

				GameManager.instance.removeTileHighlights();
				moving = true;
				attacking = false;
				GameManager.instance.highlightTilesAt(gridPosition, Color.blue, movementPerActionPoint, false);
				
				List<Tile> path = TilePathFinder.FindPath (GameManager.instance.map[(int)gridPosition.x][(int)gridPosition.y],GameManager.instance.map[(int)opponent.gridPosition.x][(int)opponent.gridPosition.y], GameManager.instance.players.Where(x => x.gridPosition != gridPosition && x.gridPosition != opponent.gridPosition).Select(x => x.gridPosition).ToArray());

				if (path != null && path.Count() > 1) {
					List<Tile> actualMovement = TileHighlight.FindHighlight(GameManager.instance.map[(int)gridPosition.x][(int)gridPosition.y], movementPerActionPoint, GameManager.instance.players.Where (x => x.gridPosition != gridPosition).Select (x => x.gridPosition).ToArray());
					path.Reverse ();
					if (path.Where (x => actualMovement.Contains(x)).Count() > 0) GameManager.instance.moveCurrentPlayer(path.Where (x => actualMovement.Contains(x)).First ());
				} else {
					SkipTurn ();
				}
					
			}
		}
		base.TurnUpdate ();
	}

	public override void TurnOnGUI() {
		
	}
}
