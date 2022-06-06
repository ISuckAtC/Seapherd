using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillerFish : MonoBehaviour
{
    public Transform Player, ReturnMarker, InBoundsSpawn, Sheepfish;
    public bool Leave, ChasePlayer, ChaseSheep;

    public float timer;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        ReturnMarker = gameObject.transform.parent.GetChild(0).transform;
        InBoundsSpawn = GameObject.FindGameObjectWithTag("PlayerRespawn").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(ChaseSheep  == true)
        {
            transform.LookAt(Sheepfish);
            transform.position += transform.forward  *timer /2;
        }
        if(ChasePlayer == true)
        {
            Player = GameObject.FindGameObjectWithTag("Player").transform;
            transform.LookAt(Player);
            transform.position += transform.forward  *timer/2;
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
        timer += Time.deltaTime;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Player.transform.position = InBoundsSpawn.transform.position;
        }
        if(other.tag == "Sheep")
        {
            Sheepfish.GetComponent<EntitySheep>().Kill();
        }
    }
}
