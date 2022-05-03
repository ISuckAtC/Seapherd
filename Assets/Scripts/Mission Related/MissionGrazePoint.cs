using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD;
using FMODUnity;

public class MissionGrazePoint : MonoBehaviour
{
    public FMODUnity.EventReference startEvent, endEvent; 
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
                GameManager.Instance.MissionObjectiveText.text = "Let the sheep graze (" + (waypoint.SelfIndex + 1) + "/" + waypoint.ParentNavigator.Waypoints.Length + ")";
                
                var grazeStart = FMODUnity.RuntimeManager.CreateInstance(startEvent);
                ATTRIBUTES_3D attributes;
                attributes.position = this.transform.position.ToFMODVector();
                attributes.velocity = Vector3.zero.ToFMODVector();
                attributes.forward = this.transform.forward.ToFMODVector();
                attributes.up = this.transform.up.ToFMODVector();
                grazeStart.set3DAttributes(attributes);

                grazeStart.start();
                grazeStart.release();
            }
            other.GetComponent<EntitySheep>().GrazingTime += Time.deltaTime;
            if (other.GetComponent<EntitySheep>().GrazingTime > MissionGrazingTime)
            {
                other.GetComponent<EntitySheep>().DoneGrazing = true;

            }
        }
        if (GameManager.Instance.SheepCount == FinishedGrazingInt)
        {
            waypoint.ParentNavigator.Activated(waypoint.SelfIndex);

            var grazeEnd = FMODUnity.RuntimeManager.CreateInstance(endEvent);
                ATTRIBUTES_3D attributes;
                attributes.position = this.transform.position.ToFMODVector();
                attributes.velocity = Vector3.zero.ToFMODVector();
                attributes.forward = this.transform.forward.ToFMODVector();
                attributes.up = this.transform.up.ToFMODVector();
                grazeEnd.set3DAttributes(attributes);

                grazeEnd.start();
                grazeEnd.release();
        }
        if (this.tag == "Exit" && GameManager.Instance.SheepCount == GameManager.Instance.SheepTotal && GameManager.Instance.SheepCount == FinishedGrazingInt)
        {
            ExitOn = true;
            //ExitZone.SetActive(true);
            //Insert what to do if all fish sheep are alive and the mission is done and you need to head back.
        }
    }
}
