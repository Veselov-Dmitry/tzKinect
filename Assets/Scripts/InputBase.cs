using System;
using System.Collections.Generic;
using UnityEngine;

public class InputBase : MonoBehaviour
{
    public delegate void InputCommon();

    public static event Action OnHandUp;
    public static event Action OnRightHandUp;
    public static event Action OnLeftHandUp;
    public static event Action OnJump;
    public static event Action OnTwoHandsUp;
    public static event Action OnTwoHandsDown;

    protected void HandUp()
    {
        if (OnHandUp != null)
            OnHandUp();
    }
    protected void RightHandUp()
    {
        if (OnRightHandUp != null)
            OnRightHandUp();
    }
    protected void LeftHandUp()
    {
        if (OnLeftHandUp != null)
            OnLeftHandUp();
    }
    protected void Jump()
    {
        if (OnJump != null)
            OnJump();
    }
    protected void TwoHandsUp()
    {
        if (OnTwoHandsUp != null)
            OnTwoHandsUp();
    }
    protected void TwoHandsDown()
    {
        if (OnTwoHandsDown != null)
            OnTwoHandsDown();
    }


}
