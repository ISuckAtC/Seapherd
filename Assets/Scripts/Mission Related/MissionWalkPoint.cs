using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionWalkPoint : MonoBehaviour
{
    public event System.Action<int> PointReachedCallback;
    public int Index;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Debug.Log("Index: " + Index);
            PointReachedCallback.Invoke(Index);
        }
    }
}
