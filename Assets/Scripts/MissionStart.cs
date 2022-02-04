using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionStart : MonoBehaviour
{
    public int CurrentMission;
    public GameManager GM;
    public string MissionText, OptionalMissionItemName, ExternalMissionText;
    public Text MissionTextBox, OptionalTextBox;
    public bool OptionalMissionItem;
    // Start is called before the first frame update
    void Start()
    {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();

        GM.CurrentMission = CurrentMission;
    }

    // Update is called once per frame
    void Update()
    {
        //Cleanup and replace placeholder mission text with External Text once it gets made.
        MissionTextBox.text = MissionText;
        OptionalTextBox.text = "Ohh by the way could you get my " + OptionalMissionItemName + " for me?";
    }
}
