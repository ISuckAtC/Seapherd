using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StartMission : MonoBehaviour
{

    public string MissionText, OptionalMissionItemName;
    public Text MissionTextBox, OptionalTextBox;
    public bool OptionalMissionItem;
    // Start is called before the first frame update
    void Start()
    {
        
        MissionTextBox.text = MissionText;
        OptionalTextBox.text = "Ohh by the way could you get my " + OptionalMissionItemName + " for me?";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
