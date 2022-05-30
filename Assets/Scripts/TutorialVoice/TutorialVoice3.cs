using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialVoice3 : MonoBehaviour
{
    public FMODUnity.EventReference mainVoice4, helperVoice;

    public GameObject dadlantean;
    Vector3 dadPosition;
    public float helperTimer;
    public float whenToActivateHelper;

    public GameObject dog;
    public GameObject[] sheep;

    bool once;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Sheep") && !once)  
        {
            once = true;
            GameManager.FMODPlayOnceEvent(mainVoice4, dadPosition, Vector3.up);
            dog.SetActive(true);
        }
    }

    void Update()
    {
        if (Input.GetButton("B") && once == true)
        {
            foreach (GameObject fish in sheep)
            {
                fish.SetActive(true);
            }
        }

    }
}
