using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactDelivery : MonoBehaviour
{
    [Header("Conspiracy true means this is the Conspiracy Theorist")]
    public bool Conspiracy;
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
        if(Input.GetAxisRaw("RShoulder") > 0.5f && GM.artifactGET == true)
        {
            if(Conspiracy == true)
            {
                GM.ConspiracyHandedIn++;
                GM.artifactGET = false;
            }
            if(Conspiracy == false)
            {
                GM.FatherHandedIn++;
                GM.artifactGET = false;
            }
        }
    }
}
