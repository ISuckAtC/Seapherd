using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionNavigate : Mission
{
    public Transform[] Waypoints;
    public int currentIndex;
    bool next;

    // Start is called before the first frame update
    void Start()
    {
        SetPoints();
        Waypoints[currentIndex].GetComponent<MissionWaypoint>().Activate();
        GameManager.Instance.MissionObjectiveText.text = "Navigate the sheep through the blue boxes to reach the grazing area (" + (currentIndex + 1) + "/" + Waypoints.Length + ")";
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

    public void Activated()
    {
        Debug.Log("Waypoint " + currentIndex + " activated");
        if (++currentIndex < Waypoints.Length)
        {
            Waypoints[currentIndex - 1].GetComponent<MissionWaypoint>().Deactivate();
            Waypoints[currentIndex].GetComponent<MissionWaypoint>().Activate();
        }
        else
        {
            Continue();
        }
    }
}
