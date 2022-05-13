using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpBox : MonoBehaviour
{
    public float Force;
    public Vector3 Direction;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay(Collider other)
    {
        if ((other.gameObject.layer & (LayerMask.NameToLayer("Player") | LayerMask.NameToLayer("Sheep"))) > 0)
        {
            other.attachedRigidbody.AddForce(Direction * Force, ForceMode.Force);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.layer & (LayerMask.NameToLayer("Player") | LayerMask.NameToLayer("Sheep"))) > 0)
        {
            //Play sound
        }
    }
}
