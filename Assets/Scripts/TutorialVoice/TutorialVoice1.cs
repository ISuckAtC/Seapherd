using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class TutorialVoice1 : MonoBehaviour
{
    public FMODUnity.EventReference mainVoice, helperVoice;

    public GameObject dadlantean;
    Vector3 dadPosition;
    public float helperTimer;
    public float whenToActivateHelper;
    
    public GameObject objectToDestroy;

    void Start()
    {
        if (objectToDestroy != null)
        {
            Destroy(objectToDestroy);
        }
    }
    void FixedUpdate()
    {
        dadPosition = dadlantean.transform.position;

        helperTimer += Time.deltaTime;
        if (helperTimer <= whenToActivateHelper)
        {
            GameManager.FMODPlayOnce(helperVoice, dadPosition, Vector3.up);
            helperTimer = 0f;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Debug.Log("");
            GameManager.FMODPlayOnce(mainVoice, dadPosition, Vector3.up);
        }
    }
}
