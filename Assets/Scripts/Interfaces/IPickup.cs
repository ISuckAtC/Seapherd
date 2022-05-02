using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPickup
{
    event System.Action PickedUp;
    string PickupName { get; }
    void Pocket(GameObject player);
}
