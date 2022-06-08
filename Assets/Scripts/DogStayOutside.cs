using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogStayOutside : MonoBehaviour
{
    public bool StayOutside;
    public LightFixingOutdoors LFO;
    public EntityDog Dog;
    private bool FireOnce;
    // Start is called before the first frame update
    void Start()
    {
        Dog = GameObject.Find("Dog").GetComponent<EntityDog>();
        LFO = GameObject.FindGameObjectWithTag("Lights").GetComponent<LightFixingOutdoors>();
    }

    // Update is called once per frame
    void Update()
    {
     if(StayOutside == true)
        {
            GameManager.Instance.InTavern = true;
            Dog.Outside = true;
        }
        else
        {
            GameManager.Instance.InTavern = false;
            Dog.Outside = false;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player") 
        {if( FireOnce== false)
            {
                LFO.DoOnce = false;
                FireOnce = true;
            }
            StayOutside = true; 
        }
        
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            LFO.DoOnce = false;
            StayOutside = false;
            FireOnce = false;
        }
    }
}
