using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mission : MonoBehaviour
{
    public string MissionName;
    public GameObject NextMission;
    public FMODUnity.EventReference MissionReturn, MissionAdvance;
    public void Continue()
    {
        if (NextMission != null)
        {
            GameManager.FMODPlayOnceEvent(MissionAdvance, transform.position, Vector3.zero);
            Instantiate(NextMission);
        }
        else
        {
            Debug.Log(MissionName + " completed!");
            GameManager.Instance.Missions[MissionName].Status = GameManager.MissionStatus.Handin;
            GameManager.FMODPlayOnceEvent(MissionReturn, transform.position, Vector3.zero);
        }

        Destroy(gameObject);
    }
}
