using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TilePathFinder {

	public TilePathFinder () {
	}

	public static List<Tile> FindPath(Tile originTile, Tile destinationTile) {
		return FindPath (originTile, destinationTile, new Vector2[0]);
	}

	// this ignores anything in the occupied array, and ignores impassible tiles
	public static List<Tile> FindPath(Tile originTile, Tile destinationTile, Vector2[] occupied) {
		List<Tile> closed = new List<Tile>();
		List<TilePath> open = new List<TilePath>();
		
		TilePath originPath = new TilePath();
		originPath.addTile(originTile);
		
		open.Add(originPath);
		
		while (open.Count > 0) {
			TilePath current = open[0];
			open.Remove(open[0]);
			
			if (closed.Contains(current.lastTile)) {
				continue;
			} 
			if (current.lastTile == destinationTile) {
				current.listOfTiles.Distinct();
				current.listOfTiles.Remove(originTile);
				return current.listOfTiles;
			}
			
			closed.Add(current.lastTile);
			
			foreach (Tile t in current.lastTile.neighbours) {
				if (t.impassible || occupied.Contains(t.gridPosition)) continue;
				TilePath newTilePath = new TilePath(current);
				newTilePath.addTile(t);
				open.Add(newTilePath);
			}
		}

		foreach (Tile t in closed) {
			Debug.Log ("closed tile" + t.gridPosition);
		}
		return null;
	}

	// this method is different because it needs to find a path between impassible tiles.
	public static List<Tile> FindPathBetweenRooms(Tile originTile, Tile destinationTile, List<List<Tile>> map) {
		List<Tile> closed = new List<Tile>();
		List<TilePath> open = new List<TilePath>();
		
		TilePath originPath = new TilePath();
		originPath.addTile(originTile);
		
		open.Add(originPath);
		
		while (open.Count > 0) {
			TilePath current = open[0];
			open.Remove(open[0]);
			
			if (closed.Contains(current.lastTile)) {
				continue;
			} 
			if (current.lastTile == destinationTile) {
				current.listOfTiles.Distinct();
				current.listOfTiles.Remove(originTile);
				return current.listOfTiles;
			}
			
			closed.Add(current.lastTile);


			current.lastTile.generateNeighbours(map);

			Debug.Log (current.lastTile.gridPosition + " with nb of " + current.lastTile.neighbours.Count);
			
			foreach (Tile t in current.lastTile.neighbours) {
				TilePath newTilePath = new TilePath(current);
				newTilePath.addTile(t);
				open.Add(newTilePath);
			}
		}
		return null;
	}
}
