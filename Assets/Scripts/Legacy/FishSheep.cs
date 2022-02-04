using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSheep : MonoBehaviour
{
    public float GrazingTime;
    public bool DoneGrazing, DoneOnce,Dead, DiedOnce;
    public MissionGraze MissionController;
    public GameManager GM;
    // Start is called before the first frame update
    void Start()
    {
        MissionController = GameObject.FindGameObjectWithTag("GrazingZone").GetComponent<MissionGraze>();
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(DoneGrazing == true&& DoneOnce == false)
        {
            MissionController.FinishedGrazingInt++;
            DoneOnce = true;
        }
        if(Dead== true && DiedOnce == false)
        {
            GM.SheepCount--;
            DiedOnce = true;
        }
    }
}
