using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class EndTurn : NetworkBehaviour
{
    public PlayerManager playerManager;
    public GameManager gameManager;

    public void OnClick()
    {
        NetworkIdentity networkIdentity = NetworkClient.connection.identity;
        playerManager = networkIdentity.GetComponent<PlayerManager>();

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameManager.IncrementTurnsPlayed();
    }
}
