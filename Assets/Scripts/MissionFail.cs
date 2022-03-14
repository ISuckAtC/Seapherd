using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MissionFail : MonoBehaviour
{
    public GameManager GM;
    public float Timer, MaxTimer;
   
    
    // Start is called before the first frame update
    void Start()
    {
        MaxTimer = 5;
        GM = gameObject.GetComponent<GameManager>();
        
    }

    void Update()
    {
        if(GM.InMainMenu == true)
        {
            goto SkipErrors;
        }
        if (GM.SheepCount <= 0 && GM.InTavern == false)
        {
            Timer += Time.deltaTime;
            //float percentcomplete = Timer / MaxTimer;
           //percentcomplete = Mathf.Clamp01(percentcomplete);

            if(Timer > 5)
            {
                SceneManager.LoadScene("TestTavern");
                Timer = 0;
            }
        }
    SkipErrors:;
    }
}
