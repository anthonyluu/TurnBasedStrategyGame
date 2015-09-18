using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Room {
	public List<Tile> regionTiles;
	// edge tiles are defined as tiles that are adjacent to impassible tiles
	public List<Tile> edgeTiles;
	public List<Room> connectedRooms;
	
	public Room() {

	}

	public Room(List<Tile> tiles, int width, int height, List<List<Tile>> map) {
		regionTiles = tiles;
		edgeTiles = new List<Tile> ();
		connectedRooms = new List<Room> ();

		foreach (Tile t in regionTiles) {
			for (int i = (int)t.gridPosition.x - 1; i <= (int)t.gridPosition.x + 1; i++) {
				for (int j = (int)t.gridPosition.y - 1; j < (int)t.gridPosition.y + 1; j++) {
					if (i == (int)t.gridPosition.x && j == (int)t.gridPosition.y) {
						continue;
					}
					if (IsInMapRange(i, j, width, height) && (i == (int)t.gridPosition.x || j == (int)t.gridPosition.y) && map[i][j].impassible) {
							// if any of the tile's directly adjacent tiles are edges (ignoring diagonals)
							edgeTiles.Add (t);
					}
				}
			}
		}

	}

	public static void ConnectRooms(Room roomA, Room roomB) {
		roomA.connectedRooms.Add (roomB);
		roomB.connectedRooms.Add (roomA);
	}

	public bool IsConnected(Room otherRoom) {
		return connectedRooms.Contains (otherRoom);
	}

	static bool IsInMapRange(int x, int y, int width, int height) {
		return x >= 0 && x < width && y >= 0 && y < height;
	}
}
