using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EntitySheep : MonoBehaviour
{
    public float DetectionRange;
    public float DirectionInfluence;
    public float ScareInfluence;
    public float CenterBias;
    public float GroundHoverHeight;
    public float SocialDistancing;
    public float Speed;
    public float Runtime;
    public float CurrentSpeed;
    private float runtimer;
    private Rigidbody rb;
    public float GrazingTime;
    public bool DoneGrazing;
    public bool GotoPlayer, RunFromPlayer, GotoMarker, GoUp;
    public GameObject Player, Marker;
    public GameManager GM;
    bool DoneOnce;
    MissionGraze MissionController;
    public float ForceGroupDuration;
    Vector3 ForceTarget;
    float forceGroupTimer;
    bool captured;
    Vector3 scarePoint;
    bool scared;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Player = GameObject.FindGameObjectWithTag("Player");
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        //Marker = GameObject.FindGameObjectWithTag("PlayerMarker");        COMMENTED OUT FOR NOW
        MissionController = GameObject.FindGameObjectWithTag("GrazingZone").GetComponent<MissionGraze>();
    }

    void Update()
    {
        if (DoneGrazing && !DoneOnce)
        {
            MissionController.FinishedGrazingInt++;
            DoneOnce = true;
        }
        if(GM.GotoMarker == true)
        {
            GotoMarker = true;
        }
        if (GM.GotoPlayer == true)
        {
            GotoPlayer = true;
        }
        if (GM.GoUp == true)
        {
            GM.GoUp = true;
        }
        if (GM.RunFromPlayer == true)
        {
            RunFromPlayer = true;
        }
    }

    void FixedUpdate()
    {
        if (captured) return;
        if (GotoMarker == true && forceGroupTimer > 0)
        {
            scared = false;
            forceGroupTimer -= Time.deltaTime;
            runtimer = forceGroupTimer;
            transform.LookAt(Marker.transform);
            transform.position += transform.forward / 10;
            goto EscapeOrders;
        }
        if (GotoPlayer == true && forceGroupTimer > 0)
        {
            scared = false;
            forceGroupTimer -= Time.deltaTime;
            runtimer = forceGroupTimer;
            transform.LookAt(Player.transform);
            transform.position += transform.forward / 10;
            goto EscapeOrders;
        }
        if (RunFromPlayer == true && forceGroupTimer > 0)
        {
            scared = false;
            forceGroupTimer -= Time.deltaTime;
            runtimer = forceGroupTimer;
            transform.forward = (ForceTarget - Player.transform.position).normalized;
            goto EscapeOrders;
        }
        if (GoUp == true && forceGroupTimer > 0)
        {
            scared = false;
            forceGroupTimer -= Time.deltaTime;
            runtimer = forceGroupTimer;
           
            transform.position += transform.up / 10;
            goto EscapeOrders;
        }
        if (forceGroupTimer > 0)
        {
            scared = false;
            forceGroupTimer -= Time.deltaTime;
            runtimer = forceGroupTimer;
            transform.forward = (ForceTarget - transform.position).normalized;
        }
        else
        {
            if (scared) transform.forward = Vector3.Lerp(transform.forward, (transform.position - scarePoint).normalized, ScareInfluence);
            Collider[] col = Physics.OverlapSphere(transform.position, DetectionRange, (1 << LayerMask.NameToLayer("Dog") | 1 << LayerMask.NameToLayer("Sheep") | 1 << LayerMask.NameToLayer("Bear") | 1 << LayerMask.NameToLayer("Player")));

            if (col.Length > 1)
            {
                if (col.Any(x => x.gameObject.layer == LayerMask.NameToLayer("Dog") || x.gameObject.layer == LayerMask.NameToLayer("Bear") || x.gameObject.layer == LayerMask.NameToLayer("Player")))
                {
                    scarePoint = col.First(x => x.gameObject.layer == LayerMask.NameToLayer("Dog") || x.gameObject.layer == LayerMask.NameToLayer("Bear") || x.gameObject.layer == LayerMask.NameToLayer("Player")).transform.position;
                    transform.forward = (transform.position - scarePoint).normalized;
                    runtimer = Runtime;
                    scared = true;
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

                    // Find the average position of all transforms in the fishes array
                    Vector3 averagePosition = Vector3.zero;
                    foreach (Collider fish in fishes)
                    {
                        averagePosition += fish.transform.position;
                    }
                    averagePosition /= fishes.Length;

                    transform.forward = Vector3.Lerp(transform.forward, averagePosition - transform.position, CenterBias);
                }
            }

            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, GroundHoverHeight, (1 << LayerMask.NameToLayer("Ground"))))
            {
                float hitDistance = hit.distance;
                transform.forward = Vector3.Lerp(transform.forward, Vector3.up, 1f - (hitDistance / GroundHoverHeight));
            }

            Collider[] hits = Physics.OverlapSphere(transform.position, SocialDistancing, 1 << LayerMask.NameToLayer("Sheep")).Where(x => x.gameObject != gameObject).ToArray();
            foreach (Collider sheep in hits)
            {
                Vector3 direction = sheep.transform.position - transform.position;
                transform.forward = Vector3.Lerp(transform.forward, direction, 1f - (Vector3.Distance(transform.position, sheep.transform.position) / SocialDistancing));
            }
        }
    EscapeOrders:

        if (runtimer > 0)
        {
            
            //Debug.Log("Setting speed");
            rb.velocity = transform.forward * Speed;
            runtimer -= Time.deltaTime;
        } else scared = false;

        CurrentSpeed = rb.velocity.magnitude;
    }

    public void ForceGroup(Vector3 target)
    {
        forceGroupTimer = ForceGroupDuration;
        ForceTarget = target;
    }

    public void Capture(Transform captor)
    {
        transform.forward = captor.right;
        transform.parent = captor;
        transform.localPosition = Vector3.forward * 2.5f;
        rb.isKinematic = true;
        captured = true;
    }

    public void Release()
    {
        transform.parent = null;
        rb.isKinematic = false;
        captured = false;
    }

    public void Kill()
    {
        GameManager.Instance.SheepCount--;
        Destroy(gameObject);
    }
}
