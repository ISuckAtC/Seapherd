using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionWaypoint : MonoBehaviour
{
    public MissionNavigate ParentNavigator;
    BoxCollider triggerCollider;
    public bool Override;
    public int SelfIndex;

    public void Start()
    {
        triggerCollider = GetComponent<BoxCollider>();
    }
    public void Activate()
    {
        GetComponent<Collider>().enabled = true;
        GetComponent<MeshRenderer>().enabled = true;
    }
    public void Deactivate()
    {
        GetComponent<Collider>().enabled = false;
        GetComponent<MeshRenderer>().enabled = false;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (Override) return;
        if (other.gameObject.layer == LayerMask.NameToLayer("Sheep"))
        {
            Debug.Log("Sheep entered waypoint");
            RaycastHit[] hits = Physics.BoxCastAll(transform.position, Vector3.Scale(triggerCollider.size, transform.localScale) / 2f, Vector3.forward, transform.rotation, 0f, (1 << LayerMask.NameToLayer("Sheep")));
            if (hits.Length == GameManager.Instance.SheepTotal)
            {
                ParentNavigator.Acvivated();
            }
        }
    }
}
