using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Match {
	public const int PlayerCount = 2;

	public List<Player> players = new List<Player> (PlayerCount);
	public int currentPlayerIndex;

	public Player CurrentPlayer {
		get {
			return players [currentPlayerIndex]; 
		}
	}

	public Player OpponentPlayer {
		get {
			// opponent is 0, returns 1. opponent is 1, returns 0.
			return players [1 - currentPlayerIndex];
		}
	}

	public Match () {
		for (int i = 0; i < PlayerCount; ++i) {
			players.Add (new Player (i));
		}
	}
}