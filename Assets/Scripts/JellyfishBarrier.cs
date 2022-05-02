using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyfishBarrier : MonoBehaviour
{
    public float timer, maxTimer;
    public GameObject KillerFish, Player;
    public bool SpawnOnce;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("PlayerParent");
        if(maxTimer == 0)
        {
            maxTimer = 10;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            timer += Time.deltaTime;
        }
        

        if (timer > maxTimer)
        {
            if (!SpawnOnce)
            {
                Instantiate(KillerFish);
                SpawnOnce = true;
            }
            if (SpawnOnce)
            {
                KillerFish.SetActive(true);
            }
            KillerFish.GetComponent<KillerFish>().ChasePlayer = true;
            KillerFish.GetComponent<KillerFish>().Leave = false;
        }
        
    }
    private void OnTriggerExit(Collider other)
    {
        timer = 0;
        KillerFish.GetComponent<KillerFish>().ChasePlayer = false;
        KillerFish.GetComponent<KillerFish>().Leave = true;
    }
}
