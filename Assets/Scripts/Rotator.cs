using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float x;
    public float y = 0.25f;
    public float z;
    
    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Rotate(x, y, z, Space.World);
    }
}
