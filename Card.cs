using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card {
	public string id;
	public string name;
	public string text;
	public int cost;
	public int orderOfPlay = int.MaxValue;
	public int ownerIndex;
	public Zones zone = Zones.Deck;

	// load using miniJson from txt file
	public virtual void Load (Dictionary<string, object> data) {
		id = (string)data ["id"];
		name = (string)data ["name"];
		text = (string)data ["text"];
		// needs to be loaded as an integer
		cost = System.Convert.ToInt32(data["cost"]);
	}
}