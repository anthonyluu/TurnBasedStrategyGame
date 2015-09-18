using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour {
	public static GameManager instance;

	public GameObject TilePrefab;
	public GameObject UserPlayerPrefab;
	public GameObject AIPlayerPrefab;
	public int mapSize = 13;
	public int currentPlayerIndex = 0;

	public List <List<Tile>> map = new List <List<Tile>>();
	public List <Player> players = new List<Player>();

	MapGenerator mapGen;


	void Awake() {
		instance = this;
	}

	// Use this for initialization
	void Start () {
		generateMap ();
		generatePlayers ();
	}
	
	// Update is called once per frame
	void Update () {
		if (players [currentPlayerIndex].HP > 0)
			players [currentPlayerIndex].TurnUpdate ();
		else
			nextTurn ();
	}

	void OnGUI() {
		players [currentPlayerIndex].TurnOnGUI ();
	}

	public void nextTurn() {
		removeTileHighlights ();
		if (currentPlayerIndex + 1 < players.Count) {
			currentPlayerIndex++;
		} else {
			currentPlayerIndex = 0;
		}
	}

	public void highlightTilesAt(Vector2 originLocation, Color highlightColour, int range, bool ignorePlayers = true) {
		List<Tile> highlightedTiles = new List<Tile> ();
		if (ignorePlayers) highlightedTiles = TileHighlight.FindHighlight (map [(int)originLocation.x] [(int)originLocation.y], range);
		else highlightedTiles = TileHighlight.FindHighlight (map [(int)originLocation.x] [(int)originLocation.y], range, players.Where(x=> x.gridPosition != originLocation).Select(x => x.gridPosition).ToArray());
		foreach (Tile t in highlightedTiles) {
			t.transform.GetComponent<Renderer>().material.color = highlightColour;
		}
	}

	public void removeTileHighlights() {
		for (int i = 0; i < mapGen.width; i ++) {
			for (int j = 0; j < mapGen.height; j++) {
				if (!map[i][j].impassible) map[i][j].transform.GetComponent<Renderer>().material.color = Color.white;
			}
		}
	}

	public bool moveCurrentPlayer(Tile destTile) {
		if (destTile.transform.GetComponent<Renderer> ().material.color != Color.white && !destTile.impassible && players[currentPlayerIndex].positionQueue.Count == 0) {
			map[(int)players[currentPlayerIndex].gridPosition.x][(int)players[currentPlayerIndex].gridPosition.y].occupied = false;
			removeTileHighlights();
			players[currentPlayerIndex].moving = false;
			foreach(Tile t in TilePathFinder.FindPath (map[(int)players[currentPlayerIndex].gridPosition.x][(int)players[currentPlayerIndex].gridPosition.y], destTile, players.Where(x=> x.gridPosition != players[currentPlayerIndex].gridPosition && x.gridPosition != players[currentPlayerIndex].gridPosition).Select(x => x.gridPosition).ToArray())) {
				players[currentPlayerIndex].positionQueue.Add(map[(int)t.gridPosition.x][(int)t.gridPosition.y].transform.position + 1.5f * Vector3.up);
			}
			players [currentPlayerIndex].gridPosition = destTile.gridPosition;
			destTile.occupied = true;
			return true;
		} else {
			Debug.Log ("Destination invalid" + destTile.gridPosition);
			return false;
		}
	}

	public void attackWithCurrentPlayer(Tile destTile) {
		if (destTile.transform.GetComponent<Renderer> ().material.color != Color.white && !destTile.impassible) {
			Player target = null;
			foreach (Player p in players) {
				if (p.gridPosition == destTile.gridPosition) {
					target = p;
				}
			}
			
			if (target != null) {
				players[currentPlayerIndex].actionPoints --;

				removeTileHighlights();
				players[currentPlayerIndex].attacking = false;
				
				// attack
				// roll to hit
				bool hit = Random.Range (0.0f,1.0f) <= players[currentPlayerIndex].attackChance - target.evade;
				
				if (hit) {
					int amountOfDamage = Mathf.Max (0, (int) Mathf.Floor (players[currentPlayerIndex].damageBase + Random.Range (0, players[currentPlayerIndex].damageRollSides)) - target.damageReduction);
					
					target.HP -= amountOfDamage;
					
					Debug.Log (players[currentPlayerIndex].playerName + " successfully hit " + target.playerName + " for " + amountOfDamage + " damage!");
				} else {
					Debug.Log (players[currentPlayerIndex].playerName + " missed " + target.playerName);
				}
			}
		} else {
			Debug.Log ("Destination invalid");
		}
	}

	void generateMap() {
		// reference the MapGenerator
		mapGen = GetComponent<MapGenerator> ();

		// create the cellular automata map
		map = mapGen.GenerateMap (30, 30, "anthonyl", false, 40, TilePrefab);
		map = mapGen.GeneratePathsBetweenRegions ();
	}

	void generatePlayers() {
		// TODO: Modify to generate players in a smart way (dont generate players in walls)

		UserPlayer player;
		player = ((GameObject)Instantiate(UserPlayerPrefab, new Vector3(7 - Mathf.Floor(mapGen.width/2),1.5f, -5 + Mathf.Floor(mapGen.height/2)), Quaternion.Euler(new Vector3()))).GetComponent<UserPlayer>();
		player.gridPosition = new Vector2(7,5);
		player.playerName = "P1";		
		player.headArmor = Armor.FromKey (ArmorKey.LeatherCap);
		player.chestArmor = Armor.FromKey (ArmorKey.LeatherVest);
		player.gauntletArmor = Armor.FromKey (ArmorKey.LeatherGauntlet);
		player.legArmor = Armor.FromKey (ArmorKey.LeatherBoots);
		player.handWeapons.Add (Weapon.GetFromKey (WeaponKey.LongBow));
		
		players.Add(player);
		
		player = ((GameObject)Instantiate(UserPlayerPrefab, new Vector3(20 - Mathf.Floor(mapGen.width/2),1.5f, -10 + Mathf.Floor(mapGen.height/2)), Quaternion.Euler(new Vector3()))).GetComponent<UserPlayer>();
		player.gridPosition = new Vector2(20,10);
		player.playerName = "P3";
		player.headArmor = Armor.FromKey (ArmorKey.LeatherCap);
		player.chestArmor = Armor.FromKey (ArmorKey.LeatherVest);
		player.gauntletArmor = Armor.FromKey (ArmorKey.LeatherGauntlet);
		player.legArmor = Armor.FromKey (ArmorKey.LeatherBoots);
		player.handWeapons.Add(Weapon.GetFromKey (WeaponKey.ShortBow));
		
		players.Add(player);
		
		player = ((GameObject)Instantiate(UserPlayerPrefab, new Vector3(20 - Mathf.Floor(mapGen.width/2),1.5f, -12 + Mathf.Floor(mapGen.height/2)), Quaternion.Euler(new Vector3()))).GetComponent<UserPlayer>();
		player.gridPosition = new Vector2(20,12);
		player.playerName = "P4";
		player.headArmor = Armor.FromKey (ArmorKey.LeatherCap);
		player.chestArmor = Armor.FromKey (ArmorKey.LeatherVest);
		player.gauntletArmor = Armor.FromKey (ArmorKey.LeatherGauntlet);
		player.legArmor = Armor.FromKey (ArmorKey.LeatherBoots);
		player.handWeapons.Add (Weapon.GetFromKey (WeaponKey.LongBow));
		
		players.Add(player);
		
		AIPlayer aiplayer = ((GameObject)Instantiate(AIPlayerPrefab, new Vector3(18 - Mathf.Floor(mapGen.width/2),1.5f, -18 + Mathf.Floor(mapGen.height/2)), Quaternion.Euler(new Vector3()))).GetComponent<AIPlayer>();
		aiplayer.gridPosition = new Vector2(18,18);
		aiplayer.playerName = "Bot1";
		player.headArmor = Armor.FromKey (ArmorKey.LeatherCap);
		player.chestArmor = Armor.FromKey (ArmorKey.LeatherVest);
		player.gauntletArmor = Armor.FromKey (ArmorKey.LeatherGauntlet);
		player.legArmor = Armor.FromKey (ArmorKey.LeatherBoots);
		player.handWeapons.Add (Weapon.GetFromKey (WeaponKey.ShortSword));
		
		players.Add(aiplayer);
		
		aiplayer = ((GameObject)Instantiate(AIPlayerPrefab, new Vector3(26 - Mathf.Floor(mapGen.width/2),1.5f, -18 + Mathf.Floor(mapGen.height/2)), Quaternion.Euler(new Vector3()))).GetComponent<AIPlayer>();
		aiplayer.gridPosition = new Vector2(26,18);
		aiplayer.playerName = "Bot2";
		player.headArmor = Armor.FromKey (ArmorKey.LeatherCap);
		player.chestArmor = Armor.FromKey (ArmorKey.LeatherVest);
		player.gauntletArmor = Armor.FromKey (ArmorKey.LeatherGauntlet);
		player.legArmor = Armor.FromKey (ArmorKey.LeatherBoots);
		player.handWeapons.Add (Weapon.GetFromKey (WeaponKey.ShortSword));
		
		players.Add(aiplayer);
		
		aiplayer = ((GameObject)Instantiate(AIPlayerPrefab, new Vector3(22 - Mathf.Floor(mapGen.width/2),1.5f, -10 + Mathf.Floor(mapGen.height/2)), Quaternion.Euler(new Vector3()))).GetComponent<AIPlayer>();
		aiplayer.gridPosition = new Vector2(22,10);
		aiplayer.playerName = "Bot3";
		player.headArmor = Armor.FromKey (ArmorKey.LeatherCap);
		player.chestArmor = Armor.FromKey (ArmorKey.LeatherVest);
		player.gauntletArmor = Armor.FromKey (ArmorKey.LeatherGauntlet);
		player.legArmor = Armor.FromKey (ArmorKey.LeatherBoots);
		player.handWeapons.Add (Weapon.GetFromKey (WeaponKey.Sheild));
		
		players.Add(aiplayer);
		
		aiplayer = ((GameObject)Instantiate(AIPlayerPrefab, new Vector3(18 - Mathf.Floor(mapGen.width/2),1.5f, -8 + Mathf.Floor(mapGen.height/2)), Quaternion.Euler(new Vector3()))).GetComponent<AIPlayer>();
		aiplayer.gridPosition = new Vector2(18,8);
		aiplayer.playerName = "Bot4";
		player.headArmor = Armor.FromKey (ArmorKey.LeatherCap);
		player.chestArmor = Armor.FromKey (ArmorKey.LeatherVest);
		player.gauntletArmor = Armor.FromKey (ArmorKey.LeatherGauntlet);
		player.legArmor = Armor.FromKey (ArmorKey.LeatherBoots);
		player.handWeapons.Add (Weapon.GetFromKey (WeaponKey.WarHammer));
		
		players.Add(aiplayer);
	}


}
