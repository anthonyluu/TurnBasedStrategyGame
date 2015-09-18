using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class MapGenerator : MonoBehaviour {
	
	public int width;
	public int height;
	public string seed;
	public bool useRandomSeed;
	public GameObject TilePrefab;
	
	[Range(0,100)]
	public int randomFillPercent;
	
	List<List<Tile>> map;

	public MapGenerator() {

	}
	
	public List<List<Tile>> GenerateMap(int w, int h, string seed, bool useRandomSeed, int randomFillPercent, GameObject tPrefab) {
		this.width = w;
		this.height = h;
		this.seed = seed;
		this.useRandomSeed = useRandomSeed;
		this.randomFillPercent = randomFillPercent;
		this.TilePrefab = tPrefab;

		map = new List <List<Tile>>();
		RandomFillMap();
		
		for (int i = 0; i < 7; i ++) {
			SmoothMap();
		}

		return map;
	}
	
	void RandomFillMap() {
		if (useRandomSeed) {
			seed = Time.time.ToString();
		}
		
		System.Random pseudoRandom = new System.Random(seed.GetHashCode());
		
		for (int i = 0; i < width; i ++) {
			List <Tile> row = new List<Tile>();
			for (int j = 0; j < height; j ++) {
				Tile tile = ((GameObject)Instantiate( TilePrefab, new Vector3(i - Mathf.Floor(width/2), 0, -j + Mathf.Floor(height/2)), Quaternion.Euler(new Vector3()))).GetComponent<Tile>();
				tile.gridPosition = new Vector2 (i,j);
				if (i == 0 || i == width-1 || j == 0 || j == height -1) {
					// force edges to be all walls
					tile.impassible = true;
				}
				else {
					tile.impassible = pseudoRandom.Next(0, 100) < randomFillPercent;
				}
				// change color to black if its impassible
				if (tile.impassible) tile.GetComponent<Renderer>().material.color = Color.black;
				row.Add (tile);
			}
			map.Add (row);
		}
	}
	
	void SmoothMap() {
		for (int i = 0; i < width; i ++) {
			for (int j = 0; j < height; j ++) {
				int neighbourWallTiles = GetSurroundingWallCount(i, j);
				
				if (neighbourWallTiles > 4){
					map[i][j].impassible = true;
					map[i][j].GetComponent<Renderer>().material.color = Color.black;
				} else if (neighbourWallTiles < 4) {
					map[i][j].impassible = false;
					map[i][j].GetComponent<Renderer>().material.color = Color.white;
				}
			}
		}
	}
	
	int GetSurroundingWallCount(int gridX, int gridY) {
		int wallCount = 0;
		for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX ++) {
			for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY ++) {
				if (neighbourX >= 0 && neighbourX < width && neighbourY >= 0 && neighbourY < height) {
					if (neighbourX != gridX || neighbourY != gridY) {
						wallCount += map[neighbourX][neighbourY].impassible ? 1 : 0;
					}
				}
				else {
					wallCount ++;
				}
			}
		}
		
		return wallCount;
	}

	public List<List<Tile>> GeneratePathsBetweenRegions() {
		List<List<Tile>> regions = GetRegions ();
		List<Room> listOfRooms = new List<Room> ();

		// convert the regions to a list of rooms
		foreach (List<Tile> region in regions) {
			Room room = new Room(region, width, height, map);
			listOfRooms.Add(room);
		}

		// compare 2 rooms together from listOfRooms
		ConnectClosestRooms (listOfRooms);

		return map;
	}

	void ConnectClosestRooms(List<Room> listOfRooms) {
		int closestDistance = 0;
		bool possibleConnectionFound = false;
		Tile closestTileA = null;
		Tile closestTileB = null;
		Room closestRoomA = new Room();
		Room closestRoomB = new Room();

		foreach (Room roomA in listOfRooms) {
			foreach (Room roomB in listOfRooms) {
				if (roomA == roomB) {
					continue;
				}
				if (roomA.IsConnected(roomB)) {
					possibleConnectionFound = false;
					break;
				}
				foreach (Tile edgeTileA in roomA.edgeTiles) {
					foreach (Tile edgeTileB in roomB.edgeTiles) {
						int distanceBtwnRooms = (int)(Mathf.Pow (edgeTileA.gridPosition.x - edgeTileB.gridPosition.x, 2) + Mathf.Pow(edgeTileA.gridPosition.y - edgeTileB.gridPosition.y, 2));
						if (distanceBtwnRooms < closestDistance || !possibleConnectionFound) {
							possibleConnectionFound = true;
							closestDistance = distanceBtwnRooms;
							closestTileA = edgeTileA;
							closestTileB = edgeTileB;
							closestRoomA = roomA;
							closestRoomB = roomB;
						}
					}
				}
			}
			if (possibleConnectionFound) {
				CreatePassage(closestRoomA, closestRoomB, closestTileA, closestTileB);
				if (IsInMapRange((int)closestTileA.gridPosition.x + 1, (int)closestTileA.gridPosition.y + 1) && IsInMapRange((int)closestTileB.gridPosition.x + 1, (int)closestTileB.gridPosition.y + 1)) {
					CreatePassage(closestRoomA, closestRoomB, map[(int)closestTileA.gridPosition.x + 1][(int)closestTileA.gridPosition.y + 1], map[(int)closestTileB.gridPosition.x + 1][(int)closestTileB.gridPosition.y + 1]);
				}
				if (IsInMapRange((int)closestTileA.gridPosition.x - 1, (int)closestTileA.gridPosition.y - 1) && IsInMapRange((int)closestTileB.gridPosition.x - 1, (int)closestTileB.gridPosition.y - 1)) {
					CreatePassage(closestRoomA, closestRoomB, map[(int)closestTileA.gridPosition.x - 1][(int)closestTileA.gridPosition.y - 1], map[(int)closestTileB.gridPosition.x - 1][(int)closestTileB.gridPosition.y - 1]);
				}
			}
		}
	}

	void CreatePassage(Room roomA, Room roomB, Tile tileA, Tile tileB) {
		Room.ConnectRooms (roomA, roomB);

		// create a path between the regions
		List<Tile> pathToRegions = new List<Tile> ();
		pathToRegions = (List<Tile>)TilePathFinder.FindPathBetweenRooms (tileA, tileB, map);
		if (pathToRegions != null) {
			foreach (Tile t in pathToRegions) {
				// break the walls between the regions and change the color
				t.impassible = false;
				t.GetComponent<Renderer>().material.color = Color.white;
			}
		}
	}

	List<List<Tile>> GetRegions() {
		List<List<Tile>> regions = new List<List<Tile>> ();
		int[,] visited = new int[width, height];

		for (int i = 0; i < width; i++) {
			for (int j = 0; j < height; j++) {
				if (visited[i, j] == 0 && !map[i][j].impassible) {
					List<Tile> currentRegion = GetTileRegions(i, j);
					regions.Add (currentRegion);
					foreach(Tile t in currentRegion) {
						visited[(int)t.gridPosition.x, (int)t.gridPosition.y] = 1;
					}
				}
			}
		}
		return regions;
	}

	List<Tile> GetTileRegions(int startX, int startY) {
		List<Tile> regionTiles = new List<Tile> ();
		int[,] visited = new int[width, height];
		Queue<Tile> queue = new Queue<Tile> ();
		queue.Enqueue (map [startX] [startY]);
		visited[startX, startY] = 1;

		while (queue.Count > 0) {
			Tile t = queue.Dequeue();
			regionTiles.Add(t);

			for (int i = (int)t.gridPosition.x - 1; i <= (int)t.gridPosition.x + 1; i++) {
				for(int j = (int)t.gridPosition.y - 1; j <= (int)t.gridPosition.y + 1; j++) {
					if (IsInMapRange(i, j) && visited[i, j] == 0 && !map[i][j].impassible) {
						visited[i, j] = 1;
						queue.Enqueue(map[i][j]);
					}
				}
			}
		}
		return regionTiles;

	}

	bool IsInMapRange(int x, int y) {
		return x >= 0 && x < width && y >= 0 && y < height;
	}
	
	
//	void OnDrawGizmos() {
//		if (map != null) {
//			for (int x = 0; x < width; x ++) {
//				for (int y = 0; y < height; y ++) {
//					Gizmos.color = (map[x,y] == 1)?Color.black:Color.white;
//					Vector3 pos = new Vector3(-width/2 + x + .5f,0, -height/2 + y+.5f);
//					Gizmos.DrawCube(pos,Vector3.one);
//				}
//			}
//		}
//	}
	
}