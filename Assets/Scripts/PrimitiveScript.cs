using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimitiveScript : MonoBehaviour
{

    public static event System.Action<PrimitiveScript> OnCollisionBarrier;
    public static event System.Action<PrimitiveScript> OnCollisionBunus;
    public enum TypePrimitive { Bonus , Barrier};
    [SerializeField] private TypePrimitive _Type;

    private List<Material> _Mat = new List<Material>();
    private bool _IsTaked;
    MeshRenderer _MeshRen;
    Collider _Coll;

    //private Vector3 savePosition;
    private void OnEnable()
    {
        _IsTaked = true;
    }

    private void Reset()
    {
        _Type = TypePrimitive.Barrier;
    }
    void Start ()
    {
        _MeshRen = GetComponent<MeshRenderer>();
        _Coll = GetComponent<Collider>();

        if (_Mat.Count == 0)
        {
            _Mat.AddRange(GameController.instance.Mat);
        }

        if (_Type == TypePrimitive.Barrier) GetComponent<Renderer>().material = _Mat[0];
        else GetComponent<Renderer>().material = _Mat[1];
    }

    private void OnTriggerEnter(Collider other)
    {
        Hit(other.gameObject);
    }
    
    private void OnTriggerStay(Collider other)
    {
        Hit(other.gameObject);
    }

    private void Hit(GameObject other)
    {
        if (other != null && _Coll.enabled)
        {
            if (other.tag == PlayerController.instance.gameObject.tag)
                if (_Type == TypePrimitive.Barrier)
                {
                    _Coll.enabled = false;
                    if (OnCollisionBarrier != null)
                        OnCollisionBarrier(this);
                }
                else if (PlayerController.IsTake)
                {
                    _Coll.enabled = false;
                    _MeshRen.enabled = false;
                    if (OnCollisionBunus != null)
                        OnCollisionBunus(this);
                }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color32(0, 255, 20, 255);
        Gizmos.DrawWireCube(
            new Vector3(
                transform.position.x
                , transform.position.y
                , transform.position.z)
            , new Vector3(
            GetComponent<Collider>().bounds.size.x
            , GetComponent<Collider>().bounds.size.y
            , GetComponent<Collider>().bounds.size.z
            ));
    }
}
