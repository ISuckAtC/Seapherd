using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int SheepCount, SheepTotal;
    public int CurrentMission, TotalMissionCompletion;
    public bool OptionalObjectiveCompleted;
    public TextMeshProUGUI MissionObjectiveText;
    public EntitySheep[] FishSheep;
    public Transform Player;
    public TextMeshProUGUI ToolTipText;
    public TextMeshProUGUI RevealText;
    public bool alt;
    public bool artifactGET;

    void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 0) return;
        Debug.Log("start");
        if (scene.buildIndex > 1)
        {
            FishSheep = GameObject.FindObjectsOfType<EntitySheep>();
            SheepCount = SheepTotal = FishSheep.Length;
        }
        MissionObjectiveText = GameObject.Find("MissionObjectiveText").GetComponent<TextMeshProUGUI>();
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        ToolTipText = GameObject.Find("Tooltip").GetComponent<TextMeshProUGUI>();
        RevealText = GameObject.Find("RevealText").GetComponent<TextMeshProUGUI>();
        MissionObjectiveText = GameObject.Find("MissionObjectiveText").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SplashText(string text, int fontSize = 36)
    {
        RevealText.text = text;
        RevealText.fontSize = fontSize;
        RevealText.GetComponent<Animator>().Play("RevealText", -1, 0f);
    }
}
