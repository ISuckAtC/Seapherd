using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityCutter : MonoBehaviour
{
    private Vector3 Origin;
    public float DetectionRadius;
    public Transform Target;
    private Rigidbody rb;
    public float Speed;

    // Start is called before the first frame update
    void Start()
    {
        Origin = transform.position;
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Collider[] sheep = Physics.OverlapSphere(transform.position, DetectionRadius, 1 << LayerMask.NameToLayer("Sheep"), QueryTriggerInteraction.UseGlobal);
        if (sheep.Length > 0)
        {
            Target = sheep[0].transform;
        }
        else
        {
            Target = null;
        }

        if (Target)
        {
            transform.forward = (Target.position - transform.position).normalized;
            rb.velocity = transform.forward * Speed;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionStay()
    {
        
    }
}
