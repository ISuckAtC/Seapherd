using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MissionGiver : MonoBehaviour, IToolTip
{
    public GameObject BoardM1, BoardM2, BoardM1Complete, BoardM2Complete;
    public GameObject Player;
    public GameObject ExclamationMark;
    [Header("Put What MissionGiver This is, we use this to specify what missions can be given by them, 0 is for the Tutorial,  1 is for Mission 1 to 4 will be possible, for MissionGiver 2 another version of Mission 3 or 4 is possible, Mission Giver 3 is tied to jokes, mainly unused")]
    [Tooltip("Use anything from 0, 1, 2 or 3")]
    public int MissionGiverNumber;
    private int MissionNumber;
    public GameManager GM;
    public bool OptionalObjectiveCheck;
    public string ToolTipText;
    public string ToolTipTextAlt;
    public string ToolTip
    {
        get
        {
            if (GameManager.Instance.alt) return ToolTipTextAlt;
            else return ToolTipText;
        }
    }
    bool next;

    bool talking;
    GameObject currentTalk;
    // Start is called before the first frame update
    void Start()
    {
        GM = GameManager.Instance;
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {if(MissionGiverNumber == 0 && GM.Missions["tutorial-p2"].status == GameManager.MissionStatus.Completed)
        {
            this.MissionGiverNumber = 1;
        }
        if (talking)
        {
            if (!currentTalk)
            {
                talking = false;
                if (!GameManager.Instance.alt)
                {
                    SceneManager.LoadScene(3 + GameManager.Instance.CurrentMission);
                }
                if (GameManager.Instance.TotalMissionCompletion == 2 && GameManager.Instance.CurrentMission == 2)
                {
                    GameManager.Instance.TotalMissionCompletion = 0;
                    GameManager.Instance.CurrentMission = 0;
                    SceneManager.LoadScene(0);
                }
                GameManager.Instance.alt = false;
            }
        }
        if (next)
        {
            //GameManager.Instance.alt = false;
            //GameManager.Instance.artifactGET = false;
            SceneManager.LoadScene(MissionNumber+1);
        }
        if (GM.TotalMissionCompletion >= MissionNumber)
        {
            ExclamationMark.SetActive(false);
            // gameObject.GetComponent<BoxCollider>().enabled = false;
        }
        gameObject.transform.LookAt(Player.transform.position);
        if (GM.CurrentMission == MissionNumber && GM.OptionalObjectiveCompleted == true)
        {
            OptionalObjectiveCheck = true;
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        return;
        //ExclamationMark.SetActive(false);
    }
    private void OnTriggerStay(Collider other)
    {
        return;
        /*if (other.tag == "Player")
        {
            if (Input.GetKey(KeyCode.Space))
            {
                StartMission();
            }
            if (Input.GetAxisRaw("RShoulder") > 0.5f)
            {

                StartMission();
            }
        }*/
    }

    public void StartMission()
    {
        Debug.Log("Start Mission call");
        if (talking) 
        {
            currentTalk.transform.GetChild(1).GetComponent<Dialogue>().Skip();
            return;
        }
        if (GameManager.Instance.alt)
        {
            GameManager.Instance.SplashText("Mission Complete!" + (GameManager.Instance.artifactGET ? "\n(+ you got the artifact bonus)" : "") + (GameManager.Instance.SheepCount < GameManager.Instance.SheepTotal ? "\n(- missing sheep)" : ""), 32);
            if (GameManager.Instance.CurrentMission == 1)
            {
                currentTalk = Instantiate(BoardM1Complete, new Vector3(0, 0, 0), Quaternion.identity);
                currentTalk.transform.GetChild(1).GetComponent<Dialogue>().DialogueText = "Mission Complete!";
                currentTalk.transform.GetChild(2).GetComponent<Dialogue>().DialogueText = "You managed to herd " + GameManager.Instance.SheepCount + " out of " + GameManager.Instance.SheepTotal + " sheep.";
            }
            else if (GameManager.Instance.CurrentMission == 2)
            {
                currentTalk = Instantiate(BoardM2Complete, new Vector3(0, 0, 0), Quaternion.identity);
                currentTalk.transform.GetChild(1).GetComponent<Dialogue>().DialogueText = GameManager.Instance.TotalMissionCompletion >= GameManager.Instance.CurrentMission ? "Mission Complete!" : "Mission Failed!";
                currentTalk.transform.GetChild(2).GetComponent<Dialogue>().DialogueText = "You managed to herd " + GameManager.Instance.SheepCount + " out of " + GameManager.Instance.SheepTotal + " sheep.";
                currentTalk.transform.GetChild(3).GetComponent<Dialogue>().DialogueText = GameManager.Instance.TotalMissionCompletion >= GameManager.Instance.CurrentMission ? "The Game will now end." : "Please try again.";
            }
            
            currentTalk.transform.parent = transform;
            talking = true;
        }
        else
        {
            if (GameManager.Instance.TotalMissionCompletion == 0)
            {
                GameManager.Instance.CurrentMission = 1;
                currentTalk = Instantiate(BoardM1, new Vector3(0, 0, 0), Quaternion.identity);
            }
            else
            {
                GameManager.Instance.CurrentMission = 2;
                currentTalk = Instantiate(BoardM2, new Vector3(0, 0, 0), Quaternion.identity);
            }
            currentTalk.transform.parent = transform;
            talking = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        return;
        /*if (MissionNumber > GM.TotalMissionCompletion)
        {
            ExclamationMark.SetActive(true);
        }*/
    }
}
