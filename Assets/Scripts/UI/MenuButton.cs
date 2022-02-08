using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour
{
    public GameObject[] UIToHide;
    public GameObject[] UIToShow;

    public int SceneToLoadInt = 0;
    public string SceneToLoadStr;

    public void ToggleUI()
    {
        foreach(GameObject ToShow in UIToShow)
        {
            ToShow.SetActive(true);
        }

        foreach(GameObject ToHide in UIToHide)
        {
            ToHide.SetActive(false);
        }
    }

    public void QuitGame()
    {
        #if UNITY_STANDALONE
            Application.Quit();
        #endif
        
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    public void LoadScene()
    {
        if(SceneToLoadInt > -1)
        {
            SceneManager.LoadScene(SceneToLoadInt);
        } 
        else if (SceneToLoadStr.Length > 0)
        {
            SceneManager.LoadScene(SceneToLoadStr);
        }
        else
        {
            Debug.LogError("Please designate a scene to load.");
        }
    }
}
