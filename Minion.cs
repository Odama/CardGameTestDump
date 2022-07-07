using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// minion for now, should be renamed Character for consistency with original plans
// not a priority at the moment.
public class Minion : Card, ICombatant, IDestructable {
	
	// ICombatant
	public int attack { get; set; }
	public int remainingAttacks { get; set; }
	public int allowedAttacks { get; set; }

	// IDestructable
	public int hitPoints { get; set; }
	public int maxHitPoints { get; set; }

	// Other
	public List<string> mechanics;
	public string race;

	public override void Load (Dictionary<string, object> data) {
		base.Load (data);
		// only have to worry about loading this for minions/characters
		attack = System.Convert.ToInt32 (data["attack"]);
		hitPoints = maxHitPoints = System.Convert.ToInt32 (data["hit points"]);
		//by default one attack, but could change this in the future for other minions/characters.
		allowedAttacks = 1;
	}
}