using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialVoice4 : MonoBehaviour
{
    public FMODUnity.EventReference mainVoice5, voice51;

    public GameObject dadlantean;
    Vector3 dadPosition;
    public float helperTimer;
    public float whenToSpawnBear;
    public GameObject bear;
    bool once;
    bool onceBear;
    bool startBearTimer;
    int sheepCount;


    
    void FixedUpdate()
    {
        dadPosition = dadlantean.transform.position;

        if (startBearTimer) helperTimer += Time.deltaTime;
        if (helperTimer >= whenToSpawnBear && !onceBear)
        {
            onceBear = true;
            GameManager.FMODPlayOnceEvent(voice51, dadPosition, Vector3.up, true, true);
            bear.SetActive(true);
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Sheep"))  
        {
            sheepCount++;

            if (sheepCount == 6 && !once)
            {
                once = true;
                GameManager.FMODPlayOnceEvent(mainVoice5, dadPosition, Vector3.up, true, true);
                startBearTimer = true;
            }
        }
    }
}
