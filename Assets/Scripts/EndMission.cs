using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class EndMission : MonoBehaviour
{
    public GameManager GM;
    public MissionGiver MissionGiving;
    void Start()
    {
        GM = GameManager.Instance;
        MissionGiving = gameObject.GetComponent<MissionGiver>();
        if(MissionGiving == null)
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
        //HardCoded in the first one just to make sure it actually works honestly
        if (Input.GetAxisRaw("RShoulder") > 0.5f || Input.GetKeyUp(KeyCode.Space))
        {
            if(MissionGiving.MissionGiverNumber == 0 && GM.Missions["tutorial-p1"].status == GameManager.MissionStatus.Handin)
            {
                GM.Missions["tutorial-p1"].SetStatus(GameManager.MissionStatus.Completed);
                GM.Missions["tutorial-p2"].SetStatus(GameManager.MissionStatus.NotStarted);
            }
              if (MissionGiving.MissionGiverNumber == 1  && GM.Missions[GM.storedMissionName].status == GameManager.MissionStatus.Handin)
            {
                GM.Missions[GM.storedMissionName].SetStatus(GameManager.MissionStatus.Completed);
                GM.CurrentMission++;
                GM.Missions[GM.storedMissionName].SetStatus(GameManager.MissionStatus.NotStarted);
            }
        }
    }
}
