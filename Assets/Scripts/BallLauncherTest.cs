using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallLauncherTest : MonoBehaviour
{
    public Rigidbody projectile;
    public float speed = 20f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Rigidbody clone;
            clone = Instantiate(projectile, transform.position, transform.rotation) as Rigidbody;

            clone.linearVelocity = transform.TransformDirection(Vector3.forward * speed);
        }
    }
}
