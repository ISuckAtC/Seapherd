using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialVoice5 : MonoBehaviour
{
    public FMODUnity.EventReference mainVoice6;

    public GameObject dadlantean;
    Vector3 dadPosition;

    bool once;
    
    void FixedUpdate()
    {
        dadPosition = dadlantean.transform.position;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Bear") && !once)  
        {
            once = true;
            GameManager.FMODPlayOnce(mainVoice6, dadPosition, Vector3.up);
        }
    }
}
