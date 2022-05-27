using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialVoice2 : MonoBehaviour
{
    public FMODUnity.EventReference mainVoice3, helperVoice;
    public GameManager gm;

    public GameObject dadlantean;
    Vector3 dadPosition;
    public float helperTimer;
    public float whenToActivateHelper;

    public GameObject sheep;
    public Transform sheepSpawn;

    bool once = false;

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
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") && !once && gm.FoundArtifacts == 1) 
        {
            once=true;
            GameManager.FMODPlayOnceEvent(mainVoice3, dadPosition, Vector3.up);
            GameObject sheep0 = Instantiate(sheep, sheepSpawn.position, Quaternion.identity);
        }
    }
}
