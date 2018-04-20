using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveChunk : MonoBehaviour
{
    public static event System.Action OnAciveSwitch;
    public static MoveChunk instance;
    public static List<PrimitiveScript> Childrens = new List<PrimitiveScript>();

    [SerializeField] private float _MoveSpeed;
    private Rigidbody _Rig;

    private void Awake()
    {
        instance = this;
        Childrens.Clear();
        _Rig = GetComponent<Rigidbody>();
    }
    private void Reset()
    {
        _MoveSpeed = 5f;
    }
    private void OnAciveSwitchMethod()
    {
        if(OnAciveSwitch != null)
            OnAciveSwitch();
    }
    private void OnEnable()
    {
        if(PlayerController.IsMoving)
            Move();
        PlayerController.OnWalkEnable += Move;
        PlayerController.OnWalkDisable += Stop;
        GameController.OnLevel2 -= Stop;
        transform.position = new Vector3(0,0,30f);
        OnAciveSwitchMethod();
        EnableAllChildrens();
    }

    private void OnDisable()
    {
        PlayerController.OnWalkEnable -= Move;
        PlayerController.OnWalkDisable -= Stop;
        GameController.OnLevel2 -= Stop;
    }

    void Start ()
    {
        _MoveSpeed = 5f;
        GameController.listChunks.Add(this);
        gameObject.SetActive(false);
	}

    private void EnableAllChildrens()
    {

       
        foreach (var item in transform.GetComponentsInChildren<PrimitiveScript>())
        {
            if (item!=null)
            {
                MeshRenderer mr = item.gameObject.GetComponent<MeshRenderer>();
                if (mr != null)
                    mr.enabled = true;
                Collider cl = item.gameObject.GetComponent<Collider>();
                if (cl != null)
                    cl.enabled = true;
            }
        }
    }
    
    
    public void Move()
    {
        //StartCoroutine(MovementChunk());
        _Rig.velocity = Vector3.back * _MoveSpeed;
    }
    public void Stop()
    {
        //StopAllCoroutines();
        if(_Rig)
            _Rig.velocity = Vector3.zero;
    }

    private IEnumerator MovementChunk()
    {
        while (true)
        {
            transform.position = new Vector3(0,0, 3f * _MoveSpeed * Time.deltaTime * Time.timeScale);
            yield return null;
        }
    }
}
