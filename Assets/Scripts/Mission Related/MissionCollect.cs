using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MissionCollect : Mission
{
    public List<GameObject> Pickups;
    private List<IPickup> pickups;
    private int totalPickups;
    private int currentPickups;
    // Start is called before the first frame update
    void Start()
    {
        try
        {
            pickups = Pickups.Select(x => x.GetComponent<IPickup>()).ToList();
            
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
        }
        totalPickups = pickups.Count;
        foreach (IPickup pickup in pickups) pickup.PickedUp += PickedUp;
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
