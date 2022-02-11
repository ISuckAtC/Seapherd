using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class PlayerVRController : MonoBehaviour
{
    public SteamVR_Action_Vector2 MoveInput;
    public float Speed = 1, GravityMagnitude = 1f;

    private CharacterController character;

    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 Direction = Player.instance.hmdTransform.TransformDirection(new Vector3(MoveInput.axis.x, 0, MoveInput.axis.y));
        character.Move((Speed * Time.deltaTime * Direction) - (new Vector3(0, GravityMagnitude, 0) * Time.deltaTime));
    }
}
