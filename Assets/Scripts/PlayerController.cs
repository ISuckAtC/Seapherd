using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float Speed;
    public float StoppingBoost;

    public float ThreatenRange;
    public float ThreatenAmount;
    public float InteractRange;
    public GameObject Marker;
    private Rigidbody rb;
    private Vector3 direction;
    public bool stopMarker;
    public bool UsingXR;
    public bool HandControlMovement;
    public Transform HandController;
    public Transform HandOrigin;
    [SerializeField]
    private float speedMod = 1f;
    public float HandControlDeadzone;
    [SerializeField]
    private float handControlMin = 100f;
    [SerializeField]
    private float handControlMax = 100f;
    private bool handControlConfigMin = true;
    private bool handControlConfigMax;
    public TextMeshProUGUI ConfigText;

    public float UnfuckValue;
    private bool unfuckVrStart;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>();
        (new System.Threading.Thread(() =>
        {
            System.Threading.Thread.Sleep(500);
            unfuckVrStart = true;
        })).Start();
        if (!HandControlMovement) ConfigText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (unfuckVrStart)
        {
            transform.position = new Vector3(transform.position.x, UnfuckValue, transform.position.z);
            unfuckVrStart = false;
        }
        if (UsingXR)
        {
            if (Input.GetButton("LStickPush") && Input.GetButton("RStickPush"))
            {
                Debug.Log("Quit");
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                    Application.Quit(0);
#endif
            }
            if (Input.GetButtonDown("X"))
            {
                if (Physics.Raycast(transform.position + new Vector3(0.0f, 0.5f, 0.0f), transform.forward, out RaycastHit hit, InteractRange, 1 << LayerMask.NameToLayer("Artifact") | 1 << LayerMask.NameToLayer("NPC")))
                {
                    if (hit.transform.TryGetComponent<IPickup>(out IPickup pickup))
                    {
                        GameManager.Instance.SplashText("You picked up " + pickup.PickupName);
                        GameManager.Instance.artifactGET = true;
                        Destroy(hit.transform.gameObject);
                    }
                    if (hit.transform.tag == "Quest")
                    {
                        hit.transform.GetComponent<MissionGiver>().StartMission();
                    }
                }
            }
            Vector2 joyAxis = new Vector2(Input.GetAxisRaw("RHorizontal"), Input.GetAxisRaw("RVertical"));

            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, joyAxis.x, 0));
            //transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(-joyAxis.y, 0, 0));

            if (Input.GetButtonDown("A"))
            {
                Vector3 averagePosition = Vector3.zero;
                foreach (EntitySheep fish in GameObject.FindObjectsOfType<EntitySheep>())
                {
                    averagePosition += fish.transform.position;
                }
                averagePosition /= GameObject.FindObjectsOfType<EntitySheep>().Length;
                foreach (EntitySheep fish in GameObject.FindObjectsOfType<EntitySheep>())
                {
                    fish.ForceGroup(averagePosition);
                }
            }
            if (Input.GetButtonDown("Y"))
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
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (Physics.Raycast(transform.position + new Vector3(0.0f, 0.5f, 0.0f), transform.forward, out RaycastHit hit, InteractRange, 1 << LayerMask.NameToLayer("Artifact") | 1 << LayerMask.NameToLayer("NPC")))
                {
                    if (hit.transform.TryGetComponent<IPickup>(out IPickup pickup))
                    {
                        GameManager.Instance.SplashText("You picked up " + pickup.PickupName);
                        GameManager.Instance.artifactGET = true;
                        Destroy(hit.transform.gameObject);
                    }
                    if (hit.transform.tag == "Quest")
                    {
                        hit.transform.GetComponent<MissionGiver>().StartMission();
                    }
                }
            }
            if (Input.GetMouseButtonDown(1))
            {
                Vector3 averagePosition = Vector3.zero;
                foreach (EntitySheep fish in GameObject.FindObjectsOfType<EntitySheep>())
                {
                    averagePosition += fish.transform.position;
                }
                averagePosition /= GameObject.FindObjectsOfType<EntitySheep>().Length;
                foreach (EntitySheep fish in GameObject.FindObjectsOfType<EntitySheep>())
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
        }

        if (HandControlMovement)
        {
            if (Input.GetButtonDown("B"))
            {
                if (handControlConfigMin)
                {
                    HandOrigin.position = HandController.position;
                    handControlConfigMin = false;
                    handControlConfigMax = true;
                    ConfigText.text = "Move your hand far away from your body (max radius), press B to confirm";
                }
                else if (handControlConfigMax)
                {
                    handControlMax = Vector3.Distance(HandOrigin.position, HandController.position);
                    handControlMin = Mathf.Lerp(0, handControlMax, HandControlDeadzone);
                    handControlConfigMax = false;
                    ConfigText.text = "";
                }
                else
                {
                    handControlConfigMin = true;
                    ConfigText.text = "Move your hand to your origin, press B to confirm";
                }
            }
            if (handControlConfigMin || handControlConfigMax)
            {
                speedMod = 0f;
            }
            else
            {
                Vector3 handPosition = HandController.position;
                float distance = Vector3.Distance(HandOrigin.position, handPosition);
                if (distance < handControlMin)
                {
                    speedMod = 0f;
                }
                else if (distance > handControlMax)
                {
                    speedMod = 1f;
                }
                else
                {
                    speedMod = distance / handControlMax;
                }
                direction = (handPosition - HandOrigin.position).normalized;
            }
        }
        else
        {
            speedMod = 1f;
            float updown;
            if (UsingXR)
            {
                updown = Input.GetAxisRaw("RShoulder") - Input.GetAxisRaw("LShoulder");
            }
            else
            {
                updown = ((Input.GetKey(KeyCode.Space) ? 1 : 0) + (Input.GetKey(KeyCode.LeftShift) ? -1 : 0));
            }

            Debug.Log(Input.GetAxisRaw("Horizontal"));

            direction = (transform.right * (UsingXR ? Input.GetAxisRaw("Horizontal") : Input.GetAxisRaw("DHorizontal")) + transform.up * updown + transform.forward * (UsingXR ? Input.GetAxisRaw("Vertical") : Input.GetAxisRaw("DVertical"))).normalized;
        }
    }

    void FixedUpdate()
    {
        Debug.Log(direction);
        if (Physics.Raycast(transform.position + new Vector3(0.0f, 0.5f, 0.0f), transform.forward, out RaycastHit hit, InteractRange))
        {
            if (hit.transform.TryGetComponent<IToolTip>(out IToolTip toolTip))
            {
                GameManager.Instance.ToolTipText.text = toolTip.ToolTip;
            }
            else
            {
                GameManager.Instance.ToolTipText.text = "";
            }
        }
        else
        {
            GameManager.Instance.ToolTipText.text = "";
        }
        
        Vector3 currentVelocity = rb.velocity;
        Vector3 alignment = currentVelocity.normalized - direction;
        Vector3 movementForce = new Vector3(
            direction.x * ((alignment.x > 1 || alignment.x < -1) ? StoppingBoost : 1),
            direction.y * ((alignment.y > 1 || alignment.y < -1) ? StoppingBoost : 1),
            direction.z * ((alignment.z > 1 || alignment.z < -1) ? StoppingBoost : 1)) * Speed * speedMod;
        rb.AddForce(movementForce, ForceMode.Acceleration);
    }
}
