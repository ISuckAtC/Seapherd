using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EntityBear : MonoBehaviour
{
    public float Speed, ChaseSpeed, EscapeSpeed, StealSpeed;
    public float ChaseRange;
    public float EscapeThreshold;
    Rigidbody rb;
    EntitySheep capturedFish = null;
    public float threatLevel;
    public float ThreatThreshold;
    bool escaping;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (capturedFish || escaping)
        {
            Vector3 direction = transform.position - GameManager.Instance.Player.position;
            rb.velocity = direction.normalized * (escaping ? EscapeSpeed : StealSpeed);
            transform.forward = direction.normalized;

            if (Vector3.Distance(transform.position, GameManager.Instance.Player.position) > EscapeThreshold)
            {
                Debug.Log("Escaped");
                if (capturedFish) capturedFish.Kill();
                Destroy(gameObject);
            }
        }
        else
        {
            EntitySheep closestFish = GameManager.Instance.FishSheep.OrderBy(x => Vector3.Distance(x.transform.position, transform.position)).First();
            if (closestFish != null)
            {
                Vector3 direction = (closestFish.transform.position - transform.position) + new Vector3(0.0f, 0.5f, 0.0f);
                if (Vector3.Distance(closestFish.transform.position, transform.position) < ChaseRange)
                {
                    rb.velocity = direction.normalized * ChaseSpeed;
                }
                else
                {
                    rb.velocity = direction.normalized * Speed;
                }
                transform.forward = direction.normalized;
            }
        }
    }

    public void Scare(float threatAmount)
    {
        threatLevel += threatAmount;
        Debug.Log("Scared (" + threatLevel + "/" + ThreatThreshold + ")");
        if (threatLevel >= ThreatThreshold)
        {
            capturedFish.Release();
            capturedFish = null;
            escaping = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!escaping && !capturedFish && other.gameObject.layer == LayerMask.NameToLayer("Sheep"))
        {
            Debug.Log("Captured a sheep");
            capturedFish = other.gameObject.GetComponent<EntitySheep>();
            capturedFish.Capture(transform);
        }
    }
}
