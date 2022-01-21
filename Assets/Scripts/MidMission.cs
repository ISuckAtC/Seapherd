using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MidMission : MonoBehaviour
{

    public bool optionalObjective;
    public GameObject OptionalObjective;
    public Text ExitText;
    public GameManager GM;
    public bool ExitOn;
    public float Grazing, MissionGrazingTime;
    public int FinishedGrazingInt;
    // Start is called before the first frame update
    void Start()
    {
        OptionalObjective = GameObject.FindGameObjectWithTag("Optional");
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
        }
        if(this.tag == "Exit" && GM.FishSheepTotal == GM.MissionStartFishSheepTotal && GM.FishSheepTotal == FinishedGrazingInt)
        {
            ExitOn = true;
            //Insert what to do if all fish sheep are alive and the mission is done and you need to head back.
        }
    }
}
