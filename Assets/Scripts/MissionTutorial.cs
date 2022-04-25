using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionTutorial : MonoBehaviour
{
    public GameObject[] TutorialPoints;
    public string[] TutorialTexts;
    public FMOD.Studio.EventInstance[] TutorialSounds;

    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject point in TutorialPoints)
        {
            point.GetComponent<MissionTutorialPoint>().PointReachedCallback += PointReachedCallback;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PointReachedCallback(int index)
    {
        // Show the text for that index
        
        // Play the sound for that index
        
    }
}
