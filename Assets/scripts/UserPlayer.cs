using UnityEngine;
using System.Collections;

public class UserPlayer : Player {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	public override void  Update () {
		base.Update ();
	}

	public override void TurnUpdate() {
		// Highlight

		if (positionQueue.Count > 0) {
			if (Vector3.Distance (positionQueue[0], transform.position) > 0.1f) {
				transform.position += (positionQueue[0] - transform.position).normalized * moveSpeed * Time.deltaTime;
				
				if (Vector3.Distance(positionQueue[0], transform.position) <= 0.1f) {
					transform.position = positionQueue[0];
					positionQueue.RemoveAt(0);
					if (positionQueue.Count == 0) {
						actionPoints--;
					}
				}
			}
		}

		base.TurnUpdate ();
	}

	public override void TurnOnGUI() {
		float buttonHeight = 50;
		float buttonWidth = 150;

		// move button
		Rect buttonRect = new Rect (0, Screen.height - buttonHeight * 3, buttonWidth, buttonHeight);

		if (GUI.Button (buttonRect, "Move")) {
			if (!moving) {
				GameManager.instance.removeTileHighlights();
				moving = true;
				GameManager.instance.highlightTilesAt(gridPosition, Color.blue, movementPerActionPoint, false);
			} else {
				moving = false;
				GameManager.instance.removeTileHighlights();
			}
			attacking = false;
		}

		// attack button
		buttonRect = new Rect (0, Screen.height - buttonHeight * 2, buttonWidth, buttonHeight);

		if (GUI.Button (buttonRect, "Attack")) {
			if (!attacking) {
				GameManager.instance.removeTileHighlights();
				attacking = true;
				GameManager.instance.highlightTilesAt(gridPosition, Color.red, attackRange);
			} else {
				attacking = false;
				GameManager.instance.removeTileHighlights();
			}
			moving = false;
		}

		// end turn button
		buttonRect = new Rect (0, Screen.height - buttonHeight * 1, buttonWidth, buttonHeight);

		if (GUI.Button (buttonRect, "End turn")) {
			GameManager.instance.removeTileHighlights();
			SkipTurn();
		}
		

		base.TurnOnGUI ();
	}
}
