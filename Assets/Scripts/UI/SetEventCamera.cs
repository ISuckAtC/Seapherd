using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Canvas))]
public class SetEventCamera : MonoBehaviour
{
    void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        if (this.GetComponent<Canvas>().worldCamera == null)
        {
            this.GetComponent<Canvas>().worldCamera = Camera.main;
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (this.GetComponent<Canvas>().worldCamera == null)
        {
            this.GetComponent<Canvas>().worldCamera = Camera.main;
        }
    }
}
