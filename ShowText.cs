using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowText : MonoBehaviour
{
    public string text;
    public Text textElement;
    public GameManager gm; 

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        textElement.text = text;
    }

    // Update is called once per frame
    void Update()
    {
        textElement.text = "NUMBER OF TURNS PASSED: " + gm.TurnsPlayed;
    }
}
