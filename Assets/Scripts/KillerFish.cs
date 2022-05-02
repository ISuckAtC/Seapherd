using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillerFish : MonoBehaviour
{
    public Transform Player, ReturnMarker;
    public bool Leave, ChasePlayer;

    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        ReturnMarker = GameObject.FindGameObjectWithTag("KillerFishSpawn").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(ChasePlayer == true)
        {
            transform.LookAt(Player);
            transform.position += transform.forward / 5 *timer;
        }
        if(Leave == true)
        {
            transform.LookAt(ReturnMarker);
            transform.position += transform.forward * timer;
        }
        if(timer >10 && ChasePlayer == false)
        {
            timer = 0;
            gameObject.SetActive(false);
            
        }
    }
}
