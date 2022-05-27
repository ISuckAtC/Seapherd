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
    public FMODUnity.EventReference pickupEvent, magicEvent, boostMagicEvent, whistleEvent; //So far using only pickupEvent. The playerSpeedWhoosh will not be among these

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
    public float HandRotateMultiplier;
    public float HandRotateExponent;
    public float HandRotateDeadzone;
    public float HandRotateDeadExponent;
    public float StreamSpeedBoost;
    public float SpeedUpTimer;
    private bool speedUp;
    private bool unfuckVrStart;
    private Vector3 lastRHandPos, lastLHandPos, lastBodyPos;
    private bool lastRHandGrab, lastLHandGrab;

    public PlayerHand HandR, HandL;

    public GameObject PauseScreen;
    public GameObject DirectionArrowPrefab;
    private GameObject directionArrow;
    public enum ControlType
    {
        KBM,
        VR_Joystick,
        VR_Leading,
        VR_Dragging
    }
    public ControlType Control;
    private bool shoulderPressR, shoulderPressL;
    // Start is called before the first frame update
    void Start()
    {
        if (!directionArrow)
        {
            directionArrow = Instantiate(DirectionArrowPrefab, Vector3.zero, Quaternion.identity);
        }
        directionArrow.SetActive(false);
        Control = GameManager._Settings.controlType;
        Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>();
        (new System.Threading.Thread(() =>
        {
            System.Threading.Thread.Sleep(500);
            unfuckVrStart = true;
        })).Start();
        //if (Control != ControlType.VR_Leading) ConfigText.text = "";

        if (GameObject.FindGameObjectWithTag("PauseMenu") != null)
        {
            PauseScreen = GameObject.FindGameObjectWithTag("PauseMenu");
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (speedUp)
        {

            if (SpeedUpTimer <= 0)
            {
                speedUp = false;
                HandDragMultiplier -= StreamSpeedBoost;
                speedMod -= StreamSpeedBoost;
            }
            else SpeedUpTimer -= Time.deltaTime;
        }

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
        /*
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
        */

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

    void SpeedMagic(float spellDuration)
    {
        speedUp = true;
        SpeedUpTimer = spellDuration;

        HandDragMultiplier += StreamSpeedBoost;
        speedMod += StreamSpeedBoost;
    }
    void KBMControls()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && Input.GetKey(KeyCode.LeftShift))
        {

            UnityEngine.Debug.Log("Quit");
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
                    Application.Quit(0);
#endif
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseScreen.GetComponent<PauseMenu>().TogglePause();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Physics.Raycast(transform.position + new Vector3(0.0f, 0.5f, 0.0f), transform.forward, out RaycastHit hit, InteractRange, 1 << LayerMask.NameToLayer("Artifact") | 1 << LayerMask.NameToLayer("NPC")))
            {
                if (hit.transform.TryGetComponent<IPickup>(out IPickup pickup))
                {
                    GameManager.Instance.SplashText("You picked up " + pickup.PickupName);
                    GameManager.Instance.FoundArtifacts++;
                    GameManager.Instance.artifactGET = true;
                    if (hit.transform.GetComponent<SpecificArtifact>().HDD == true)
                    {
                        GameManager.Instance.HDD = true;
                    }
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
                    //hit.transform.GetComponent<MissionGiver>().StartMission();
                }
            }
        }
        //if (Input.GetKeyDown(KeyCode.Q) && !speedUp) 
        //{
        //    SpeedMagic(5);
        //}
        //this didnt work
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

        //Vector2 joyAxis = new Vector2(Input.GetAxisRaw("RHorizontal"), Input.GetAxisRaw("RVertical"));

        //transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, joyAxis.x, 0));

        /*if (Input.GetButtonDown("B"))
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
        */
        if (Input.GetAxisRaw("RGrip") > 0.5f) // TODO: Add a button to hold to move
        {
            Vector3 handPosition = HandControllerR.position;
            float distance = Vector3.Distance(Camera.main.transform.position, handPosition);
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
                speedMod = 1f;
            }
            direction = (handPosition - Camera.main.transform.position).normalized;
        }
    }
    void VRDraggingControls()
    {
        /*
        if (Input.GetButton("LStickPush") && Input.GetButton("RStickPush"))
        {
            UnityEngine.Debug.Log("Quit");
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
                    Application.Quit(0);
#endif
        }
        */

        FaceButtonControls();

        //Vector2 joyAxis = new Vector2(Input.GetAxisRaw("RHorizontal"), Input.GetAxisRaw("RVertical"));

        //transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, joyAxis.x, 0));

        if (Input.GetAxisRaw("LGrip") > 0.5f)// && !GripL) // TODO: Add a button to hold to drag yourself
        {
            Vector3 drag = (lastLHandPos - (lastBodyPos - transform.position)) - HandControllerL.position;
            drag *= HandDragMultiplier;
            rb.velocity += drag.normalized * Mathf.Pow(drag.magnitude, HandDragExponent);


            float distanceFromBody = Vector3.Distance(transform.position, HandControllerL.position);

            Vector3 lastArmRelative = (lastLHandPos - (lastBodyPos - transform.position)) - transform.position;

            Vector2 lastArmRelative2D = new Vector2(lastArmRelative.x, lastArmRelative.z);

            Vector3 currentArmRelative = HandControllerL.position - transform.position;

            Vector2 currentArmRelative2D = new Vector2(currentArmRelative.x, currentArmRelative.z);


            Vector2 handToBodyOffset = currentArmRelative2D.normalized;
            //UnityEngine.Debug.Log(handToBodyOffset);

            handToBodyOffset = new Vector2(
                handToBodyOffset.x < 0 ? -Mathf.Pow(Mathf.Abs(handToBodyOffset.x), HandRotateDeadExponent) : Mathf.Pow(handToBodyOffset.x, HandRotateDeadExponent), 
                handToBodyOffset.y < 0 ? -Mathf.Pow(Mathf.Abs(handToBodyOffset.y), HandRotateDeadExponent) : Mathf.Pow(handToBodyOffset.y, HandRotateDeadExponent)
            );


            Vector2 movement = currentArmRelative2D - lastArmRelative2D;

            movement = movement.normalized;

            movement = Vector2.Lerp(movement, new Vector2(movement.x * handToBodyOffset.y, movement.y * handToBodyOffset.x), HandRotateDeadzone);

            float angle = Vector2.SignedAngle(lastArmRelative2D, currentArmRelative2D);

            angle = Mathf.Lerp(angle, 0, 1 - movement.magnitude);

            //UnityEngine.Debug.Log(angle + " | " + HandRotateExponent + "|" + distanceFromBody + "|" + HandRotateMultiplier + "|" + handToBodyOffset);

            rb.angularVelocity += new Vector3(
                0,
                (
                    angle < 0 ? 
                        -Mathf.Pow(Mathf.Abs(angle) * distanceFromBody, HandRotateExponent) : 
                        Mathf.Pow(angle * distanceFromBody, HandRotateExponent)
                ) * HandRotateMultiplier,
                0
            );

            if (!lastLHandGrab) // GRABBED THIS FRAME
            {
                HandL.Grab();
            }
            lastLHandGrab = true;
        }
        else
        {
            if (lastLHandGrab) // RELEASED THIS FRAME
            {
                HandL.Release();
            }
            lastLHandGrab = false;
        }
        if (Input.GetAxisRaw("RGrip") > 0.5f)// && !GripR) // TODO: Add a button to hold to drag yourself
        {
            Vector3 drag = (lastRHandPos - (lastBodyPos - transform.position)) - HandControllerR.position;
            drag *= HandDragMultiplier;
            //Debug.Log(drag.normalized * Mathf.Pow(drag.magnitude, HandDragExponent));
            rb.velocity += drag.normalized * Mathf.Pow(drag.magnitude, HandDragExponent);

            float distanceFromBody = Vector3.Distance(transform.position, HandControllerR.position);

            Vector3 lastArmRelative = (lastRHandPos - (lastBodyPos - transform.position)) - transform.position;

            Vector2 lastArmRelative2D = new Vector2(lastArmRelative.x, lastArmRelative.z);

            Vector3 currentArmRelative = HandControllerR.position - transform.position;

            Vector2 currentArmRelative2D = new Vector2(currentArmRelative.x, currentArmRelative.z);


            Vector2 handToBodyOffset = currentArmRelative2D.normalized;
            handToBodyOffset = new Vector2(
                handToBodyOffset.x < 0 ? -Mathf.Pow(Mathf.Abs(handToBodyOffset.x), HandRotateDeadExponent) : Mathf.Pow(handToBodyOffset.x, HandRotateDeadExponent), 
                handToBodyOffset.y < 0 ? -Mathf.Pow(Mathf.Abs(handToBodyOffset.y), HandRotateDeadExponent) : Mathf.Pow(handToBodyOffset.y, HandRotateDeadExponent)
            );


            Vector2 movement = currentArmRelative2D - lastArmRelative2D;

            movement = movement.normalized;

            movement = Vector2.Lerp(movement, new Vector2(movement.x * handToBodyOffset.y, movement.y * handToBodyOffset.x), HandRotateDeadzone);

            float angle = Vector2.SignedAngle(lastArmRelative2D, currentArmRelative2D);

            angle = Mathf.Lerp(angle, 0, 1 - movement.magnitude);

            rb.angularVelocity += new Vector3(
                0,
                (
                    angle < 0 ? 
                        -Mathf.Pow(Mathf.Abs(angle) * distanceFromBody, HandRotateExponent) : 
                        Mathf.Pow(angle * distanceFromBody, HandRotateExponent)
                ) * HandRotateMultiplier,
                0
            );

            if (!lastRHandGrab) // GRABBED THIS FRAME
            {
                HandR.Grab();
            }
            lastRHandGrab = true;
        }
        else
        {
            if (lastRHandGrab) // RELEASED THIS FRAME
            {
                HandR.Release();
            }
            lastRHandGrab = false;
        }

        lastLHandPos = HandControllerL.position;
        lastRHandPos = HandControllerR.position;
        lastBodyPos = transform.position;
    }

    void FaceButtonControls()
    {
        if (Input.GetButtonDown("LStickPush"))
        {
            if (PauseScreen != null)
            {
                PauseScreen.GetComponent<PauseMenu>().TogglePause();
            }
        }
        if (Input.GetAxisRaw("RShoulder") > 0.5f)
        {
            if (!shoulderPressR)
            {
                shoulderPressR = true;
                if (Physics.Raycast(Camera.main.transform.position, (HandControllerR.transform.position - Camera.main.transform.position).normalized, out RaycastHit hit, InteractRange, 1 << LayerMask.NameToLayer("Artifact") | 1 << LayerMask.NameToLayer("NPC")))
                {
                    if (hit.transform.TryGetComponent<IPickup>(out IPickup pickup))
                    {
                        GameManager.Instance.SplashText("You picked up " + pickup.PickupName);
                        GameManager.Instance.FoundArtifacts++;
                        GameManager.Instance.artifactGET = true;
                        if (hit.transform.GetComponent<SpecificArtifact>().HDD == true)
                        {
                            GameManager.Instance.HDD = true;
                        }

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
                        //hit.transform.GetComponent<MissionGiver>().StartMission();
                    }
                }
            }
        }
        else if (shoulderPressR) shoulderPressR = false;
        /*if (Input.GetButtonDown("RStickPush"))
        {
            int index = (int)Control;
            index++;
            if (index >= 4)
            {
                index = 1;
            }
            Control = (ControlType)index;
            GameManager._Settings.controlType = Control;
        }*/
        /*if (Input.GetButtonDown("B"))
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
        }*/

        if (Input.GetButtonDown("B"))
        {
            Dog.GetComponent<EntityDog>().State = DogState.Follow;
        }

        if (Input.GetButton("A"))
        {
            if (!directionArrow.activeSelf)
            {
                directionArrow.SetActive(true);
            }

            Vector3 direction = (HandControllerR.position - Camera.main.transform.position).normalized;
            Vector3 averagePosition = Vector3.zero;
            foreach (EntitySheep sheep in GameManager.Instance.FishSheep)
            {
                averagePosition += sheep.transform.position;
            }
            averagePosition /= GameManager.Instance.FishSheep.Count;

            directionArrow.transform.position = averagePosition;
            directionArrow.transform.forward = direction;
        }

        if (Input.GetButtonUp("A"))
        {
            directionArrow.SetActive(false);
            // Command dog to herd sheep in direction
            Vector3 direction = (HandControllerR.position - Camera.main.transform.position).normalized;

            Dog.GetComponent<EntityDog>().State = DogState.Herd;
            Dog.GetComponent<EntityDog>().HerdDirection = direction;

            var whistleSound = FMODUnity.RuntimeManager.CreateInstance(whistleEvent);
            ATTRIBUTES_3D attributes;
            attributes.position = this.transform.position.ToFMODVector();
            attributes.velocity = rb.velocity.ToFMODVector();
            attributes.forward = this.transform.forward.ToFMODVector();
            attributes.up = this.transform.up.ToFMODVector();
            whistleSound.set3DAttributes(attributes);
            whistleSound.start();
            whistleSound.release();
        }



        if (Input.GetButtonDown("X"))
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

        if (Input.GetButtonDown("Y"))
        {
            SpeedMagic(5);
            /*
            Collider[] col = Physics.OverlapSphere(transform.position, ThreatenRange, (1 << LayerMask.NameToLayer("Bear")));

            if (col.Length > 0)
            {
                foreach (Collider c in col)
                {
                    c.GetComponentInParent<EntityBear>().Scare(ThreatenAmount);
                }
            }
            */
        }
    }
}
