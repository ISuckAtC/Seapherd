using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectArtifact : MonoBehaviour, IToolTip, IPickup
{
    public event System.Action PickedUp;
    public string ToolTipText;
    public string PickupNameText;
    public string ToolTip {get {return ToolTipText;}}
    public string PickupName {get {return PickupNameText;}}
    public void Pickup()
    {
        PickedUp.Invoke();
    }
}
