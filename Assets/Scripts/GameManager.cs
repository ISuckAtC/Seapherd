using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public struct Settings
    {
        public PlayerController.ControlType controlType;
        
    }
    public static Settings _Settings;
    public static GameManager Instance;

    public int SheepCount, SheepTotal;
    public int CurrentMission, TotalMissionCompletion;
    public bool OptionalObjectiveCompleted;
    public TextMeshProUGUI MissionObjectiveText;
    public List<EntitySheep> FishSheep;
    public Transform Player;
    public TextMeshProUGUI ToolTipText;
    public TextMeshProUGUI RevealText;
    public bool alt;
    public bool artifactGET;
    public int DebugSkill;
    public bool StartBugStop;
    public bool GotoPlayer, RunFromPlayer, GotoMarker, GoUp;
    public bool InTavern, InMainMenu;
    public AudioMixer _AM;
    [Space(10)]
    public int SkillPoints = 0;
    public Dictionary<string, bool> SkillsUnlocked = new Dictionary<string, bool>();

    void Awake()
    {
        _AM = Resources.Load<AudioMixer>("MasterVolume");

        _Settings.controlType = PlayerController.ControlType.VR_Dragging;

        #region Dictionary for all the skills
        SkillsUnlocked.Add("PlayerSpeed-1", false);
        SkillsUnlocked.Add("PlayerSpeed-2", false);
        SkillsUnlocked.Add("DogfishHerdCommand-1", false);
        SkillsUnlocked.Add("DogfishWaitCommand-1", false);
        #endregion

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

        if (scene.buildIndex == 0) {
            //SceneManager.LoadScene(1, LoadSceneMode.Single);
            return;
        }
        Debug.Log("start");

        if (scene.buildIndex == 0)
        {
            InTavern = false;
            InMainMenu = true;
        }

        if (scene.buildIndex > 1)
        {
            FishSheep = GameObject.FindObjectsOfType<EntitySheep>().ToList();
            SheepCount = SheepTotal = FishSheep.Count;

            InTavern = false;
            InMainMenu = false;
        }
        if (scene.buildIndex == 1)
        {
            InTavern = true;
            InMainMenu = false;
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
        if (Input.GetKey(KeyCode.U))
        {
            switch (DebugSkill)
            {
                case 1:
                    GotoPlayer = true;
                 break;
                case 2:
                    RunFromPlayer = true;
                    break;
                case 3:
                    GotoMarker = true;
                    break;
                case 4:
                    GoUp = true;
                    break;
                case 5:
                    DebugSkill = 0;
                    GotoPlayer = false;
                    RunFromPlayer = false;
                    GotoMarker = false;
                    GoUp = false;
                    break;
            }
        }
    }

    public void SplashText(string text, int fontSize = 36)
    {
        RevealText.text = text;
        RevealText.fontSize = fontSize;
        RevealText.GetComponent<Animator>().Play("RevealText", -1, 0f);
    }

    public void ToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }


    // Basis for volume settings
    public void VolumeControl(float Value, string VolumeType)
    {
        switch (VolumeType)
        {
            case "Master":
                _AM.SetFloat("ExposedMasterParam", Value);
                break;

            case "Effect":
                _AM.SetFloat("ExposedEffectParam", Value);
                break;

            case "Music":
                _AM.SetFloat("ExposedMusicParam", Value);
                break;

            case "Voice":
                _AM.SetFloat("ExposedVoiceParam", Value);
                break;

            case "Ambiance":
                _AM.SetFloat("ExposedAmbianceParam", Value);
                break;
        }
    }

    public void UnlockSkill(string unlockedSkill)
    {
        if (SkillsUnlocked.ContainsKey(unlockedSkill))
        {
            SkillsUnlocked[unlockedSkill] = true;
        } else
        {
            Debug.LogError(unlockedSkill + " is not recognised.");
        }
    }
}
