using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatorScript : MonoBehaviour
{
    [Range(0.1f,10)]
    public float m_Speed;
    private void Start()
    {
        m_Speed = 3;
    }

    void Update ()
    {
        gameObject.transform.Rotate(Vector3.forward, m_Speed);
	}
}
