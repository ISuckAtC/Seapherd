using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int FishSheepTotal, MissionStartFishSheepTotal;
    public int CurrentMission;
    public bool OptionalObjectiveCompleted;
    public TextMeshProUGUI MissionObjectiveText;
    public Fish[] FishSheep;
    void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        MissionObjectiveText = GameObject.Find("MissionObjectiveText").GetComponent<TextMeshProUGUI>();
        FishSheep = GameObject.FindObjectsOfType<Fish>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
