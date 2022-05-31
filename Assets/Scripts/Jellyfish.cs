using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jellyfish : MonoBehaviour
{
    Transform jellyfish;
    float speed;
    float bobSpeed;
    public float minBobSpeed;
    public float maxBobSpeed;
    public float minSpinSpeed;
    public float maxSpinSpeed;
    
    void Start()
    {
        jellyfish = gameObject.transform;
        speed = Random.Range(minSpinSpeed, maxSpinSpeed);
        bobSpeed = Random.Range(minBobSpeed, maxBobSpeed);
    }

    void FixedUpdate()
    {
        jellyfish.Rotate(new Vector3(0f, speed, 0f), Space.Self);
        transform.localPosition += new Vector3(0, Mathf.Sin(Time.time) * bobSpeed, 0);
    }
}
