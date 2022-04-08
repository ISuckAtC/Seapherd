using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestMultiLoad : MonoBehaviour
{
    bool loaded;
    Scene scene;
    
    // Start is called before the first frame update
    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!loaded) SceneManager.LoadScene("MultiTestInner", LoadSceneMode.Additive);
            else SceneManager.UnloadSceneAsync("MultiTestInner");
        }
        
    }

    void OnSceneLoaded(Scene _scene, LoadSceneMode mode)
    {
        Debug.Log("Scene loaded: " + scene.name);
        loaded = true;
        
    }

    void OnSceneUnloaded(Scene _scene)
    {
        Debug.Log("Scene unloaded: " + scene.name);
        loaded = false;
    }
}
