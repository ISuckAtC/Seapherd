using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : MonoBehaviour
{
    public bool Moving;
    public float Speed;
    public Transform Player;
    public Transform PlayerCam;
    private Vector3 oldPos;
    private Vector3 startLocal;
    private Vector2 mouseOld;
    // Start is called before the first frame update
    void Start()
    {
        oldPos = transform.position;
        startLocal = transform.localPosition;
        mouseOld = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = Vector3.zero;
        direction = Input.GetAxisRaw("Horizontal") * Player.right + Input.GetAxisRaw("Vertical") * Player.forward + ((Input.GetKey(KeyCode.Space) ? -1f : 0f) + (Input.GetKey(KeyCode.LeftShift) ? 1f : 0f)) * Player.up;
        if (direction.magnitude > 0)
        {
            transform.forward = direction.normalized;
            Moving = true;
        }
        else
        {
            Moving = false;
        }

        Vector2 mouseAxis = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        Player.rotation = Quaternion.Euler(Player.rotation.eulerAngles + new Vector3(0, mouseAxis.x, 0));
        Player.rotation = Quaternion.Euler(Player.rotation.eulerAngles + new Vector3(-mouseAxis.y, 0, 0));
    }

    void FixedUpdate()
    {
        Vector3 diff = transform.position - oldPos;
        transform.localPosition = startLocal;
        Player.transform.position += diff;
        oldPos = transform.position;
        if (Moving)
        {
            GetComponent<Rigidbody>().velocity = transform.forward * Speed;
        }
    }
}
