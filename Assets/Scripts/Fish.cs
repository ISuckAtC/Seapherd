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
        Collider[] col = Physics.OverlapSphere(transform.position, DetectionRange, (1 << LayerMask.NameToLayer("Dog") | 1 << LayerMask.NameToLayer("Fish") | 1 << LayerMask.NameToLayer("Bear")));

        if (col.Length > 1)
        {
            if (col.Any(x => x.gameObject.layer == LayerMask.NameToLayer("Dog")) || col.Any(x => x.gameObject.layer == LayerMask.NameToLayer("Bear")))
            {
                transform.forward = (transform.position - col.First(x => x.gameObject.layer == LayerMask.NameToLayer("Dog") || x.gameObject.layer == LayerMask.NameToLayer("Bear")).transform.position).normalized;
                runtimer = Runtime;
            }
            Collider[] fishes = col.Where(x => x.gameObject.layer == LayerMask.NameToLayer("Fish")).ToArray();
            Vector3 averageDirection = Vector3.zero;
            foreach (Collider fish in fishes)
            {
                averageDirection += fish.transform.forward;
            }
            averageDirection /= fishes.Length;

            transform.forward = Vector3.Lerp(transform.forward, averageDirection, DirectionInfluence);
        }

        if (runtimer > 0)
        {
            Debug.Log("Setting speed");
            rb.velocity = transform.forward * Speed;
            runtimer -= Time.deltaTime;
        }

        CurrentSpeed = rb.velocity.magnitude;
    }
}
