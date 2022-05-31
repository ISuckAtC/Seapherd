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

    bool once = false;
    bool helperActive;

    void FixedUpdate()
    {
        dadPosition = dadlantean.transform.position;

        if(helperActive == true) helperTimer += Time.deltaTime;
        if (helperTimer <= whenToActivateHelper)
        {
            helperTimer = 0;
            helperActive = false;
            GameManager.FMODPlayAudioThen(helperVoice, dadPosition, Vector3.up, () => {helperActive = true;}, true, true);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") && !once && gm.Missions["TutorialArtifactHandin"].Status == GameManager.MissionStatus.Handin) 
        {
            once=true;
            GameManager.FMODPlayOnceEvent(mainVoice3, dadPosition, Vector3.up, true, true);
            sheep.SetActive(true);
        }
    }
}
