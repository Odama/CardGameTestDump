using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : Card, IArmored, ICombatant, IDestructable {
	
	// IArmored
	// for now armor does nothing, but could be useful for future work
	public int armor { get; set; }

	// ICombatant
	// also not implemented is attacking with your hero, but should be added
	// along with armor. Need to implement weapon cards first.
	public int attack { get; set; }
	public int remainingAttacks { get; set; }
	public int allowedAttacks { get; set; }

	// IDesructable
	public int hitPoints { get; set; }
	public int maxHitPoints { get; set; }
}