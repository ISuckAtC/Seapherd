using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionCollect : Mission
{
    public List<IPickup> Pickups;
    private int totalPickups;
    private int currentPickups;
    // Start is called before the first frame update
    void Start()
    {
        totalPickups = Pickups.Count;
        foreach (IPickup pickup in Pickups) pickup.PickedUp += PickedUp;
    }

    public void PickedUp()
    {
        if (++currentPickups == totalPickups)
        {
            Continue();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
