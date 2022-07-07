using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class DragDrop : NetworkBehaviour
{
    public GameObject Canvas;
    public PlayerManager playerManager;

    private bool isDragging = false;
    private bool isDraggable = false;
    private bool isPlayable = true;
    private GameObject startParent;
    private Vector2 startPosition;
    private GameObject mainZone;
    private bool isOverMainZone;

    // Start is called before the first frame update
    void Start()
    {
        Canvas = GameObject.Find("Main Canvas");

        // Only the player can drag owned cards
        if(hasAuthority)
        {
            isDraggable = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(isDragging)
        {
            transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            transform.SetParent(Canvas.transform, true);
        }
    }

    public void StartDrag()
    {
        if (!isDraggable && isPlayable)
        {
            return;
        }

        isDragging = true;
        startParent = transform.parent.gameObject;
        startPosition = transform.position;
    }

    public void EndDrag()
    {
        if (!isDraggable)
        {
            return;
        }

        // Used to ensure that a card can meets its condition/cost to be played.
        //GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();

        isDragging = false;
        if(isOverMainZone)// && GetComponent<CardStats>().cost <= gm.TurnsPlayed)
        {
            transform.SetParent(mainZone.transform, false);
            isDraggable = false;
            NetworkIdentity networkIdentity = NetworkClient.connection.identity;
            playerManager = networkIdentity.GetComponent<PlayerManager>();
            playerManager.PlayCard(gameObject);
        }
        else
        {
            transform.position = startPosition;
            transform.SetParent(startParent.transform, false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("Colliding!");
        isOverMainZone = true;
        mainZone = collision.gameObject;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //Debug.Log("Stop Colliding!");
        isOverMainZone = false;
        mainZone = null;
        // potential logic here for multiple zones
    }
}
