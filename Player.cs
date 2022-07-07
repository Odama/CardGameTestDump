using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player {
	public const int maxDeck = 30;
	public const int maxHand = 10;
	public const int maxBattlefield = 7;

	public readonly int index;
	public ControlModes mode;
	public Mana mana = new Mana ();

	public List<Card> hero = new List<Card> (1);
	//public List<Card> weapon = new List<Card> (1);
	public List<Card> deck = new List<Card> (maxDeck);
	public List<Card> hand = new List<Card> (maxHand);
	public List<Card> battlefield = new List<Card> (maxBattlefield);
	// potentially add a list for trap cards in the future.

	public List<Card> this[Zones zone] {
		get {
			switch (zone) {
				case Zones.Hero:
					return hero;

				/* Not implemented yet
				case Zones.Weapon:
					return weapon; */

				case Zones.Deck:
					return deck;

				case Zones.Hand:
					return hand;

				case Zones.Battlefield:
					return battlefield;

				default:
					return null;
			}
		}
	}

	public Player (int index) {
		this.index = index;
	}
}