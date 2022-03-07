using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MissionFail : MonoBehaviour
{
    public GameManager GM;
    public float Timer, MaxTimer;
    public Scene scene;
    public bool InTavern;
    // Start is called before the first frame update
    void Start()
    {
        MaxTimer = 5;
        GM = gameObject.GetComponent<GameManager>();
    }

    // Update is called once per frame
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.buildIndex == 1)
        {
            InTavern = true;
        }
        if(scene.buildIndex > 1)
        {
            InTavern = false;
        }
    }
    void Update()
    {
        if (GM.SheepCount <= 0 && InTavern == false)
        {
            Timer += Time.deltaTime;
            //float percentcomplete = Timer / MaxTimer;
           //percentcomplete = Mathf.Clamp01(percentcomplete);

            if(Timer > 5)
            {
                SceneManager.LoadScene(1);
            }
           
        }
    }
}
