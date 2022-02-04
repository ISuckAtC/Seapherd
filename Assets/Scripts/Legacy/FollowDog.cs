using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowDog : MonoBehaviour
{
    public Transform Player;
    public float Speed;
    public float FollowDistance;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.forward = (Player.position - transform.position).normalized;
        float distance = Vector3.Distance(Player.position, transform.position);
        if (distance > FollowDistance) {
            float actualSpeed = Speed;
            if (distance < FollowDistance + 1)
            {
                actualSpeed = Speed * (distance - FollowDistance);
            }
            rb.velocity = (Player.position - transform.position).normalized * actualSpeed;
        }
        else rb.velocity = Vector3.zero;
    }
}
