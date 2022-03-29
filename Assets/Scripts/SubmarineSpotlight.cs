using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmarineSpotlight : MonoBehaviour
{
    public Transform Player;
    public float rotationSpeed;
    private Quaternion LookTowards;
    private Vector3 Direction;
    public bool Spotted,StillInSpotlight;
    [Header("if this isnt changed, then it will instantly forget about you the second you leave the spotlight")]
    public float MaxTime;
    public float timer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {if(StillInSpotlight == false)
        {
            timer += Time.deltaTime;
        }
        if (StillInSpotlight == true)
        {
            timer = 0;
        }
        if (timer > MaxTime)
        {
            Spotted = false;
            timer = 0;
        }
        if(Spotted == true)
        {   //find the vector pointing from our position to the target
            Direction = (Player.position - transform.position).normalized;

            //create the rotation we need to be in to look at the target
            LookTowards = Quaternion.LookRotation(Direction);

            //rotate us over time according to speed until we are in the required rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, LookTowards, Time.deltaTime * rotationSpeed);
            
        }
        
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            Spotted = true;
            StillInSpotlight = true;
        }
        
    }
    private void OnTriggerExit(Collider other)
    {
        StillInSpotlight = false;
    }

}
