using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MissionGiver : MonoBehaviour, IToolTip
{
    public GameObject Player;
    public GameObject ExclamationMark;
    public int MissionNumber;
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
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (next)
        {
            //GameManager.Instance.alt = false;
            //GameManager.Instance.artifactGET = false;
            //SceneManager.LoadScene(0);
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
        ExclamationMark.SetActive(false);
    }
    private void OnTriggerStay(Collider other)
    {

        if (other.tag == "Player")
        {
            if (Input.GetKey(KeyCode.Space))
            {
                //StartMission();
            }
        }
    }

    public void StartMission()
    {
        if (GameManager.Instance.alt)
        {
            GameManager.Instance.SplashText("Mission Complete!" + (GameManager.Instance.artifactGET ? "\n(+ you got the artifact bonus)" : "") + (GameManager.Instance.SheepCount < GameManager.Instance.SheepTotal ? "\n(- missing sheep)" : ""), 32);
            new System.Threading.Thread(() => {
                System.Threading.Thread.Sleep(5000);
                next = true;
            }).Start();
        }
        else
        {
            GM.CurrentMission = MissionNumber;
            SceneManager.LoadScene(MissionNumber + 1);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (MissionNumber > GM.TotalMissionCompletion)
        {
            ExclamationMark.SetActive(true);
        }
    }
}