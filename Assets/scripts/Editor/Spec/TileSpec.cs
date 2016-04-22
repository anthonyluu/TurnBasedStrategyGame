using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;

namespace TurnBasedStrategyGame {

	[TestFixture]
	[Category("Sample Tests")]
	public class TileSpec {

		[Test]
		public void getTopLeftCornerNeighbours() {
			List<List<TileModel>> map = new List<List<TileModel>>();
			// generate map
			for (int i = 0; i < 10; i++) {
				List<TileModel> row = new List<TileModel>();
				for (int j = 0; j < 10; j++) {
					TileModel myTile = new TileModel (new Vector2(i,j));
					row.Add (myTile);
				}
				map.Add (row);
			}

			// get top left tile
			TileModel topLeftTile = map[0][0];

			// assert
			topLeftTile.generateNeighbours(map);
			List<TileModel> actualNeighbours = topLeftTile.neighbours;
			Assert.IsTrue (actualNeighbours.Count == 2);
		}
	}
}
