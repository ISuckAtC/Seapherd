using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slavos : MissionGiver
{
    public override void CompleteMission(string mission)
    {
        switch (mission)
        {
            case "Tutorial-p1":
                {
                    GM.Missions["Tutorial-p2"].Status = GameManager.MissionStatus.NotStarted;
                    break;
                }
            case "Tutorial-p2":
                {

                    break;
                }
        }
    }

    public override void StartMission(string mission)
    {
        base.StartMission(mission);
    }

    public override void RemindMission(string mission)
    {
        base.RemindMission(mission);
    }
}
