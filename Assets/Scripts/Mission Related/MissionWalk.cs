using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD;

public class MissionWalk : Mission
{
    public FMODUnity.EventReference[] VoiceLines;
    public GameObject[] WalkPoints;
    public string[] PointTexts;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        UnityEngine.Debug.Log("MissionWalk Start (Mission Name: " + MissionName + ")");
        for (int i = 0; i < WalkPoints.Length; ++i)
        {
            MissionWalkPoint point = WalkPoints[i].GetComponent<MissionWalkPoint>();
            point.PointReachedCallback += PointReachedCallback;
            point.Index = i;
            //TutorialPoints[i].SetActive(false);
        }
        if (WalkPoints.Length > 0)
        {
            //TutorialPoints[0].SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PointReachedCallback(int index)
    {
        WalkPoints[index].SetActive(false);
        if (index + 1 >= WalkPoints.Length)
        {
            Continue();
        }
        else
        {
            //TutorialPoints[index + 1].SetActive(true);
        }
        // Show the text for that index

        // Play the sound for that index
        
        if (index < VoiceLines.Length && !VoiceLines[index].IsNull)
        {
            GameManager.FMODPlayOnceEvent(VoiceLines[index], GameManager.Instance.Player.position, Vector3.zero, true, true);
        }
    }
}
