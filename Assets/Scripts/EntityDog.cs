using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum DogState
{
    Stay,
    Follow,
    Go,
    Chase,
    Herd,
    Outside
}
public class EntityDog : MonoBehaviour, IToolTip
{
    public Transform Player, Bear, OutsideTavern;
    public float Speed;
    public float FollowDistance;
    Rigidbody rb;
    public DogState State;
    public Vector3 HerdDirection;
    public float StrayDistanceThreshold;
    public float HerdDistance;
    private Vector3 GoLocation;
    public float GoLeeway;
    public string ToolTipText;
    public string ToolTip { get { return ToolTipText; } }
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        OutsideTavern = GameObject.FindGameObjectWithTag("StayOut").transform;
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
            case DogState.Chase:
                Bear = GameObject.Find("Bearfish").transform;
                transform.forward = (Bear.position - transform.position).normalized;
                float ChaseDistance = Vector3.Distance(Bear.position, transform.position);
                if (ChaseDistance < 1)
                {
                    Bear.GetComponent<EntityBear>().Scare(Player.GetComponent<PlayerController>().ThreatenAmount);
                }
                if (Bear.GetComponent<EntityBear>().escaping == true)
                {
                    State = DogState.Follow;
                }
                break;

            case DogState.Herd:
                // Get the average position of all the sheep
                Vector3 averagePosition = Vector3.zero;
                foreach (EntitySheep sheep in GameManager.Instance.FishSheep)
                {
                    averagePosition += sheep.transform.position;
                }
                averagePosition /= GameManager.Instance.FishSheep.Count;
                // Get the sheep thats furthest away from the average position
                EntitySheep furthestSheep = GameManager.Instance.FishSheep.OrderBy(x => Vector3.Distance(x.transform.position, averagePosition)).First();

                // Check if the sheep is too far away from the average position
                if (Vector3.Distance(furthestSheep.transform.position, averagePosition) > StrayDistanceThreshold)
                {
                    Vector3 targetPosition = furthestSheep.transform.position + (furthestSheep.transform.position - averagePosition).normalized * HerdDistance;
                    transform.forward = (targetPosition - transform.position).normalized;
                    rb.velocity = transform.forward * Speed;
                }
                else
                {
                    Vector3 targetPosition = averagePosition - HerdDirection.normalized * HerdDistance;
                    transform.forward = (targetPosition - transform.position).normalized;
                    rb.velocity = transform.forward * Speed;
                }

                break;
            case DogState.Outside:
                
                break;
        }
    }
}
