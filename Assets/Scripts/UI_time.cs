using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_time : MonoBehaviour {

    public static event System.Action OnTimeOut;
    public static UI_time instance;
    private float start;
    private Text txt;

    private void Awake()
    {
        instance = this;
    }
    private void OnEnable()
    {
        GameController.OnRun += GameController_OnRun;
    }


    private void OnDisable()
    {
        GameController.OnRun -= GameController_OnRun;
    }

    void Start ()
    {
        txt = GetComponent<Text>();
        txt.text = "";
    }	
    
    private void GameController_OnRun()
    {
        txt = GetComponent<Text>();
        txt.text = "Time: 0";
        start = Time.time;
        StartCoroutine(TimeCounter());
    }

    private IEnumerator TimeCounter()
    {
        while (true)
        {
            float time = Time.time - start;
            TimeSpan total = new TimeSpan(0, 0, 0, (int)time);
            if (time > 120)
            {
                if (OnTimeOut != null)
                {
                    OnTimeOut();
                }
                yield break;
            }
            txt.text = string.Format("Time: {0}:{1}", total.Minutes.ToString("00"), total.Seconds.ToString("0#."));

            yield return new WaitForSeconds(0.5f);
        }
    }
}
