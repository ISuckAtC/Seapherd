using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DynamicLoader : MonoBehaviour
{
    public Transform LoadPoint;
    public string SceneToLoad;

    public GameObject lightsToLoad;

    void Awake()
    {
        lightsToLoad.SetActive(false);

    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            SceneManager.LoadSceneAsync(SceneToLoad, LoadSceneMode.Additive).completed += OnSceneLoadComplete;
            lightsToLoad.SetActive (true);
        }
    }
    public void OnSceneLoadComplete(AsyncOperation a)
    {
        GameObject sceneParent = SceneManager.GetSceneByName(SceneToLoad).GetRootGameObjects()[0];
        sceneParent.transform.position = LoadPoint.position;
        sceneParent.transform.rotation = LoadPoint.rotation;
        sceneParent.transform.localScale = LoadPoint.localScale;
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            SceneManager.UnloadSceneAsync(SceneToLoad);
            lightsToLoad.SetActive(false);
        }
    }
}
