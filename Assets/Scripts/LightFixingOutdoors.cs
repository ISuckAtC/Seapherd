using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFixingOutdoors : MonoBehaviour
{
    public GameObject[] Lights;
    private int LightCurrent;
    private bool DoOnce;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.InTavern == true && DoOnce == false)
        {
            Lights[LightCurrent].SetActive(true);
            LightCurrent++;
            if(LightCurrent > Lights.Length)
            {
                DoOnce = true;
                LightCurrent = 0;
            }
        }
        if(GameManager.Instance.InTavern == false && DoOnce == false)
        {
            Lights[LightCurrent].SetActive(false);
            LightCurrent++;
            if (LightCurrent > Lights.Length)
            {
                DoOnce = true;
                LightCurrent = 0;
            }
        }
    }
}
