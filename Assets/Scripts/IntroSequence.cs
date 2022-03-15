using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD;
using FMODUnity;

public class IntroSequence : MonoBehaviour
{
    //public FMODUnity.EventReference lineA;
    public FMOD.Studio.EventInstance eventInstance;
    public AudioSource audioSource;
    public AudioClip[] clips;
    public Animator anim;
    int i;

    void OneShot(EventReference eventName)
    {
        FMOD.Studio.EventInstance soundName = FMODUnity.RuntimeManager.CreateInstance(eventName);
        ATTRIBUTES_3D attributes;
        attributes.position = this.transform.position.ToFMODVector();
        attributes.velocity = this.GetComponent<Rigidbody>().velocity.ToFMODVector();
        attributes.forward = this.transform.forward.ToFMODVector();
        attributes.up = this.transform.up.ToFMODVector();
        soundName.set3DAttributes(attributes);

        soundName.start();
        soundName.release();
    }
    // Start is called before the first frame update
    /*void Start()
    {
        anim.SetTrigger("LookAtDoor");
        anim.SetTrigger("LookAtDad");
        anim.SetTrigger("LeaveHouse");
    }*/
    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Animals/BearGrab");
        //}

        if (Input.anyKeyDown && eventInstance.getParameterByName("DadState", ))
        {
            switch (i)
            {
                case 1:
                    UnityEngine.Debug.Log("case 1, first line");
                    eventInstance.setParameterByNameWithLabel("DadState", "A");
                    //OneShot(lineA);
                    break;

                case 2:
                    UnityEngine.Debug.Log("case 2, looked at door and second line");
                    eventInstance.setParameterByNameWithLabel("DadState", "B");

                    //OneShot(lineB);
                    anim.SetBool("LookAtDoor", true);
                    break;

                case 3:
                    UnityEngine.Debug.Log("case 3, looked at dad and third line");
                    eventInstance.setParameterByNameWithLabel("DadState", "C");

                    //OneShot(lineC);
                    anim.SetBool("LookAtDoor", false);
                    anim.SetBool("LookAtDad", true);
                    break;

                case 4:
                    UnityEngine.Debug.Log("case 4, fourth line");
                    eventInstance.setParameterByNameWithLabel("DadState", "D");

                    //OneShot(lineD);
                    break;

                case 5:
                    UnityEngine.Debug.Log("case 5, fifth line");
                    eventInstance.setParameterByNameWithLabel("DadState", "E");

                    //OneShot(lineE);
                    break;

                case 6:
                    UnityEngine.Debug.Log("case 6, exit house and sixth line");
                    eventInstance.setParameterByNameWithLabel("DadState", "F");

                    //OneShot(lineF);
                    anim.SetBool("LookAtDoor", false);
                    anim.SetBool("LookAtDad", false);
                    anim.SetBool("LeaveHouse", true);
                    break;
            }
        }
    }
}
