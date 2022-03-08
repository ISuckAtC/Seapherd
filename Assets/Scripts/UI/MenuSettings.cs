using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MenuSettings : MonoBehaviour
{
    [SerializeField]
    GameManager _GM;

    public AudioMixer mixer;

    //public Dropdown ResolutionDropdown;
    public Dropdown SceneLoadDropdown, ControlTypeDropdown;

    public int IndexOffset = 1;

    private static string NameFromIndex(int BuildIndex)
    {
        string path = SceneUtility.GetScenePathByBuildIndex(BuildIndex);
        int slash = path.LastIndexOf('/');
        string name = path.Substring(slash + 1);
        int dot = name.LastIndexOf('.');
        return name.Substring(0, dot);
    }


    // Start is called before the first frame update
    void Start()
    {
        _GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        

        RefreshSceneDropdown();
        RefreshControlDropdown();
    }

    // Update is called once per frame
    void Update()
    {

    }

    /*  This script was copied from Lonely Echo, commenting out the irrelevant stuff for now
     
    public void SetFullscreen(bool ToSet)
    {
        _GM.SetFullScreen(ToSet);
        Debug.Log("Fullscreen set to " + ToSet);
    }


    public void SetScreenResolution(int NewResolutionIndex)
    {
        _GM.SetResolution(NewResolutionIndex);
    }
    */

    public void MainVolumeControl(float MainVal)
    {
        _GM.VolumeControl(MainVal, "Master");
        Debug.Log("Master set to " + MainVal);
    }

    public void EffectVolumeControl(float SFXVal)
    {
        _GM.VolumeControl(SFXVal, "Effect");
        Debug.Log("SFX set to " + SFXVal);
    }

    public void MusicVolumeControl(float BGMVal)
    {
        _GM.VolumeControl(BGMVal, "Music");
        Debug.Log("BGM set to " + BGMVal);
    }

    public void VoiceVolumeControl(float VoiceVal)
    {
        _GM.VolumeControl(VoiceVal, "Voice");
        Debug.Log("Voice set to " + VoiceVal);
    }

    public void AmbianceVolumeControl(float AmbiVal)
    {
        _GM.VolumeControl(AmbiVal, "Ambiance");
        Debug.Log("Ambiance set to " + AmbiVal);
    }

    /*
    public void MouseSpeed(float Speed)
    {
        _GM.SetSensitivity(Speed);
        Debug.Log("Sensitivity set to " + Speed);
    }
    */

    private void RefreshSceneDropdown()
    {
        if (SceneLoadDropdown != null)
        {
            SceneLoadDropdown.ClearOptions();

            List<string> SceneList = new List<string>();
            for (int i = 0; i < SceneManager.sceneCountInBuildSettings - IndexOffset; i++)
            {
                //SceneToLoad.options.Add(new Dropdown.OptionData() { text = SceneManager.GetSceneByBuildIndex(i + 1).name });
                //(SceneManager.GetSceneByBuildIndex(SceneManager.GetActiveScene().buildIndex + (i+1)).name.ToString())
                string NewScene = " (" + (i + IndexOffset) + ") " + NameFromIndex(i + IndexOffset);
                SceneList.Add(NewScene);
                // Debug.Log(NewScene);
            }

            SceneLoadDropdown.AddOptions(SceneList);

            SceneLoadDropdown.value = 0;
            SceneLoadDropdown.RefreshShownValue();
        }
    }private void RefreshControlDropdown()
    {
        if (ControlTypeDropdown != null)
        {
            ControlTypeDropdown.ClearOptions();

            List<string> ControlList = new List<string>();
            ControlList.Add("Keyboard and Mouse");
            ControlList.Add("VR Joystick");
            ControlList.Add("VR_Leading");
            ControlList.Add("VR_Dragging");
            /*
            for (int i = 0; i < 4; i++)
            {
                string TempString;
                switch (i)
                {
                    case 0:
                        TempString = "Keyboard and Mouse";
                        break;
                    case 1:
                        TempString = "VR Joystick";
                        break;
                    case 2:
                        TempString = "VR_Leading";
                        break;
                    case 3:
                        TempString = "VR_Dragging";
                        break;

                    default:
                        Debug.LogWarning("Int outside bounds of array");
                        break;
                }
                string newControlType = $"({i}) {TempString}";
                ControlList.Add(newControlType);
            }
            */

            ControlTypeDropdown.AddOptions(ControlList);

            ControlTypeDropdown.value = 0;
            ControlTypeDropdown.RefreshShownValue();
        }
    }

    public void DebugLoadScene()
    {
        Debug.Log("Debug load scene");
        SceneManager.LoadScene(SceneLoadDropdown.value + IndexOffset);
    }

    public void SetControlType()
    {
        GameManager._Settings.controlType = (PlayerController.ControlType)ControlTypeDropdown.value;
    }


    public void GoToMain()
    {
        _GM.ToMainMenu();
    }
}
