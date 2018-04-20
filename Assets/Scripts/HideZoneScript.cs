using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideZoneScript : MonoBehaviour
{
    public static event System.Action OnReachEnd;

    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.SetActive(false);
        if(OnReachEnd != null)
        {
            OnReachEnd();
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color32(255, 20, 20, 255);
        Gizmos.DrawWireCube(
            new Vector3(
                transform.position.x
                , transform.position.y
                , transform.position.z-0.5f)
            , new Vector3(
            GetComponent<BoxCollider>().bounds.size.x
            , GetComponent<BoxCollider>().bounds.size.y
            , GetComponent<BoxCollider>().bounds.size.z
            ));
    }
}
