using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogStayOutside : MonoBehaviour
{
    public bool StayOutside;
    public EntityDog Dog;
    // Start is called before the first frame update
    void Start()
    {
        Dog = GameObject.Find("Dog").GetComponent<EntityDog>();
    }

    // Update is called once per frame
    void Update()
    {
     if(StayOutside == true)
        {
            Dog.Outside = true;
        }
        else
        {
            Dog.Outside = false;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player") 
        { 
            StayOutside = true; 
        }
        
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            StayOutside = false;
        }
    }
}
