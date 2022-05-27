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
    public Transform dogSpawn;

    bool once;

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
        if (other.gameObject.layer == LayerMask.NameToLayer("Sheep") && !once)  
        {
            once = true;
            GameManager.FMODPlayOnceEvent(mainVoice4, dadPosition, Vector3.up);
            Instantiate(dog, dogSpawn.position, Quaternion.identity);

            for (int i = 0; i > sheep.Length; i++)
            {
                sheep[i].SetActive(true);
            }
        }
    }
}
