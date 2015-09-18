using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {

	public Vector2 gridPosition = Vector2.zero;

	public float moveSpeed = 2.0f;
	public Vector3 moveDestination;

	public bool moving = false;
	public bool attacking = false;

	public string playerName = "1";
	public int HP = 25;
	public int actionPoints = 2;

	public Armor headArmor;
	public Armor chestArmor;
	public Armor gauntletArmor;
	public Armor legArmor;

	public List<Weapon> handWeapons = new List<Weapon>();
	
	public int baseMovementPerActionPoint = 5;
	public int baseAttackRange = 1;
	public int baseDamageReduction = 0;
	public float baseAttackChance = 0.75f;
	public float baseEvade = 0.15f;
	public float baseDamageBase = 5;
	public float baseDamageRollSides = 6;

	public int movementPerActionPoint {
		get {
			int ret = baseMovementPerActionPoint;
			if (headArmor != null) ret += headArmor.alterMovementPerActionPoint;
			if (chestArmor != null) ret += chestArmor.alterMovementPerActionPoint;
			if (gauntletArmor != null) ret += gauntletArmor.alterMovementPerActionPoint;
			if (legArmor != null) ret += legArmor.alterMovementPerActionPoint;
			
			foreach (Weapon w in handWeapons) {
				if (w != null) {
					ret += w.alterMovementPerActionPoint;
					if (w.type == WeaponSlotType.TwoHanded) break;
				}
			}
			return ret;
		}
		set {}
	}
	
	public int attackRange {
		get {
			int ret = baseAttackRange;
			if (headArmor != null) ret += headArmor.alterAttackRange;
			if (chestArmor != null) ret += chestArmor.alterAttackRange;
			if (gauntletArmor != null) ret += gauntletArmor.alterAttackRange;
			if (legArmor != null) ret += legArmor.alterAttackRange;
			
			foreach (Weapon w in handWeapons) {
				if (w != null) {
					ret += w.alterAttackRange;
					if (w.alterAttackRange > 0) break;
				}
			}
			return ret;
		}
		set {}
	}

	public int damageReduction {
		get {
			int ret = baseDamageReduction;
			if (headArmor != null) ret += headArmor.alterDamageReduction;
			if (chestArmor != null) ret += chestArmor.alterDamageReduction;
			if (gauntletArmor != null) ret += gauntletArmor.alterDamageReduction;
			if (legArmor != null) ret += legArmor.alterDamageReduction;

			foreach (Weapon w in handWeapons) {
				if (w != null) {
					ret += w.alterDamageReduction;
					if (w.type == WeaponSlotType.TwoHanded) break;
				}
			}
			return ret;
		}
		set {}
	}

	public float attackChance {
		get {
			float ret = baseAttackChance;
			if (headArmor != null) ret += headArmor.alterAttackChance;
			if (chestArmor != null) ret += chestArmor.alterAttackChance;
			if (gauntletArmor != null) ret += gauntletArmor.alterAttackChance;
			if (legArmor != null) ret += legArmor.alterAttackChance;
			
			foreach (Weapon w in handWeapons) {
				if (w != null) {
					ret += w.alterAttackChance;
					if (w.type == WeaponSlotType.TwoHanded) break;
				}
			}
			return ret;
		}
		set {}
	}

	public float evade {
		get {
			float ret = baseEvade;
			if (headArmor != null) ret += headArmor.alterEvade;
			if (chestArmor != null) ret += chestArmor.alterEvade;
			if (gauntletArmor != null) ret += gauntletArmor.alterEvade;
			if (legArmor != null) ret += legArmor.alterEvade;
			
			foreach (Weapon w in handWeapons) {
				if (w != null) {
					ret += w.alterEvade;
					if (w.type == WeaponSlotType.TwoHanded) break;
				}
			}
			return ret;
		}
		set {}
	}

	public float damageBase {
		get {
			float ret = baseDamageBase;
			if (headArmor != null) ret += headArmor.alterDamageBase;
			if (chestArmor != null) ret += chestArmor.alterDamageBase;
			if (gauntletArmor != null) ret += gauntletArmor.alterDamageBase;
			if (legArmor != null) ret += legArmor.alterDamageBase;
			
			foreach (Weapon w in handWeapons) {
				if (w != null) {
					ret += w.alterDamageBase;
					if (w.type == WeaponSlotType.TwoHanded) break;
				}
			}
			return ret;
		}
		set {}
	}

	public float damageRollSides {
		get {
			float ret = baseDamageRollSides;
			if (headArmor != null) ret += headArmor.alterDamageRollSides;
			if (chestArmor != null) ret += chestArmor.alterDamageRollSides;
			if (gauntletArmor != null) ret += gauntletArmor.alterDamageRollSides;
			if (legArmor != null) ret += legArmor.alterDamageRollSides;
			
			foreach (Weapon w in handWeapons) {
				if (w != null) {
					ret += w.alterDamageRollSides;
					if (w.type == WeaponSlotType.TwoHanded) break;
				}
			}
			return ret;
		}
		set {}
	}

	// movement animation
	public List<Vector3> positionQueue = new List<Vector3>();

	void Awake() {
		moveDestination = transform.position;
	}

	// Use this for initialization
	void Start () {
	
	}

	public void SkipTurn() {
		actionPoints = 2;
		moving = false;
		attacking = false;
		GameManager.instance.nextTurn();
	}
	
	// Update is called once per frame
	public virtual void Update () {
		if (GameManager.instance.players [GameManager.instance.currentPlayerIndex] == this) {
			transform.GetComponent<Renderer> ().material.color = Color.green;
		} else {
			transform.GetComponent<Renderer>().material.color = Color.white;
		}
		if (HP <= 0) {
			transform.rotation = Quaternion.Euler (new Vector3(90, 0, 0));
			transform.GetComponent<Renderer>().material.color = Color.red;
		}
	}

	public virtual void TurnUpdate() {
		if (actionPoints <= 0) {
			SkipTurn();
		}
	}

	public virtual void TurnOnGUI() {

	}

	public void OnGUI() {
		Vector3 location = Camera.main.WorldToScreenPoint (transform.position) + Vector3.up * 35;
		GUI.TextArea(new Rect(location.x, Screen.height - location.y, 30, 20), HP.ToString());
	}
}
