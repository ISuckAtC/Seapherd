using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionWalk : Mission
{
    public GameObject[] TutorialPoints;
    public string[] TutorialTexts;
    public FMOD.Studio.EventInstance[] TutorialSounds;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < TutorialPoints.Length; ++i)
        {
            MissionTutorialPoint point = TutorialPoints[i].GetComponent<MissionTutorialPoint>();
            point.PointReachedCallback += PointReachedCallback;
            point.Index = i;
            //TutorialPoints[i].SetActive(false);
        }
        if (TutorialPoints.Length > 0)
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
        TutorialPoints[index].SetActive(false);
        if (index + 1 >= TutorialPoints.Length)
        {
            Continue();
        }
        else
        {
            //TutorialPoints[index + 1].SetActive(true);
        }
        // Show the text for that index
        
        // Play the sound for that index
        
    }
}
