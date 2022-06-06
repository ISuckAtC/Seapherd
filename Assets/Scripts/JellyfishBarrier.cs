using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyfishBarrier : MonoBehaviour
{
    public float timer, maxTimer;
    public GameObject KillerFish, Player, Sheepfish;
    public bool SpawnOnce;
    // Start is called before the first frame update
    void Start()
    {
        KillerFish = this.gameObject.transform.GetChild(1).gameObject;
        KillerFish.SetActive(false);
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
        
        if(other.tag == "Sheep")
        {
            timer += Time.deltaTime;
            Sheepfish = other.gameObject;
        }
        if (timer > maxTimer)
        {
           
            
               KillerFish.SetActive(true);
                
            
           
            if(other.tag == "sheep")
            {
                KillerFish.GetComponent<KillerFish>().Sheepfish = Sheepfish.transform;
                KillerFish.GetComponent<KillerFish>().ChaseSheep = true;
                goto skipPlayerChase;
            }
            KillerFish.GetComponent<KillerFish>().ChasePlayer = true;
        skipPlayerChase:;
            KillerFish.GetComponent<KillerFish>().Leave = false;
        }
        
    }
    private void OnTriggerExit(Collider other)
    {if(other.tag == "Player"|| other.tag == "Sheep")
        {
            timer = 0;
            KillerFish.GetComponent<KillerFish>().ChasePlayer = false;
            KillerFish.GetComponent<KillerFish>().Leave = true;
        }
      
    }
}
