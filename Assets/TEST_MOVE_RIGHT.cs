using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST_MOVE_RIGHT : MonoBehaviour
{
    public float MovePosition;
    public float MoveX;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("MoveRight", 2f, 1f);

        System.Runtime.InteropServices.GCHandle handle = System.Runtime.InteropServices.GCHandle.Alloc(MovePosition, System.Runtime.InteropServices.GCHandleType.Pinned);
        System.IntPtr pointer = System.Runtime.InteropServices.GCHandle.ToIntPtr(handle);
        Debug.Log(pointer);

        System.Runtime.InteropServices.GCHandle handlea = System.Runtime.InteropServices.GCHandle.Alloc(MoveX, System.Runtime.InteropServices.GCHandleType.Pinned);
        System.IntPtr pointera = System.Runtime.InteropServices.GCHandle.ToIntPtr(handlea);
        Debug.Log(pointera);
    }

    void MoveRight()
    {
        transform.position = new Vector3(MoveX, MovePosition, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
