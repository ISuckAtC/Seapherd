using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Fish : MonoBehaviour
{
    public float DetectionRange;
    public float DirectionInfluence;
    public float Speed;
    public float Runtime;
    public float CurrentSpeed;
    private float runtimer;
    private Rigidbody rb;
    public float GrazingTime;
    public bool DoneGrazing;
    bool DoneOnce, Dead, DiedOnce;
    MidMission MissionController;
    public float ForceGroupDuration;
    Vector3 ForceTarget;
    float forceGroupTimer;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        MissionController = GameObject.FindGameObjectWithTag("GrazingZone").GetComponent<MidMission>();
    }

    void Update()
    {
        if (DoneGrazing && !DoneOnce)
        {
            MissionController.FinishedGrazingInt++;
            DoneOnce = true;
        }
        if (Dead && !DiedOnce)
        {
            GameManager.Instance.FishSheepTotal--;
            DiedOnce = true;
        }
    }

    void FixedUpdate()
    {
        if (forceGroupTimer > 0)
        {
            forceGroupTimer -= Time.deltaTime;
            runtimer = forceGroupTimer;
            transform.forward = (ForceTarget - transform.position).normalized;
        }
        else
        {
            Collider[] col = Physics.OverlapSphere(transform.position, DetectionRange, (1 << LayerMask.NameToLayer("Dog") | 1 << LayerMask.NameToLayer("Sheep") | 1 << LayerMask.NameToLayer("Bear") | 1 << LayerMask.NameToLayer("Player")));

            if (col.Length > 1)
            {
                if (col.Any(x => x.gameObject.layer == LayerMask.NameToLayer("Dog") || x.gameObject.layer == LayerMask.NameToLayer("Bear") || x.gameObject.layer == LayerMask.NameToLayer("Player")))
                {
                    transform.forward = (transform.position - col.First(x => x.gameObject.layer == LayerMask.NameToLayer("Dog") || x.gameObject.layer == LayerMask.NameToLayer("Bear") || x.gameObject.layer == LayerMask.NameToLayer("Player")).transform.position).normalized;
                    runtimer = Runtime;
                }
                Collider[] fishes = col.Where(x => x.gameObject.layer == LayerMask.NameToLayer("Sheep")).ToArray();
                if (fishes.Length > 0)
                {
                    Vector3 averageDirection = Vector3.zero;
                    foreach (Collider fish in fishes)
                    {
                        averageDirection += fish.transform.forward;
                    }
                    averageDirection /= fishes.Length;

                    transform.forward = Vector3.Lerp(transform.forward, averageDirection, DirectionInfluence);
                }
            }
        }


        if (runtimer > 0)
        {
            //Debug.Log("Setting speed");
            rb.velocity = transform.forward * Speed;
            runtimer -= Time.deltaTime;
        }

        CurrentSpeed = rb.velocity.magnitude;
    }

    public void ForceGroup(Vector3 target)
    {
        forceGroupTimer = ForceGroupDuration;
        ForceTarget = target;
    }
}
