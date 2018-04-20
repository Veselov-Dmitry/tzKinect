using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_counter : MonoBehaviour
{
    private Text txt;

    public static UI_counter instance;

    private void Awake()
    {
        instance = this;
    }

    private void Reset()
    {
        GetComponent<Text>().text = "Count: 0";
    }
    private void OnEnable()
    {
        PrimitiveScript.OnCollisionBunus += PrimitiveScript_OnCollisionBunus;
        PrimitiveScript.OnCollisionBarrier += PrimitiveScript_OnCollisionBunus;
        GameController.OnRun += GameController_OnRun;
    }


    private void OnDisable()
    {
        PrimitiveScript.OnCollisionBunus -= PrimitiveScript_OnCollisionBunus;
        PrimitiveScript.OnCollisionBarrier -= PrimitiveScript_OnCollisionBunus;
        GameController.OnRun -= GameController_OnRun;
    }
    void Start ()
    {
        txt = GetComponent<Text>();
        txt.text = "";
    }
    private void GameController_OnRun()
    {
        GetComponent<Text>().text = "Count: 0";
    }

    private void PrimitiveScript_OnCollisionBunus(PrimitiveScript obj)
    {
        txt.text = "Count:"  + GameController.instance._Count.ToString();
    }
    
}
