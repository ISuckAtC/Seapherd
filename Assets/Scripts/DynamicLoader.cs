using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DynamicLoader : MonoBehaviour
{
    public Transform LoadPoint;
    public string SceneToLoad;
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            SceneManager.LoadSceneAsync(SceneToLoad, LoadSceneMode.Additive);
            SceneManager.GetSceneByName(SceneToLoad).GetRootGameObjects()[0].transform.position = LoadPoint.position;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            SceneManager.UnloadSceneAsync(SceneToLoad);
        }
    }
}
