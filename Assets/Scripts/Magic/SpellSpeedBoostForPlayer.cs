using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellSpeedBoostForPlayer : MonoBehaviour
{
    PlayerController Player;

    public float NewSpeedMultiplier = 2f, BoostDuration = 5f;

    private float OldSpeedMultiplier = 1f;

    bool CanBoost = false;

    public KeyCode DebugBoost = KeyCode.Alpha1;
    // Start is called before the first frame update
    void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(DebugBoost))
        {
            StartCoroutine(SpeedIncrease());
        }
    }

    public IEnumerator SpeedIncrease()
    {
        if (CanBoost)
        {
            CanBoost = false;
            OldSpeedMultiplier = Player.ExternalSpeedMod;
            Player.ExternalSpeedMod = Player.ExternalSpeedMod * NewSpeedMultiplier;
            yield return new WaitForSeconds(BoostDuration);
            SpeedReset();
        }
    }

    public void SpeedReset()
    {
        Player.ExternalSpeedMod = OldSpeedMultiplier;
        CanBoost = true;
    }
}
