using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerController : MonoBehaviour
{
    public float Speed;
    public float StoppingBoost;

    public float ThreatenRange;
    public float ThreatenAmount;
    public float InteractRange;

    private Rigidbody rb;
    private Vector3 direction;
    public TMPro.TextMeshProUGUI ToolTipText;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 averagePosition = Vector3.zero;
            foreach (Fish fish in GameObject.FindObjectsOfType<Fish>())
            {
                averagePosition += fish.transform.position;
            }
            averagePosition /= GameObject.FindObjectsOfType<Fish>().Length;
            foreach (Fish fish in GameObject.FindObjectsOfType<Fish>())
            {
                fish.ForceGroup(averagePosition);
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            Collider[] col = Physics.OverlapSphere(transform.position, ThreatenRange, (1 << LayerMask.NameToLayer("Bear")));

            if (col.Length > 0)
            {
                foreach (Collider c in col)
                {
                    c.GetComponentInParent<EntityBear>().Scare(ThreatenAmount);
                }
            }
        }
        Vector2 mouseAxis = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, mouseAxis.x, 0));
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(-mouseAxis.y, 0, 0));

        direction = (transform.right * Input.GetAxisRaw("Horizontal") + transform.up * ((Input.GetKey(KeyCode.Space) ? 1 : 0) + (Input.GetKey(KeyCode.LeftShift) ? -1 : 0)) + transform.forward * Input.GetAxisRaw("Vertical")).normalized;
    }

    void FixedUpdate()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, InteractRange))
        {
            if (hit.transform.TryGetComponent<IToolTip>(out IToolTip toolTip))
            {
                ToolTipText.text = toolTip.ToolTip;
            }
            else
            {
                ToolTipText.text = "";
            }
        }
        else
        {
            ToolTipText.text = "";
        }
        Vector3 currentVelocity = rb.velocity;
        Vector3 alignment = currentVelocity.normalized - direction;
        Vector3 movementForce = new Vector3(
            direction.x * ((alignment.x > 1 || alignment.x < -1) ? StoppingBoost : 1),
            direction.y * ((alignment.y > 1 || alignment.y < -1) ? StoppingBoost : 1),
            direction.z * ((alignment.z > 1 || alignment.z < -1) ? StoppingBoost : 1)) * Speed;
        rb.AddForce(movementForce, ForceMode.Acceleration);
    }
}
