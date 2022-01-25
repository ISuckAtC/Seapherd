using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionNavigate : MonoBehaviour
{
    public Transform[] Waypoints;
    int currentIndex;

    // Start is called before the first frame update
    void Start()
    {
        SetPoints();
        Waypoints[currentIndex].GetComponent<MissionWaypoint>().Activate();
        GameManager.Instance.MissionObjectiveText.text = "Navigate the sheep through the blue boxes (" + (currentIndex + 1) + "/" + Waypoints.Length + ")";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPoints()
    {
        for (int i = 0; i < Waypoints.Length; i++)
        {
            Waypoints[i].GetComponent<MissionWaypoint>().ParentNavigator = this;
            Waypoints[i].GetComponent<MissionWaypoint>().Deactivate();
        }
    }

    public void Acvivated()
    {
        Debug.Log("Waypoint activated");
        if (currentIndex < Waypoints.Length)
        {
            Waypoints[currentIndex++].GetComponent<MissionWaypoint>().Deactivate();
            Waypoints[currentIndex].GetComponent<MissionWaypoint>().Activate();
            GameManager.Instance.MissionObjectiveText.text = "Navigate the sheep through the blue boxes (" + (currentIndex + 1) + "/" + Waypoints.Length + ")";
        }
    }
}
