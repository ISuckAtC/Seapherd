using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.InputSystem;
using TMPro;
using FMOD;
using FMODUnity;

public class PlayerController : MonoBehaviour
{
    public FMODUnity.EventReference pickupEvent, magicEvent, boostMagicEvent; //So far using only pickupEvent. The playerSpeedWhoosh will not be among these

    public float Speed;
    public float StoppingBoost;

    public float ThreatenRange;
    public float ThreatenAmount;
    public float InteractRange;
    public GameObject Marker, Dog;
    private Rigidbody rb;
    private Vector3 direction;
    public bool stopMarker;
    public Transform HandControllerR;
    public Transform HandControllerL;
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
    [HideInInspector]
    public float ExternalSpeedMod = 1f;

    public float UnfuckValue;
    public float HandDragExponent;
    public float HandDragMultiplier;
    private bool unfuckVrStart;
    private Vector3 lastRHandPos, lastLHandPos, lastBodyPos;

    public GameObject GripR, GripL;

    public enum ControlType
    {
        KBM,
        VR_Joystick,
        VR_Leading,
        VR_Dragging
    }
    public ControlType Control;
    // Start is called before the first frame update
    void Start()
    {
        Control = GameManager._Settings.controlType;
        Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>();
        (new System.Threading.Thread(() =>
        {
            System.Threading.Thread.Sleep(500);
            unfuckVrStart = true;
        })).Start();
        if (Control != ControlType.VR_Leading) ConfigText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (unfuckVrStart)
        {
            transform.position = new Vector3(transform.position.x, UnfuckValue, transform.position.z);
            unfuckVrStart = false;
        }

        switch (Control)
        {
            case ControlType.KBM:
                KBMControls();
                break;
            case ControlType.VR_Joystick:
                VRJoystickControls();
                break;
            case ControlType.VR_Leading:
                VRLeadingControls();
                break;
            case ControlType.VR_Dragging:
                VRDraggingControls();
                break;
        }
    }

    void FixedUpdate()
    {
        //Debug.Log(direction);
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

        if (Control != ControlType.VR_Dragging)
        {
            Vector3 currentVelocity = rb.velocity;
            Vector3 alignment = currentVelocity.normalized - direction;
            Vector3 movementForce = new Vector3(
                direction.x * ((alignment.x > 1 || alignment.x < -1) ? StoppingBoost : 1),
                direction.y * ((alignment.y > 1 || alignment.y < -1) ? StoppingBoost : 1),
                direction.z * ((alignment.z > 1 || alignment.z < -1) ? StoppingBoost : 1)) * Speed * speedMod * ExternalSpeedMod;
            rb.AddForce(movementForce, ForceMode.Acceleration);
        }
    }

    void KBMControls()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UnityEngine.Debug.Log("Quit");
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
                    Application.Quit(0);
#endif
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Physics.Raycast(transform.position + new Vector3(0.0f, 0.5f, 0.0f), transform.forward, out RaycastHit hit, InteractRange, 1 << LayerMask.NameToLayer("Artifact") | 1 << LayerMask.NameToLayer("NPC")))
            {
                if (hit.transform.TryGetComponent<IPickup>(out IPickup pickup))
                {
                    GameManager.Instance.SplashText("You picked up " + pickup.PickupName);
                    GameManager.Instance.artifactGET = true;

                    var pickupSound = FMODUnity.RuntimeManager.CreateInstance(pickupEvent);
                    ATTRIBUTES_3D attributes;
                    attributes.position = this.transform.position.ToFMODVector();
                    attributes.velocity = rb.velocity.ToFMODVector();
                    attributes.forward = this.transform.forward.ToFMODVector();
                    attributes.up = this.transform.up.ToFMODVector();
                    pickupSound.set3DAttributes(attributes);
                    pickupSound.start();
                    pickupSound.release();

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

        speedMod = 1f;

        float updown = ((Input.GetKey(KeyCode.Space) ? 1 : 0) + (Input.GetKey(KeyCode.LeftShift) ? -1 : 0));

        direction = (transform.right * Input.GetAxisRaw("DHorizontal") + transform.up * updown + transform.forward * Input.GetAxisRaw("DVertical")).normalized;
    }
    void VRJoystickControls()
    {
        if (Input.GetButton("LStickPush") && Input.GetButton("RStickPush"))
        {
            UnityEngine.Debug.Log("Quit");
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
                    Application.Quit(0);
#endif
        }
        
        Vector2 joyAxis = new Vector2(Input.GetAxisRaw("RHorizontal"), Input.GetAxisRaw("RVertical"));

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, joyAxis.x, 0));
        //transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(-joyAxis.y, 0, 0));

        FaceButtonControls();

        speedMod = 1f;
        float updown;

        updown = Input.GetAxisRaw("RShoulder") - Input.GetAxisRaw("LShoulder");

        direction = (transform.right * Input.GetAxisRaw("Horizontal") + transform.up * updown + transform.forward * Input.GetAxisRaw("Vertical")).normalized;
    }
    void VRLeadingControls()
    {
        if (Input.GetButton("LStickPush") && Input.GetButton("RStickPush"))
        {
            UnityEngine.Debug.Log("Quit");
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
                    Application.Quit(0);
#endif
        }

        FaceButtonControls();

        Vector2 joyAxis = new Vector2(Input.GetAxisRaw("RHorizontal"), Input.GetAxisRaw("RVertical"));

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, joyAxis.x, 0));

        if (Input.GetButtonDown("B"))
        {
            if (handControlConfigMin)
            {
                HandOrigin.position = HandControllerR.position;
                handControlConfigMin = false;
                handControlConfigMax = true;
                ConfigText.text = "Move your hand far away from your body (max radius), press B to confirm";
            }
            else if (handControlConfigMax)
            {
                handControlMax = Vector3.Distance(HandOrigin.position, HandControllerR.position);
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
        else if (Input.GetAxisRaw("RGrip") > 0.5f) // TODO: Add a button to hold to move
        {
            Vector3 handPosition = HandControllerR.position;
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
    void VRDraggingControls()
    {
        if (Input.GetButton("LStickPush") && Input.GetButton("RStickPush"))
        {
            UnityEngine.Debug.Log("Quit");
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
                    Application.Quit(0);
#endif
        }

        FaceButtonControls();

        Vector2 joyAxis = new Vector2(Input.GetAxisRaw("RHorizontal"), Input.GetAxisRaw("RVertical"));

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, joyAxis.x, 0));

        if (Input.GetAxisRaw("LGrip") > 0.5f)// && !GripL) // TODO: Add a button to hold to drag yourself
        {
            Vector3 drag = (lastLHandPos - (lastBodyPos - transform.position)) - HandControllerL.position;
            drag *= HandDragMultiplier;
            rb.velocity += drag.normalized * Mathf.Pow(drag.magnitude, HandDragExponent);
        }
        if (Input.GetAxisRaw("RGrip") > 0.5f)// && !GripR) // TODO: Add a button to hold to drag yourself
        {
            Vector3 drag = (lastRHandPos - (lastBodyPos - transform.position)) - HandControllerR.position;
            drag *= HandDragMultiplier;
            //Debug.Log(drag.normalized * Mathf.Pow(drag.magnitude, HandDragExponent));
            rb.velocity += drag.normalized * Mathf.Pow(drag.magnitude, HandDragExponent);
        }

        lastLHandPos = HandControllerL.position;
        lastRHandPos = HandControllerR.position;
        lastBodyPos = transform.position;
    }

    void FaceButtonControls()
    {
        if (Input.GetButtonDown("RStickPush"))
        {
            int index = (int)Control;
            index++;
            if (index >= 4)
            {
                index = 1;
            }
            Control = (ControlType)index;
            GameManager._Settings.controlType = Control;
        }
        if (Input.GetButtonDown("X"))
        {
            if (Physics.Raycast(transform.position + new Vector3(0.0f, 0.5f, 0.0f), transform.forward, out RaycastHit hit, InteractRange, 1 << LayerMask.NameToLayer("Artifact") | 1 << LayerMask.NameToLayer("NPC")|1<< LayerMask.NameToLayer("Bear")))
            {
                if (hit.transform.TryGetComponent<IPickup>(out IPickup pickup))
                {
                    GameManager.Instance.SplashText("You picked up " + pickup.PickupName);
                    GameManager.Instance.artifactGET = true;

                    var pickupSound = FMODUnity.RuntimeManager.CreateInstance(pickupEvent);
                    ATTRIBUTES_3D attributes;
                    attributes.position = this.transform.position.ToFMODVector();
                    attributes.velocity = rb.velocity.ToFMODVector();
                    attributes.forward = this.transform.forward.ToFMODVector();
                    attributes.up = this.transform.up.ToFMODVector();
                    pickupSound.set3DAttributes(attributes);
                    pickupSound.start();
                    pickupSound.release();

                    Destroy(hit.transform.gameObject);
                }
                if (hit.transform.tag == "Quest")
                {
                    hit.transform.GetComponent<MissionGiver>().StartMission();
                }
                if(hit.transform.tag == "Bear")
                {
                    Dog.GetComponent<EntityDog>().State = DogState.Chase;
                }
            }
        }
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
}
