using UnityEngine;
using System.Collections;

public enum ArmorSlotType {
	Head,
	Chest,
	Gauntlet,
	Leg
}

public enum ArmorKey {
	// head
	LeatherCap,
	IronHelm,
	MagicianHat,

	// chest
	LeatherVest,
	IronPlate,
	MagicianRobe,
	
	// gauntlet
	LeatherGauntlet,
	IronGauntlet,
	MagicianRing,

	//leg
	LeatherBoots,
	IronBoots,
	MagicianBoots

}

public class Armor : Item {
	public ArmorSlotType type;

	public static Armor FromKey (ArmorKey key) {
		Armor ret = new Armor();
		switch (key) {
		case ArmorKey.LeatherCap:
			ret = new Armor() {
				type = ArmorSlotType.Head,
				alterMovementPerActionPoint = 0,
				alterAttackRange = 0,
				alterDamageReduction = 1,
				alterAttackChance = 0.0f,
				alterEvade = 0.0f,
				alterDamageBase = 0.0f,
				alterDamageRollSides = 0.0f
			};
			break;
		case ArmorKey.IronHelm:
			ret = new Armor() {
				type = ArmorSlotType.Head,
				alterMovementPerActionPoint = -1,
				alterAttackRange = 0,
				alterDamageReduction = 3,
				alterAttackChance = 0.0f,
				alterEvade = -0.15f,
				alterDamageBase = 0.0f,
				alterDamageRollSides = 0.0f
			};
			break;
		case ArmorKey.MagicianHat:
			ret = new Armor() {
				type = ArmorSlotType.Head,
				alterMovementPerActionPoint = 1,
				alterAttackRange = 0,
				alterDamageReduction = 1,
				alterAttackChance = 0.0f,
				alterEvade = 0.15f,
				alterDamageBase = 0.0f,
				alterDamageRollSides = 0.0f
			};
			break;
		case ArmorKey.LeatherVest:
			ret = new Armor() {
				type = ArmorSlotType.Chest,
				alterMovementPerActionPoint = 0,
				alterAttackRange = 0,
				alterDamageReduction = 0,
				alterAttackChance = 0.0f,
				alterEvade = 0.0f,
				alterDamageBase = 0.0f,
				alterDamageRollSides = 0.0f
			};
			break;

		case ArmorKey.IronPlate:
			ret = new Armor() {
				type = ArmorSlotType.Chest,
				alterMovementPerActionPoint = -2,
				alterAttackRange = 0,
				alterDamageReduction = 3,
				alterAttackChance = 0.0f,
				alterEvade = -0.15f,
				alterDamageBase = 0.0f,
				alterDamageRollSides = 0.0f
			};
			break;
		case ArmorKey.MagicianRobe:
			ret = new Armor() {
				type = ArmorSlotType.Chest,
				alterMovementPerActionPoint = 2,
				alterAttackRange = 0,
				alterDamageReduction = 3,
				alterAttackChance = 0.0f,
				alterEvade = 0.15f,
				alterDamageBase = 0.0f,
				alterDamageRollSides = 0.0f
			};
			break;
		case ArmorKey.LeatherGauntlet:
			ret = new Armor() {
				type = ArmorSlotType.Gauntlet,
				alterMovementPerActionPoint = 0,
				alterAttackRange = 0,
				alterDamageReduction = 1,
				alterAttackChance = 0.0f,
				alterEvade = 0.0f,
				alterDamageBase = 0.0f,
				alterDamageRollSides = 0.0f
			};
			break;
		case ArmorKey.IronGauntlet:
			ret = new Armor() {
				type = ArmorSlotType.Gauntlet,
				alterMovementPerActionPoint = -2,
				alterAttackRange = 0,
				alterDamageReduction = 3,
				alterAttackChance = 0.0f,
				alterEvade = -0.15f,
				alterDamageBase = 0.0f,
				alterDamageRollSides = 0.0f
			};
			break;
		case ArmorKey.MagicianRing:
			ret = new Armor() {
				type = ArmorSlotType.Gauntlet,
				alterMovementPerActionPoint = 1,
				alterAttackRange = 0,
				alterDamageReduction = 0,
				alterAttackChance = 0.0f,
				alterEvade = 0.15f,
				alterDamageBase = 0.0f,
				alterDamageRollSides = 0.0f
			};
			break;
		case ArmorKey.IronBoots:
			ret = new Armor() {
				type = ArmorSlotType.Leg,
				alterMovementPerActionPoint = -2,
				alterAttackRange = 0,
				alterDamageReduction = 3,
				alterAttackChance = 0.0f,
				alterEvade = -0.15f,
				alterDamageBase = 0.0f,
				alterDamageRollSides = 0.0f
			};
			break;
		case ArmorKey.MagicianBoots:
			ret = new Armor() {
				type = ArmorSlotType.Leg,
				alterMovementPerActionPoint = 2,
				alterAttackRange = 0,
				alterDamageReduction = 1,
				alterAttackChance = 0.0f,
				alterEvade = 0.0f,
				alterDamageBase = 0.0f,
				alterDamageRollSides = 0.0f
			};
			break;
		}
		return ret;
	}
}
