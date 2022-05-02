using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    public IGrabbable currentlyGrabbed;
    public float GrabRange;
    public void Grab()
    {
        RaycastHit hit;
        if (Physics.SphereCast(transform.position, GrabRange, transform.forward, out hit, 0f, 1 << LayerMask.NameToLayer("Pickup")))
        {
            IGrabbable grabbable = hit.collider.GetComponent<IGrabbable>();
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
