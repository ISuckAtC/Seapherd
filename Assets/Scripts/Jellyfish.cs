using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jellyfish : MonoBehaviour
{
    Transform jellyfish;
    float speed;
    public float minSpeed;
    public float maxSpeed;
    
    void Start()
    {
        jellyfish = gameObject.transform;
        speed = Random.Range(minSpeed, maxSpeed);
    }

    void FixedUpdate()
    {
        jellyfish.Rotate(new Vector3(0f, speed, 0f), Space.Self);
    }
}
