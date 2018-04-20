using System;
using System.Collections.Generic;
using UnityEngine;

public class InputKeyBoard : InputBase
{/*
    public static event Action OnHandUp;
    public static event Action OnRightHandUp;
    public static event Action OnLeftHandUp;
    public static event Action OnJump;
    public static event Action OnTwoHandsUp;
    public static event Action OnTwoHandsDown;/**/
    

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            LeftHandUp();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            RightHandUp();
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            HandUp();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            TwoHandsDown();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        if ((Input.GetKey(KeyCode.LeftArrow) && Input.GetKeyDown(KeyCode.RightArrow)) ||
            (Input.GetKey(KeyCode.RightArrow) && Input.GetKeyDown(KeyCode.LeftArrow)))
        {
            TwoHandsUp();
        }
    }
}
