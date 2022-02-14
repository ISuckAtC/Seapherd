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
    public Dropdown SceneLoadDropdown;

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
        _GM = GameObject.Find("__app").GetComponentInChildren<GameManager>();
        
        /*
        ResolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int CurrentResolutionIndex = 0;
        for (int i=0; i < _GM.ScreenResolutions.Length; i++)
        {
            string NewRes = _GM.ScreenResolutions[i].width + "x" + _GM.ScreenResolutions[i].height;
            options.Add(NewRes);
            if (_GM.ScreenResolutions[i].width == Screen.currentResolution.width && _GM.ScreenResolutions[i].height == Screen.currentResolution.height)
            {
                CurrentResolutionIndex = i;
            }
        }

        ResolutionDropdown.AddOptions(options);
        ResolutionDropdown.value = CurrentResolutionIndex;
        ResolutionDropdown.RefreshShownValue();
        SetScreenResolution(CurrentResolutionIndex);
        */

        RefreshSceneDropdown();
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
    }

    public void DebugLoadScene()
    {
        SceneManager.LoadScene(SceneLoadDropdown.value + IndexOffset);
    }


    public void GoToMain()
    {
        _GM.ToMainMenu();
    }
}
