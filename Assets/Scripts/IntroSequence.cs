using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD;
using FMODUnity;

public class IntroSequence : MonoBehaviour
{
    FMOD.Studio.EventInstance eventInstance;
    public FMODUnity.EventReference dadlanteanEvent;
    FMOD.Studio.EventDescription eventDescription;
    FMOD.Studio.PARAMETER_DESCRIPTION descriptionStorage;
    string currentState;
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
    void Start()
    {



        eventInstance = FMODUnity.RuntimeManager.CreateInstance(dadlanteanEvent);
        ATTRIBUTES_3D attributes;
        attributes.position = this.transform.position.ToFMODVector();
        attributes.velocity = this.GetComponent<Rigidbody>().velocity.ToFMODVector();
        attributes.forward = this.transform.forward.ToFMODVector();
        attributes.up = this.transform.up.ToFMODVector();
        eventInstance.set3DAttributes(attributes);
        eventInstance.start();

        eventDescription = FMODUnity.RuntimeManager.GetEventDescription("event:/VO/Dadlantean/Dadlantean"); eventInstance.release();
    }
    void Update()
    {
        if (Input.anyKeyDown)
        {
            UnityEngine.Debug.Log("Pressed any key");
            i++;
            switch (i)
            {
                case 1: //first line
                    eventInstance.setParameterByNameWithLabel("DadState", "A");
                    break;

                case 2: //ooked at door and second line
                    eventInstance.setParameterByNameWithLabel("DadState", "B");

                    anim.SetBool("LookAtDoor", true);
                    break;

                case 3: //looked at dad and third line
                    eventInstance.setParameterByNameWithLabel("DadState", "C");

                    anim.SetBool("LookAtDoor", false);
                    anim.SetBool("LookAtDad", true);
                    break;

                case 4: //fourth line
                    eventInstance.setParameterByNameWithLabel("DadState", "D");
                    break;

                case 5: //fifth line
                    eventInstance.setParameterByNameWithLabel("DadState", "E");
                    break;

                case 6: //case 6, exit house and sixth line
                    eventInstance.setParameterByNameWithLabel("DadState", "F");

                    anim.SetBool("LookAtDoor", false);
                    anim.SetBool("LookAtDad", false);
                    anim.SetBool("LeaveHouse", true);
                    break;
            }
        }
    }
}
