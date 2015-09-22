# TurnBasedStrategyGame
Classic Turn Based Strategy Game made with Unity3D

## Map Generation
The map is generated using Cellular Automaton. A pseudo random string is used to consistently generate the same map. The map is then smoothed a couple of times. Disjoint rooms are connected together to make the cave-like map more realistic

## AI
Pathfinding algorithm is used to determine the location of a non-ai player. The AI then proceeds to move to their nearest attack range and attack the player.

## Highlighting
A highlighting system was used to show the valid action ranges (for movement or attacking)

## Weapons
A basic weapon system was implemented to modify base stats

## Future
* Item/Weapon pickups
* Create team based system
* Create a level system for characters
* Persist data in a saved state
* Smarter AI
* Fog of war
* Mission objectives
* Create multiple stages
