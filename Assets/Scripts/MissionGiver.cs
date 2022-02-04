using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MissionGiver : MonoBehaviour
{
    public GameObject Player;
    public GameObject ExclamationMark;
    public int MissionNumber;
    public GameManager GM;
    public bool OptionalObjectiveCheck;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GM.TotalMissionCompletion >= MissionNumber)
        {
            ExclamationMark.SetActive(false);
            // gameObject.GetComponent<BoxCollider>().enabled = false;
        }
        gameObject.transform.LookAt(Player.transform.position);
        if(GM.CurrentMission == MissionNumber && GM.OptionalObjectiveCompleted == true)
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
       
        if(other.tag == "Player")
        {
            if (Input.GetKey(KeyCode.Space))
            {
                GM.CurrentMission = MissionNumber;
                SceneManager.LoadScene(MissionNumber+1);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(MissionNumber > GM.TotalMissionCompletion)
        {
            ExclamationMark.SetActive(true);
        }
    }
}
