using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameManager GM;

    public KeyCode PauseButton = KeyCode.Escape; //ControllerPause = KeyCode.

    public GameObject PauseScreen, Player;

    public float DistanceOffset = 10f;

    public bool IsPaused = false;
    // Start is called before the first frame update
    void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        PauseScreen.SetActive(IsPaused);

    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (Player == null)
        {
            if (GameObject.FindGameObjectWithTag("Player") != null)
            {
                Player = GameObject.FindGameObjectWithTag("Player");
            }
        }

        if (GM == null)
        {
            if (GameObject.Find("GameManager") != null)
            {
                Player = GameObject.Find("GameManager");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
            if (GM != null && GameManager._Settings.controlType == PlayerController.ControlType.KBM && IsPaused)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            if (GM != null && !IsPaused) // && GameManager._Settings.controlType == PlayerController.ControlType.KBM)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
    }

    public void TogglePause()
    {
        IsPaused = !IsPaused;


        if (IsPaused)
        {
            transform.position = Player.transform.position + Player.transform.forward * DistanceOffset;
            transform.LookAt(Player.transform);
            Time.timeScale = 0f;
        }else
        {
            Time.timeScale = 1f;
        }

        PauseScreen.SetActive(IsPaused);

    }

    public void MakePausedFalse()
    {
        IsPaused = false;
    }
}
