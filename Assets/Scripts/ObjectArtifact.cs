using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectArtifact : MonoBehaviour, IToolTip, IPickup, IGrabbable
{
    public event System.Action PickedUp;
    public string ToolTipText;
    public string PickupNameText;
    public string ToolTip {get {return ToolTipText;}}
    public string PickupName {get {return PickupNameText;}}
    private Rigidbody rb;
    private Transform prevParent;

    public void Start()
    {
        prevParent = transform.parent;
        rb = GetComponent<Rigidbody>();
    }

    public void Grab(PlayerHand hand)
    {
        hand.currentlyGrabbed = this;
        gameObject.layer = LayerMask.NameToLayer("PickupHeld");
        transform.parent = hand.transform;
        transform.position = hand.transform.position;
        rb.useGravity = false;
        rb.isKinematic = false;
    }

    public void Release(PlayerHand hand)
    {
        hand.currentlyGrabbed = null;
        gameObject.layer = LayerMask.NameToLayer("Pickup");
        transform.parent = prevParent;
        rb.useGravity = true;
        rb.isKinematic = false;
    }

    public void Pocket(GameObject player)
    {
        if (PickedUp.GetInvocationList() != null)
        {
            PickedUp.Invoke();
        }
        else
        {
            Debug.LogWarning("Picked up event fired, but no listeners were found.");
        }
        Destroy(gameObject);
    }
}
