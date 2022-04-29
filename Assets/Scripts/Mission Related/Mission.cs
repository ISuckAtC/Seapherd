using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mission : MonoBehaviour
{
    public string MissionName;
    public GameObject NextMission;

    public void Continue()
    {
        if (NextMission != null)
        {
            Instantiate(NextMission);
        }
        else
        {
            GameManager.Instance.Missions[MissionName].SetStatus(GameManager.MissionStatus.Handin);
        }

        Destroy(gameObject);
    }
}
