using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct WaypointSpawn
{
    public Transform SpawnPosition;
    public GameObject SpawnObject;
}
public class MissionWaypoint : MonoBehaviour
{

    public MissionNavigate ParentNavigator;
    BoxCollider triggerCollider;
    public bool Override;
    public int SelfIndex;
    public WaypointSpawn[] Spawns;
    bool activated;

    public void Start()
    {
        triggerCollider = GetComponent<BoxCollider>();
    }
    public void Activate()
    {
        GetComponent<Collider>().enabled = true;
        GetComponent<MeshRenderer>().enabled = true;
        foreach (WaypointSpawn spawn in Spawns)
        {
            Instantiate(spawn.SpawnObject, spawn.SpawnPosition.position, spawn.SpawnPosition.rotation);
        }
    }
    public void Deactivate()
    {
        GetComponent<Collider>().enabled = false;
        GetComponent<MeshRenderer>().enabled = false;
    }

    public void FixedUpdate()
    {
        if (activated || Override) return;
        RaycastHit[] hits = Physics.BoxCastAll(transform.position, Vector3.Scale(triggerCollider.size, transform.localScale) / 2f, Vector3.forward, transform.rotation, 0f, (1 << LayerMask.NameToLayer("Sheep")));
        if (hits.Length == GameManager.Instance.SheepCount)
        {
            ParentNavigator.Acvivated();
            activated = true;
        }
    }
}
