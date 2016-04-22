using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TurnBasedStrategyGame {
	public class TileModel {
		public Vector2 gridPosition = Vector2.zero;

		public int movementCost = 1;
		public bool impassible = false;
		public bool occupied = false;
		public List<TileModel> neighbours = new List<TileModel>();

		public TileModel(Vector2 gridPos) {
			this.gridPosition = gridPos;
		}

		// neighbours is a List of Tiles adjacent to this Tile
		public void generateNeighbours(List<List<TileModel>> map) {
			// above
			if (gridPosition.y > 0) {
				Vector2 n = new Vector2(gridPosition.x, gridPosition.y - 1);
				neighbours.Add(map[(int)n.x][(int)n.y]);
			}

			// below
			if (gridPosition.y < map.Count - 1) {
				Vector2 n = new Vector2(gridPosition.x, gridPosition.y + 1);
				neighbours.Add(map[(int)n.x][(int)n.y]);
			}

			// left
			if (gridPosition.x > 0) {
				Vector2 n = new Vector2(gridPosition.x - 1, gridPosition.y);
				neighbours.Add(map[(int)n.x][(int)n.y]);
			}

			// right
			if (gridPosition.x < map.Count - 1) {
				Vector2 n = new Vector2(gridPosition.x + 1, gridPosition.y);
				neighbours.Add(map[(int)n.x][(int)n.y]);
			}
		}
	}
}
