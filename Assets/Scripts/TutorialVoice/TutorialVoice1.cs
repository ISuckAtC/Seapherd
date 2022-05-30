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

    public GameObject[] sheep;
    public GameObject artifact;
    public GameObject dogFish;
    public GameObject bearFish;
    bool once;

    void Start()
    {
        GameManager.FMODPlayOnceEvent(mainVoice1, dadPosition, Vector3.up);
        Debug.Log("Plays M1");
        //Setactive all sheep
        //Dogfish
        //Artifact
        artifact.SetActive(false);
        dogFish.SetActive(false);
        bearFish.SetActive(false);
        foreach (GameObject fish in sheep)
        {
            fish.SetActive(false);
        }

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
            artifact.SetActive(true);
            Debug.Log("Plays M2");
        }
    }
}
