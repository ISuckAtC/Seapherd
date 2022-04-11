using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactDelivery : MonoBehaviour
{
    [Header("Conspiracy true means this is the Conspiracy Theorist")]
    public bool Conspiracy;
    [Header("EasterEgg is used for alternate hidden handins for people, either purely for the joke or a reference")]
    public bool EasterEgg;
    public GameManager GM;
    // Start is called before the first frame update
    void Start()
    {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        
        if(other.tag == "Player")
        {
            transform.LookAt(other.transform);
            if (Input.GetAxisRaw("RShoulder") > 0.5f && GM.artifactGET == true)
            {
                if (Conspiracy == true)
                {
                    GM.ConspiracyHandedIn++;
                    GM.artifactGET = false;
                    if(GM.HDD == true)
                    {
                        GM.HDDConspiracy = true;
                        GM.HDD = false;
                        
                    }
                }
                if (Conspiracy == false)
                {
                    GM.FatherHandedIn++;
                    GM.artifactGET = false;
                    if (GM.HDD == true)
                    {
                        GM.HDDFather = true;
                        GM.HDD = false;
                        
                    }
                }
                if(EasterEgg == true)
                {
                    GM.artifactGET = false;
                   
                    GM.HDDEasterEgg = true;
                }
            }
        }
        
    }
}
