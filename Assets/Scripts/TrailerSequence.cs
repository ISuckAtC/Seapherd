using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailerSequence : MonoBehaviour
{
    public Animator anim;
    int i;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            anim.SetBool("Zero", false);
            anim.SetBool("One", true);

            if (i == 1)
            {
                anim.SetBool("One", false);
                anim.SetBool("Two", true);
            }

            if (i == 2)
            {
                anim.SetBool("Two", false);
                anim.SetBool("Three", true);
            }

            if (i == 3)
            {
                anim.SetBool("Three", false);
                anim.SetBool("Four", true);
            }

            i++;
        }
    }
}
