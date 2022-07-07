using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class CardZoom : NetworkBehaviour
{
    public GameObject Canvas;
    public GameObject ZoomCard;

    private GameObject zoomCard;
    private Sprite zoomSprite;

    public void Awake()
    {
        Canvas = GameObject.Find("Main Canvas");
        zoomSprite = gameObject.GetComponent<Image>().sprite;
    }

    public void OnHoverEnter()
    {
        if (!hasAuthority)
        {
            return;
        }

        zoomCard = Instantiate(ZoomCard, new Vector3(Input.mousePosition.x, Input.mousePosition.y + 250), Quaternion.identity);
        zoomCard.GetComponent<Image>().sprite = zoomSprite;
        zoomCard.transform.SetParent(Canvas.transform, true);
        RectTransform rectTransform = zoomCard.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(220, 320);
    }

    public void OnHoverExit()
    {
        Destroy(zoomCard);
    }
}
