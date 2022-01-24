using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float Speed;
    public float StoppingBoost;


    private Rigidbody rb;
    private Vector3 direction;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mouseAxis = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, mouseAxis.x, 0));
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(-mouseAxis.y, 0, 0));

        direction = (transform.right * Input.GetAxisRaw("Horizontal") + transform.up * ((Input.GetKey(KeyCode.Space) ? 1 : 0) + (Input.GetKey(KeyCode.LeftShift) ? -1 : 0)) + transform.forward * Input.GetAxisRaw("Vertical")).normalized;
    }

    void FixedUpdate()
    {
        Vector3 currentVelocity = rb.velocity;
        Vector3 alignment = currentVelocity.normalized - direction;
        Vector3 movementForce = new Vector3(
            direction.x * ((alignment.x > 1 || alignment.x < -1) ? StoppingBoost : 1), 
            direction.y * ((alignment.y > 1 || alignment.y < -1) ? StoppingBoost : 1),
            direction.z * ((alignment.z > 1 || alignment.z < -1) ? StoppingBoost : 1)) * Speed;
        rb.AddForce(movementForce, ForceMode.Acceleration);
    }
}
