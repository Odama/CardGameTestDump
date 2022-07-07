using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class TargetClick : NetworkBehaviour
{
    public PlayerManager playerManager;

    public void OnTargetClick()
    {
        NetworkIdentity identity = NetworkClient.connection.identity;
        playerManager = identity.GetComponent<PlayerManager>();

        if (hasAuthority)
        {
            playerManager.CmdTargetSelfCard();
        }
        else
        {
            playerManager.CmdTargetOtherCard(gameObject);
        }
    }
}
