using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GemstoneFunction : MonoBehaviour
{
    public string RuneType;
    public bool InPrimarySlot = false, InSecondarySlot = false;
    [HideInInspector]
    public Vector3 PrimarySlot, SecondarySlot;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CastSpell()
    {
        if (InPrimarySlot || InSecondarySlot)
        {
            switch (RuneType)
            {
                case "WaterStream":
                    Debug.Log(RuneType + " was cast.");
                    break;

                case "AttractSheep":
                    Debug.Log(RuneType + " was cast.");
                    break;

                case "SummonSpear":
                    Debug.Log(RuneType + " was cast.");
                    break;

                case "LureEnemy":
                    Debug.Log(RuneType + " was cast.");
                    break;

                case "PlayerBoost":
                    Debug.Log(RuneType + " was cast.");
                    break;

                default:
                    Debug.LogWarning(RuneType + " not recognised.");
                    break;
            }
        }
    }

}
