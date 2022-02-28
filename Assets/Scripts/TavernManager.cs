using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TavernManager : MonoBehaviour
{
    GameManager GM;
    public int CurrentMissionAvailible;
    [Header("remember to put new questgivers into the array")]
    [Tooltip("Handles the amount of questgivers")]
    public GameObject[] QuestGivers;
    // Start is called before the first frame update
    void Start()
    {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
     
        

        foreach (GameObject QuestGiver in QuestGivers)
        {
            QuestGiver.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (QuestGivers.Length < CurrentMissionAvailible)
        {
            goto NoMoreMissions;
        }
        while (CurrentMissionAvailible < GM.TotalMissionCompletion + 1)
        {
            
            QuestGivers[CurrentMissionAvailible].gameObject.SetActive(true);
            CurrentMissionAvailible++;
        
        }
    NoMoreMissions:;
    }
}
