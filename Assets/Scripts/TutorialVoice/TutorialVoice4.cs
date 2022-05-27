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
    public Transform bearSpawn;

    bool once;
    bool onceBear;
    int sheepCount;

    
    void FixedUpdate()
    {
        dadPosition = dadlantean.transform.position;

        helperTimer += Time.deltaTime;
        if (helperTimer >= whenToSpawnBear && !onceBear)
        {
            onceBear = true;
            GameManager.FMODPlayOnce(voice51, dadPosition, Vector3.up);

            GameObject bearSpawned = Instantiate(bear, bearSpawn.position, Quaternion.identity);
            //helperTimer = 0f;
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
                GameManager.FMODPlayOnce(mainVoice5, dadPosition, Vector3.up);
            }
        }
    }
}
