using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillerFish : MonoBehaviour
{
    public Transform Player, ReturnMarker, InBoundsSpawn, Sheepfish;
    public bool Leave, ChasePlayer, ChaseSheep;

    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        ReturnMarker = GameObject.FindGameObjectWithTag("KillerFishSpawn").transform;
        InBoundsSpawn = GameObject.FindGameObjectWithTag("PlayerRespawn").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(ChaseSheep  == true)
        {
            transform.LookAt(Sheepfish);
            transform.position += transform.forward / 5 * timer;
        }
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
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Player.transform.position = InBoundsSpawn.position;
        }
        if(other.tag == "Sheep")
        {
            Sheepfish.GetComponent<EntitySheep>().Kill();
        }
    }
}
