using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DogState
{
    Stay,
    Follow,
    Go
}
public class EntityDog : MonoBehaviour, IToolTip
{
    public Transform Player;
    public float Speed;
    public float FollowDistance;
    Rigidbody rb;
    public DogState State;
    private Vector3 GoLocation;
    public float GoLeeway;
    public string ToolTipText;
    public string ToolTip {get {return ToolTipText;}}
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        switch (State)
        {
            case DogState.Stay:
                break;
            case DogState.Follow:
                transform.forward = (Player.position - transform.position).normalized;
                float distance = Vector3.Distance(Player.position, transform.position);
                if (distance > FollowDistance)
                {
                    float actualSpeed = Speed;
                    if (distance < FollowDistance + 1)
                    {
                        actualSpeed = Speed * (distance - FollowDistance);
                    }
                    rb.velocity = (Player.position - transform.position).normalized * actualSpeed;
                }
                else rb.velocity = Vector3.zero;
                break;
            case DogState.Go:
                if (Vector3.Distance(transform.position, GoLocation) < GoLeeway)
                {
                    State = DogState.Stay;
                }
                else
                {
                    transform.forward = (GoLocation - transform.position).normalized;
                    rb.velocity = transform.forward * Speed;
                }
                break;
        }
    }
}
