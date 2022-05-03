using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MissionHerd : Mission
{
    public Transform[] Waypoints;
    bool next;

    public List<EntitySheep> Sheep;

    // Start is called before the first frame update
    void Start()
    {
        Sheep = FindObjectsOfType<EntitySheep>().Where(x => x.PartOfMission.Contains(MissionName)).ToList();
        GameManager.Instance.Missions[MissionName].Extras = (Sheep.Count, Sheep.Count);
        foreach(EntitySheep sheep in Sheep) sheep.Killed += SheepKilled;
        SetPoints();
        Waypoints[0].GetComponent<MissionWaypoint>().Activate();
        //GameManager.Instance.MissionObjectiveText.text = "Navigate the sheep through the blue boxes to reach the grazing area (" + (currentIndex + 1) + "/" + Waypoints.Length + ")";
    }

    public void SheepKilled()
    {
        GameManager.Instance.Missions[MissionName].Extras = ((((((int, int))GameManager.Instance.Missions[MissionName].Extras).Item1 - 1), (((int, int))GameManager.Instance.Missions[MissionName].Extras).Item2));
    }

    public void SetPoints()
    {
        for (int i = 0; i < Waypoints.Length; i++)
        {
            MissionWaypoint waypoint = Waypoints[i].GetComponent<MissionWaypoint>(); ;
            waypoint.ParentNavigator = this;
            waypoint.Deactivate();
            waypoint.SelfIndex = i;

        }
    }

    public void Activated(int index)
    {
        Debug.Log("Waypoint " + index + " activated");
        Waypoints[index].GetComponent<MissionWaypoint>().Deactivate();
        if (index + 1 < Waypoints.Length)
        {
            Waypoints[index + 1].GetComponent<MissionWaypoint>().Activate();
        }
        else
        {
            Continue();
        }
    }
}
