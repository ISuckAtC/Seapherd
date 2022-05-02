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

    public enum MissionStatus
    {
        Unavailable,
        NotStarted,
        InProgress,
        Handin,
        Completed,
        Failed
    }

    public static Settings _Settings;
    public static GameManager Instance;
    public Joystick_KC KC;
    public int FoundArtifacts, ConspiracyHandedIn, FatherHandedIn;
    public int SheepCount, SheepTotal;
    public int CurrentMission, TotalMissionCompletion;
    public bool OptionalObjectiveCompleted;
    public TextMeshProUGUI MissionObjectiveText;
    public List<EntitySheep> FishSheep;
    public Transform Player;
    public TextMeshProUGUI ToolTipText;
    public TextMeshProUGUI RevealText;
    public bool alt;
    public bool artifactGET, HDD, HDDConspiracy, HDDFather, HDDEasterEgg;
    public int DebugSkill;
    public bool StartBugStop;
    public bool GotoPlayer, RunFromPlayer, GotoMarker, GoUp;
    public bool InTavern, InMainMenu;
    public string storedMissionName;
    public AudioMixer _AM;
    [Space(10)]
    public int SkillPoints = 0;
    public Dictionary<string, bool> SkillsUnlocked = new Dictionary<string, bool>();
    public Dictionary<string, (MissionStatus status, object extras)> Missions = new Dictionary<string, (MissionStatus status, object extras)>();

    

    void Awake()
    {
        /*
            HELLO. READ ME.

            Regarding the extras for missions, its for stuff like total sheep to herd and such.
            To load multiple variables just use a tuple, make sure to remember what you load in
            so that you load the correct stuff when you access the tuple later on.


        */


        Missions.Add("Tutorial-p1", (MissionStatus.NotStarted, null));
        Missions.Add("Tutorial-p2", (MissionStatus.Unavailable, null));
        Missions.Add("Mission-1", (MissionStatus.Unavailable, null));
        Missions.Add("Mission-2", (MissionStatus.Unavailable, null));
        Missions.Add("Mission-3", (MissionStatus.Unavailable, null));
        Missions.Add("Mission-4", (MissionStatus.Unavailable, null));



        Missions["Tutorial-p1"].SetStatus(MissionStatus.InProgress);

        Missions["Mission-1"].SetExtras((5, 5));




        _AM = Resources.Load<AudioMixer>("MasterVolume");
        KC = gameObject.GetComponent<Joystick_KC>();
        _Settings.controlType = PlayerController.ControlType.VR_Dragging;
        KC.enabled = false;
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

        Debug.Log("Gamemanager awake finished");
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        
        if (scene.buildIndex == 0) {
            //SceneManager.LoadScene(1, LoadSceneMode.Single);
            return;
        }

        if (scene.buildIndex == 0)
        {
            InTavern = false;
            InMainMenu = true;
        }

        /*if (scene.buildIndex > 2)
        {
            FishSheep = GameObject.FindObjectsOfType<EntitySheep>().ToList();
            SheepCount = SheepTotal = FishSheep.Count;

            InTavern = false;
            InMainMenu = false;
        }
        if (scene.buildIndex == 2)
        {
            InTavern = true;
            InMainMenu = false;
        }*/
        if (GameObject.Find("MissionObjectiveText"))
        {
            MissionObjectiveText = GameObject.Find("MissionObjectiveText").GetComponent<TextMeshProUGUI>();
        }
        if (GameObject.FindGameObjectWithTag("Player"))
        {
            Player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        if (GameObject.Find("ToolTipText"))
        {
            ToolTipText = GameObject.Find("ToolTipText").GetComponent<TextMeshProUGUI>();
        }
        if (GameObject.Find("RevealText"))
        {
            RevealText = GameObject.Find("RevealText").GetComponent<TextMeshProUGUI>();
        }
        if (GameObject.Find("MissionObjectiveText"))
        {
            MissionObjectiveText = GameObject.Find("MissionObjectiveText").GetComponent<TextMeshProUGUI>();
        }

        Debug.Log("Gamemanager onsceneloaded finished");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            SceneManager.LoadScene("TestTavern");
        }
        /*
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
        }*/
        switch (CurrentMission)
        {
            case 1:
                storedMissionName = "Mission-1";
                return;
            case 2:
                storedMissionName = "Mission-2";
                return;
            case 3:
                storedMissionName = "Mission-3";
                return;
            case 4:
                storedMissionName = "Mission-4";
                return;

        }
        if(HDDConspiracy == true)
        {
            KC.enabled = true;
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
