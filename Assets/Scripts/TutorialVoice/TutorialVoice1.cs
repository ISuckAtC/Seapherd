using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class TutorialVoice1 : MonoBehaviour
{
    public FMODUnity.EventReference mainVoice1, mainVoice2, helperVoice;

    public GameObject dadlantean;
    Vector3 dadPosition;
    public float helperTimer;
    public float whenToActivateHelper;

    public GameObject artifact;
    public Transform artifactSpawn;
    bool once;

    void Start()
    {
        GameManager.FMODPlayOnceEvent(mainVoice1, dadPosition, Vector3.up);
        Debug.Log("Plays M1");
    }
    /*
    void FixedUpdate()
    {
        dadPosition = dadlantean.transform.position;

        helperTimer += Time.deltaTime;
        if (helperTimer <= whenToActivateHelper)
        {
            GameManager.FMODPlayOnce(helperVoice, dadPosition, Vector3.up);
            //helperTimer = 0f;
        }
    }
    */
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") && !once) 
        {
            once = true;
            GameManager.FMODPlayOnceEvent(mainVoice2, dadPosition, Vector3.up);
            Instantiate(artifact, artifactSpawn.position, Quaternion.identity);
            Debug.Log("Plays M2");
        }
    }
}
