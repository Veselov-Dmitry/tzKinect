using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static event Action OnWalkEnable;
    public static event Action OnWalkDisable;
    public static PlayerController instance;
    public static bool IsTake { get { return _IsTake; } }
    public static bool IsMoving { get { return _IsMoving; } }
    private int _WalkRemamber;
    private int _MaxWalkRemember = 3;

    private static bool _IsMoving = false;
    private static bool _IsTake = false;

    private Animator anim;
    private Coroutine cor;
    private object _AnimPlay;

    private void Awake()
    {
        instance = this;
    }
    private void OnEnable()
    {
        _WalkRemamber = 0;
        _IsTake = false;
        InputBase.OnHandUp += Take;
        InputBase.OnLeftHandUp += Left;
        InputBase.OnRightHandUp += Rigth;
        InputBase.OnJump += Walk;
        InputBase.OnTwoHandsUp += TPose;
        InputBase.OnTwoHandsDown += Idle;
        PrimitiveScript.OnCollisionBarrier += PrimitiveScript_OnCollisionBarrier;
    }


    private void OnDisable()
    {
        InputBase.OnHandUp -= Take;
        InputBase.OnLeftHandUp -= Left;
        InputBase.OnRightHandUp -= Rigth;
        InputBase.OnJump -= Walk;
        InputBase.OnTwoHandsUp -= TPose;
        InputBase.OnTwoHandsDown -= Idle;
        PrimitiveScript.OnCollisionBarrier += PrimitiveScript_OnCollisionBarrier;
    }

    private void GameController_OnLevel2()
    {
        Idle();
    }

    private void Start()
    {
        _IsMoving = false;
        _AnimPlay = null;
        anim = GetComponent<Animator>();
    }
    [ContextMenu("+++Walk")]
    private void Walk()
    {
        if (GameController.St == GameController.State.Run)
        {
            if (_WalkRemamber <= 0)
            {
                anim.SetInteger("TPose", 0);
                anim.SetBool("Walk", true);
                _IsMoving = true;
                if (OnWalkEnable != null)
                    OnWalkEnable();
                _WalkRemamber = 1;
            }
            else if (_WalkRemamber < _MaxWalkRemember)
            {
                _WalkRemamber++;
            }
        }
    }


    [ContextMenu("+++GoLeft")]
    private void Left()
    {
        if (GameController.St == GameController.State.Run)
        {
            if (transform.position.x > -0.5f)
            {
                if (_AnimPlay == null)
                {
                    anim.SetTrigger("GoLeft");
                    _AnimPlay = new object();
                }

            }
        }
        else
        if (GameController.St == GameController.State.Level2)
        {
            anim.SetInteger("TPose", 3);
        }
    }

    [ContextMenu("+++GoRight")]
    private void Rigth()
    {
        if (GameController.St == GameController.State.Run)
        {
            if (transform.position.x < 0.5f)
            {
                if (_AnimPlay == null)
                {
                    anim.SetTrigger("GoRight");
                    _AnimPlay = new object();
                }
            }
        }
        else if (GameController.St == GameController.State.Level2)
        {
            anim.SetInteger("TPose", 2);
        }
    }
    [ContextMenu("+++Take")]
    private void Take()
    {
        if (GameController.St == GameController.State.Run)
        {
            if (!_IsTake)
            {
                anim.SetTrigger("Take");
                _IsTake = true;
                cor = StartCoroutine(GetAnimaionTakeState());
            }
        }
    }


    [ContextMenu("+++Idle")]
    private void Idle()
    {
        anim.SetInteger("TPose", 0);
    }
    [ContextMenu("+++TPose")]
    private void TPose()
    {
        if (GameController.St == GameController.State.Level2)
        {
            anim.SetBool("Walk", false);
            anim.SetTrigger("Idle");
            anim.SetInteger("TPose", 1);
        }
    }
    [ContextMenu("+++LeftHand")]
    private void LeftHand()
    {
        anim.SetInteger("TPose", 3);
    }
    [ContextMenu("+++RightHand")]
    private void RightHand()
    {
        anim.SetInteger("TPose", 2);
    }
    private void PrimitiveScript_OnCollisionBarrier(PrimitiveScript obj)
    {
        if (_AnimPlay == null)
        {
            _IsTake = false;
            anim.SetBool("Fall", true);
            _AnimPlay = new object();
        }
    }

    public void SetNotFall()
    {
        anim.SetBool("Fall", false);
        SetNotWalk();
        SetBlockGoMove();
    }

    public void SetNotWalk()
    {
        if (_WalkRemamber <= 0)
        {
            anim.SetBool("Walk", false);
            _IsMoving = false;
            if (OnWalkDisable != null)
                OnWalkDisable();
        }
        _WalkRemamber--;
    }
    public void SetBlockGoMove()
    {
        _AnimPlay = null;
    }

    private IEnumerator GetAnimaionTakeState()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);
            _IsTake = false;
            break;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color32(255, 20, 20, 255);
        Gizmos.DrawWireCube(
            new Vector3(
                transform.position.x
                , transform.position.y + 2.2f
                , transform.position.z)
            , new Vector3(
            GetComponent<Collider>().bounds.size.x
            , GetComponent<Collider>().bounds.size.y
            , GetComponent<Collider>().bounds.size.z
            ));
    }


}

