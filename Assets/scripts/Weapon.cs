using UnityEngine;
using System.Collections;

public enum WeaponSlotType {
	OneHanded,
	TwoHanded
}

public enum WeaponKey {
	ShortSword,
	LongSword,
	Sheild,
	WarHammer,
	ShortBow,
	LongBow
}

public class Weapon : Item {
	public WeaponSlotType type;

	public static Weapon GetFromKey (WeaponKey key) {
		Weapon ret = new Weapon ();
		switch (key) {
		case WeaponKey.ShortSword: 
			ret = new Weapon() {
				type = WeaponSlotType.OneHanded,
				alterMovementPerActionPoint = 0,
				alterAttackRange = 1,
				alterDamageReduction = 0,
				alterAttackChance = 0.5f,
				alterEvade = 0.0f,
				alterDamageBase = 3.0f,
				alterDamageRollSides = 0.0f
			};
			break;
		case WeaponKey.Sheild: 
			ret = new Weapon() {
				type = WeaponSlotType.OneHanded,
				alterMovementPerActionPoint = 0,
				alterAttackRange = 1,
				alterDamageReduction = 2,
				alterAttackChance = -0.5f,
				alterEvade = 0.0f,
				alterDamageBase = 0.0f,
				alterDamageRollSides = 0.0f
			};
			break;
		case WeaponKey.LongSword: 
			ret = new Weapon() {
				type = WeaponSlotType.TwoHanded,
				alterMovementPerActionPoint = 0,
				alterAttackRange = 1,
				alterDamageReduction = 0,
				alterAttackChance = -0.15f,
				alterEvade = -0.05f,
				alterDamageBase = 6.0f,
				alterDamageRollSides = 7.0f
			};
			break;
		case WeaponKey.WarHammer: 
			ret = new Weapon() {
				type = WeaponSlotType.TwoHanded,
				alterMovementPerActionPoint = 0,
				alterAttackRange = 1,
				alterDamageReduction = 0,
				alterAttackChance = -0.15f,
				alterEvade = -0.15f,
				alterDamageBase = 3.0f,
				alterDamageRollSides = 15.0f
			};
			break;
		case WeaponKey.ShortBow: 
			ret = new Weapon() {
				type = WeaponSlotType.TwoHanded,
				alterMovementPerActionPoint = 0,
				alterAttackRange = 5,
				alterDamageReduction = 0,
				alterAttackChance = 0.15f,
				alterEvade = 0.00f,
				alterDamageBase = 3.0f,
				alterDamageRollSides = 4.0f
			};
			break;
		case WeaponKey.LongBow: 
			ret = new Weapon() {
				type = WeaponSlotType.TwoHanded,
				alterMovementPerActionPoint = 0,
				alterAttackRange = 10,
				alterDamageReduction = 0,
				alterAttackChance = 0.2f,
				alterEvade = 0.00f,
				alterDamageBase = 6.0f,
				alterDamageRollSides = 7.0f
			};
			break;
		}
		return ret;

	}

}
