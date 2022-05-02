using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGrabbable
{
    void Grab(PlayerHand hand);
    void Release(PlayerHand hand);
}
