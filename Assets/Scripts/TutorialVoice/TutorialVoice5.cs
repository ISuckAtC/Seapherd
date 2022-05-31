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
        Debug.Log("Bearfish barrier entered");
        if (other.gameObject.layer == LayerMask.NameToLayer("Bear") && !once)  
        {
            Debug.Log("bear actually entered the barrier");
            once = true;

            StartCoroutine(GameManager.FMODPlayAudioThen(mainVoice6, dadPosition, Vector3.up, () => {
                try
                {
                    UnityEngine.SceneManagement.SceneManager.LoadScene("MissionSketch_Conch", UnityEngine.SceneManagement.LoadSceneMode.Single);
                } 
                catch(System.Exception e)
                {
                    Debug.Log(e);
                }
            }, true, true));
        }
    }
}
