using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    public IGrabbable currentlyGrabbed;
    public float GrabRange;
    public void Grab()
    {
        Debug.Log("Grabbing");
        Collider[] overlaps = Physics.OverlapSphere(transform.position, GrabRange, 1 << LayerMask.NameToLayer("Pickup"));
        if (overlaps.Length > 0)
        {
            IGrabbable grabbable = overlaps[0].GetComponent<IGrabbable>();
            if (grabbable != null)
            {
                grabbable.Grab(this);
            }
        }
    }
    public void Release()
    {
        if (currentlyGrabbed != null)
        {
            currentlyGrabbed.Release(this);
        }
    }
    public void Pocket()
    {
        if (currentlyGrabbed != null)
        {
            if (currentlyGrabbed is IPickup)
            {
                (currentlyGrabbed as IPickup).Pocket(gameObject);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, GrabRange);
    }
}
