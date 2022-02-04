using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class EndMission : MonoBehaviour
{
    public GameManager GM;
    public TextMeshProUGUI EndText, OptionalEndText;
    public string ExternalMissionEndText, ExternalOptionalEndText;
    // Start is called before the first frame update
    void Start()
    {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
       /* switch (GM.CurrentMission)
        {
            case 1:
                //Code Goes here, can pull specific strings from certain objects in this case for the text.
            break;
        }*/

    }

    // Update is called once per frame
    void Update()
    {
        EndText.text = ExternalMissionEndText;
        if(GM.OptionalObjectiveCompleted == true)
        {//Filler Text right now, replace with better lines, or replace with External text strings. should be accessible from other scripts and gameobjects to fill in the blanks.
            OptionalEndText.text = " I am proud of you for once";
        }
    }
}
