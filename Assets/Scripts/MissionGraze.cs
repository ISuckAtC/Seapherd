using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionGraze : MonoBehaviour
{
    public bool optionalObjective;
    public GameObject OptionalObjective;//, ExitZone;
    public bool ExitOn;
    public float Grazing, MissionGrazingTime;
    public int FinishedGrazingInt;
    MissionWaypoint waypoint;
    public Material GoalMarker;
    bool entered;
    // Start is called before the first frame update
    void Start()
    {
        waypoint = GetComponent<MissionWaypoint>();
        waypoint.Override = true;
        //ExitZone = GameObject.FindGameObjectWithTag("Exit");
        //ExitZone.SetActive(false);
        //OptionalObjective = GameObject.FindGameObjectWithTag("Optional");
    }

    // Update is called once per frame
    void Update()
    {
        if (optionalObjective == true)
        {
            GameManager.Instance.OptionalObjectiveCompleted = true;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Sheep"))
        {
            if (!entered)
            {
                GetComponent<MeshRenderer>().material = GoalMarker;
                entered = true;
                GameManager.Instance.MissionObjectiveText.text = "Let the sheep graze (" + (waypoint.SelfIndex + 1) + "/10)";
            }
            other.GetComponent<Fish>().GrazingTime += Time.deltaTime;
            if (other.GetComponent<Fish>().GrazingTime > MissionGrazingTime)
            {
                other.GetComponent<Fish>().DoneGrazing = true;

            }
        }
        if (GameManager.Instance.FishSheepTotal == FinishedGrazingInt)
        {
            waypoint.ParentNavigator.Acvivated();
        }
        if (this.tag == "Exit" && GameManager.Instance.FishSheepTotal == GameManager.Instance.MissionStartFishSheepTotal && GameManager.Instance.FishSheepTotal == FinishedGrazingInt)
        {
            ExitOn = true;
            //ExitZone.SetActive(true);
            //Insert what to do if all fish sheep are alive and the mission is done and you need to head back.
        }
    }
}
