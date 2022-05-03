using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class EndMission : MonoBehaviour
{
    public GameManager GM;
    public MissionGiver MissionGiving;
    bool heldLastFrame;
    void Start()
    {
        GM = GameManager.Instance;
        MissionGiving = gameObject.GetComponent<MissionGiver>();
        if (MissionGiving == null)
        {
            Debug.LogError("Attach this to the MissionGiver, ");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerStay(Collider other)
    {
        //Debug.Log("Stays in mission trigger");

        //HardCoded in the first one just to make sure it actually works honestly
        if ((Input.GetAxisRaw("RShoulder") > 0.5f || Input.GetKeyUp(KeyCode.Space)))
        {
            if (!heldLastFrame)
            {
                heldLastFrame = true;
                MissionGiving.Interact();
            }
            return;
            if (MissionGiving.MissionGiverNumber == 0 && GM.Missions["Tutorial-p1"].Status == GameManager.MissionStatus.Handin)
            {
                GM.Missions["Tutorial-p1"].Status = GameManager.MissionStatus.Completed;
                GM.Missions["Tutorial-p2"].Status = GameManager.MissionStatus.NotStarted;
            }
            if (MissionGiving.MissionGiverNumber == 1 && GM.Missions[GM.storedMissionName].Status == GameManager.MissionStatus.Handin)
            {
                GM.Missions[GM.storedMissionName].Status = GameManager.MissionStatus.Completed;
                GM.CurrentMission++;
                GM.Missions[GM.storedMissionName].Status = GameManager.MissionStatus.NotStarted;
            }
        } 
        else
        {
            heldLastFrame = false;
        }
    }
}
