using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerManager : NetworkBehaviour
{
    public GameObject Card1;
    public GameObject Card2;
    public GameObject PlayerArea;
    public GameObject OpponentArea;
    public GameObject PlayerMain;
    public GameObject OpponentMain;

    List<GameObject> Cards = new List<GameObject>();
    List<GameObject> CardsOnBoard = new List<GameObject>();

    public override void OnStartClient()
    {
        base.OnStartClient();

        PlayerArea = GameObject.Find("PlayerArea");
        OpponentArea = GameObject.Find("OpponentArea");
        PlayerMain = GameObject.Find("PlayerMain");
        OpponentMain = GameObject.Find("OpponentMain");
    }

    [Server]
    public override void OnStartServer()
    {
        base.OnStartServer();

        Cards.Add(Card1);
        Cards.Add(Card2);
        //Debug.Log(Cards);
    }

    [Command]
    public void CmdDealCards()
    {
        for (int i = 0; i < 5; i++)
        {
            // Randomize cards drawn
            GameObject card = Instantiate(Cards[Random.Range(0, Cards.Count)], new Vector3(0, 0), Quaternion.identity);
            NetworkServer.Spawn(card, connectionToClient);
            RpcShowCard(card, "Dealt");
        }
    }

    public void PlayCard(GameObject card)
    {
        CmdPlayCard(card);
    }

    [Command]
    void CmdPlayCard(GameObject card)
    {
        RpcShowCard(card, "Played");
        if (isServer)
        {
            UpdateTurnsPlayed();
        }
        CardsOnBoard.Add(card);
    }

    [Server]
    void UpdateTurnsPlayed()
    {
        GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        gm.IncrementTurnsPlayed();
        RpcLogToClients("Turns Played: " + gm.TurnsPlayed);
    }

    [ClientRpc]
    void RpcLogToClients(string message)
    {
        Debug.Log(message);
    }

    [ClientRpc]
    void RpcShowCard(GameObject card, string type)
    {
        if (type == "Dealt")
        {
            // gives ownership of cards drawn to the person who drew them
            if (hasAuthority)
            {
                card.transform.SetParent(PlayerArea.transform, false);
            }
            else
            {
                card.transform.SetParent(OpponentArea.transform, false);
                // opponent can only see the card back while cards in hand
                card.GetComponent<CardFlipper>().Flip();
            }
        }
        if (type == "Played")
        {
            card.transform.SetParent(PlayerMain.transform, false);
            // opponent should be able to see the card when played
            if (!hasAuthority)
            {
                card.GetComponent<CardFlipper>().Flip();
            }
        }
    }

    [Command]
    public void CmdTargetSelfCard()
    {
        TargetSelfCard();
    }

    [Command]
    public void CmdTargetOtherCard(GameObject target)
    {
        NetworkIdentity opponentIdentity = target.GetComponent<NetworkIdentity>();
        // lets the network know that the card is not owned by the player
        TargetOtherCard(opponentIdentity.connectionToClient);
    }

    [TargetRpc]
    void TargetSelfCard()
    {
        Debug.Log("Targeting own card");
    }

    [TargetRpc]
    void TargetOtherCard(NetworkConnection target)
    {
        Debug.Log("Targeted by opponent");
        if (target.identity.GetComponent<PlayerManager>().CardsOnBoard.Count <= 0 || CardsOnBoard.Count <= 0)
        {
            return;
        }
        if (CardsOnBoard[0].GetComponent<CardStats>().attack > target.identity.GetComponent<CardStats>().defense)
        {
            target.identity.GetComponent<PlayerManager>().CardsOnBoard.RemoveAt(0);
        }
    }

    [Command]
    public void CmdIncrementClick(GameObject card)
    {
        RpcIncrementClick(card);
    }

    [ClientRpc]
    void RpcIncrementClick(GameObject card)
    {
        card.GetComponent<IncrementClick>().NumberOfClicks++;
    }
}
