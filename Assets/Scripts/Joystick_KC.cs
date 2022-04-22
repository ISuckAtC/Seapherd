using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joystick_KC : MonoBehaviour
{
   private KeyCode[] konamiCode;
    int currentKeyIndex = 0;
    public int step;
    public bool KC;
    void Start()
    {
        
    }
    private void Update()
    {
        switch (step)
        {
            case 0:
                if (Input.GetAxisRaw("Vertical") > 0.1) step++;
                break;
            case 1:
                if (Input.GetAxisRaw("Vertical") > 0.1) step++;
                break;
            case 2:
                if (Input.GetAxisRaw("Vertical") < -0.1) step++;
                break;
            case 3:
                if (Input.GetAxisRaw("Vertical") < -0.1) step++;
                break;
            case 4:
                if (Input.GetAxisRaw("Horizontal") < -0.1) step++;
                break;
            case 5:
                if (Input.GetAxisRaw("Horizontal") > 0.1) step++;
                break;
            case 6:
                if (Input.GetAxisRaw("Horizontal") < -0.1) step++;
                break;
            case 7:
                if (Input.GetAxisRaw("Horizontal") > 0.1) step++;
                break;
            case 8:
                if (Input.GetButtonDown("B")) step++;
                break;
            case 9:
                if (Input.GetButtonDown("A")) step++;
                break;
            case 10:
                KC = true;
                break;
        }
    }
    
}
