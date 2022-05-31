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

    public class MissionInfo
    {
        public MissionStatus Status;
        public object Extras;
        public MissionInfo(MissionStatus status, object extras)
        {
            Status = status;
            Extras = extras;
        }
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
    public Dictionary<string, MissionInfo> Missions = new Dictionary<string, MissionInfo>();
    [SerializeField]
    private List<GameObject> missionPrefabLoad = new List<GameObject>();

    public Dictionary<string, GameObject> MissionPrefabs = new Dictionary<string, GameObject>();

    private List<FMOD.Studio.EventInstance> currentPlaying = new List<FMOD.Studio.EventInstance>();

    void Awake()
    {
        /*
            HELLO. READ ME.

            Regarding the extras for missions, its for stuff like total sheep to herd and such.
            To load multiple variables just use a tuple, make sure to remember what you load in
            so that you load the correct stuff when you access the tuple later on.


        */


        Missions.Add("Tutorial-p1", new MissionInfo(MissionStatus.NotStarted, null));
        Missions.Add("Tutorial-p2", new MissionInfo(MissionStatus.Unavailable, null));
        Missions.Add("Mission-1", new MissionInfo(MissionStatus.Unavailable, null));
        Missions.Add("Mission-2", new MissionInfo(MissionStatus.Unavailable, null));
        Missions.Add("Mission-3", new MissionInfo(MissionStatus.Unavailable, null));
        Missions.Add("Mission-4", new MissionInfo(MissionStatus.Unavailable, null));
        Missions.Add("TutorialArtifactHandin", new MissionInfo(MissionStatus.InProgress, null));



        Missions["Tutorial-p1"].Status = MissionStatus.InProgress;

        Debug.Log("IJDHBFIUKHJSDF | " + Missions["Tutorial-p1"].Status.ToString());


        for (int i = 0; i < Missions.Count; ++i)
        {
            if (i < missionPrefabLoad.Count && missionPrefabLoad[i] != null)
            {
                MissionPrefabs.Add(Missions.ElementAt(i).Key, missionPrefabLoad[i]);
            }
        }


        _AM = Resources.Load<AudioMixer>("MasterVolume");
        KC = gameObject.GetComponent<Joystick_KC>();
        if (!(Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.Space))) _Settings.controlType = PlayerController.ControlType.VR_Dragging;
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

        if (scene.buildIndex == 0)
        {
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
        //Debug.Log("Mission status for tut 1 is " + Missions["Tutorial-p1"].status);
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
        if (HDDConspiracy == true)
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
        }
        else
        {
            Debug.LogError(unlockedSkill + " is not recognised.");
        }
    }

    public static IEnumerator FMODPlayAudioThen(FMODUnity.EventReference eventRef, Vector3 position, Vector3 velocity, System.Action action, bool registerToStop = false, bool stopCurrent = false)
    {
        FMOD.Studio.EventInstance instance = FMODUnity.RuntimeManager.CreateInstance(eventRef);

        FMOD.Studio.EventDescription desc;
        FMOD.RESULT result = instance.getDescription(out desc);

        Debug.Log(result);

        int duration;
        result = desc.getLength(out duration);

        FMODPlayOnceInstance(ref instance, position, velocity, registerToStop, stopCurrent);

        //Debug.Log("Playing " + eventRef.Path + " (duration: " + duration + " | result " + result + ")");

        yield return new WaitForSeconds(((float)duration) / 1000f);

        //Debug.Log("Finished playing " + eventRef.Path);

        action();
    }

    public static void FMODPlayOnceEvent(FMODUnity.EventReference eventReference, Vector3 postition, Vector3 velocity, bool registerToStop = false, bool stopCurrent = false)
    {
        var eventPlay = FMODUnity.RuntimeManager.CreateInstance(eventReference);
        FMODPlayOnceInstance(ref eventPlay, postition, velocity, registerToStop, stopCurrent);
    }

    public static void FMODPlayOnceInstance(ref FMOD.Studio.EventInstance instance, Vector3 postition, Vector3 velocity, bool registerToStop = false, bool stopCurrent = false)
    {
        FMOD.ATTRIBUTES_3D attributes;

        attributes.position = FMODUnity.RuntimeUtils.ToFMODVector(postition);
        attributes.velocity = FMODUnity.RuntimeUtils.ToFMODVector(velocity);
        attributes.forward = FMODUnity.RuntimeUtils.ToFMODVector(Vector3.forward);
        attributes.up = FMODUnity.RuntimeUtils.ToFMODVector(Vector3.up);
        instance.set3DAttributes(attributes);

        if (stopCurrent)
        {
            Debug.Log("Stopping current sounds, sounds to stop: " + GameManager.Instance.currentPlaying.Count);
            foreach (var ins in GameManager.Instance.currentPlaying) 
            {
                UnityEngine.Debug.Log("Stopping sound");
                ins.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            }
            GameManager.Instance.currentPlaying.Clear();
        }

        instance.start();
        instance.release();

        if (registerToStop)
        {

            FMOD.Studio.EventDescription desc;
            FMOD.RESULT result = instance.getDescription(out desc);

            int duration;
            result = desc.getLength(out duration);

            GameManager.Instance.currentPlaying.Add(instance);

            IEnumerator updateList(FMOD.Studio.EventInstance ins)
            {
                yield return new WaitForSeconds(((float)duration) / 1000f);
                Debug.Log("Removing sound (duration ended)");
                if (GameManager.Instance.currentPlaying.Contains(ins)) GameManager.Instance.currentPlaying.Remove(ins);
            }

            GameManager.Instance.StartCoroutine(updateList(instance));
        }
    }
}
