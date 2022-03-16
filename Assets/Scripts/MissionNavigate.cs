using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionNavigate : MonoBehaviour
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

    // Update is called once per frame
    void Update()
    {
        if (next)
        {
            GameManager.Instance.alt = true;
            UnityEngine.SceneManagement.SceneManager.LoadScene("TestTavern");

            GameManager.Instance.TotalMissionCompletion++;
        }
    }

    public void SetPoints()
    {
        for (int i = 0; i < Waypoints.Length; i++)
        {
            MissionWaypoint waypoint = Waypoints[i].GetComponent<MissionWaypoint>();;
            waypoint.ParentNavigator = this;
            waypoint.Deactivate();
            waypoint.SelfIndex = i;
            
        }
    }

    public void Acvivated()
    {
        Debug.Log("Waypoint " + currentIndex + " activated");
        if (++currentIndex < Waypoints.Length)
        {
            Waypoints[currentIndex - 1].GetComponent<MissionWaypoint>().Deactivate();
            Waypoints[currentIndex].GetComponent<MissionWaypoint>().Activate();
            if (currentIndex < 8) GameManager.Instance.MissionObjectiveText.text = "Navigate the sheep through the blue boxes to reach the grazing area (" + (currentIndex + 1) + "/" + Waypoints.Length + ")";
            else GameManager.Instance.MissionObjectiveText.text = "Herd your sheep back home! (" + (currentIndex + 1) + "/" + Waypoints.Length + ")";
        }
        else
        {
            GameManager.Instance.MissionObjectiveText.text = "COMPLETE!";
            new System.Threading.Thread(() => 
            {
                System.Threading.Thread.Sleep(1000);
                if (GameManager.Instance.SheepCount > GameManager.Instance.SheepTotal / 2f)
                {
                    GameManager.Instance.TotalMissionCompletion++;
                }
                next = true;
            }).Start();
        }
    }
}
