using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public struct WaypointSpawn
{
    public Transform SpawnPosition;
    public GameObject SpawnObject;
}
public class MissionWaypoint : MonoBehaviour
{

    public MissionHerd ParentNavigator;
    BoxCollider triggerCollider;
    public bool Override;
    public int SelfIndex;
    public WaypointSpawn[] Spawns;
    bool activated;
    bool active;

    public void Start()
    {
        triggerCollider = GetComponent<BoxCollider>();
    }
    public void Activate()
    {
        active = true;
        GetComponent<Collider>().enabled = true;
        GetComponent<MeshRenderer>().enabled = true;
        foreach (WaypointSpawn spawn in Spawns)
        {
            Instantiate(spawn.SpawnObject, spawn.SpawnPosition.position, spawn.SpawnPosition.rotation);
        }
    }
    public void Deactivate()
    {
        active = false;
        GetComponent<Collider>().enabled = false;
        GetComponent<MeshRenderer>().enabled = false;
    }

    public void FixedUpdate()
    {
        if ((!active) || activated || Override) return;
        Collider[] hits = Physics.OverlapBox(transform.position, Vector3.Scale(triggerCollider.size, transform.localScale) / 2f, transform.rotation, (1 << LayerMask.NameToLayer("Sheep")));
        if (hits.Length > 0)
        {
            Debug.Log("Hit " + hits.Length + " sheep");
            List<EntitySheep> sheep = hits.Select(x => x.transform.GetComponent<EntitySheep>()).Where(x => x.PartOfMission.Contains(ParentNavigator.MissionName)).ToList();
            if (sheep.Count >= (((int, int))GameManager.Instance.Missions[ParentNavigator.MissionName].Extras).Item1)
            {
                ParentNavigator.Activated(SelfIndex);
                activated = true;
            }
        }
    }
}
