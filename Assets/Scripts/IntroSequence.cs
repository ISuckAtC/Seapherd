using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroSequence : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] clips;
    public Animator anim;
    int i;

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
        if (Input.GetKeyDown(KeyCode.Space) && !audioSource.isPlaying)
        {
            Debug.Log("Audio" + i + "played");
            audioSource.PlayOneShot(clips[i]);
            if (i == 2)
            {
                anim.SetBool("LookAtDoor", true);
                //Open the doors

            }
            if (i == 3)
            {
                anim.SetBool("LookAtDoor", false);

                anim.SetBool("LookAtDad", true);
            }
            if (i == 6)
            {
                anim.SetBool("LookAtDoor", false);
                anim.SetBool("LookAtDad", false);

                //audioSource.PlayOneShot(clips[6]);
                anim.SetBool("LeaveHouse", true);
            }
            i++;

        }

    }
}
