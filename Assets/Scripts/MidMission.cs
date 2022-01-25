using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class MidMission : MonoBehaviour
{

    public bool optionalObjective;
    public GameObject OptionalObjective, ExitZone;
    public TextMeshProUGUI ExitText;
    public GameManager GM;
    public bool ExitOn;
    public float Grazing, MissionGrazingTime;
    public int FinishedGrazingInt;
    // Start is called before the first frame update
    void Start()
    {

        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        ExitText = GameObject.Find("MissionObjectiveText").GetComponent<TextMeshProUGUI>();
        ExitZone = GameObject.FindGameObjectWithTag("Exit");
        ExitZone.SetActive(false);
        //OptionalObjective = GameObject.FindGameObjectWithTag("Optional");
    }

    // Update is called once per frame
    void Update()
    {
        if(optionalObjective == true)
        {
            GM.OptionalObjectiveCompleted = true;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.tag== "Sheep")
        {
            other.GetComponent<FishSheep>().GrazingTime += Time.deltaTime;
        }
        if(other.GetComponent<FishSheep>().GrazingTime > MissionGrazingTime)
        {
            other.GetComponent<FishSheep>().DoneGrazing = true;
            
        }
        if(GM.FishSheepTotal == FinishedGrazingInt)
        {
            ExitOn = true;
            ExitText.text = "time to head back!";
            ExitZone.SetActive(true);
        }
        if(this.tag == "Exit" && GM.FishSheepTotal == GM.MissionStartFishSheepTotal && GM.FishSheepTotal == FinishedGrazingInt)
        {
            ExitOn = true;
            ExitZone.SetActive(true);
            //Insert what to do if all fish sheep are alive and the mission is done and you need to head back.
        }
    }
}
