using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyForce : MonoBehaviour
{
    public Rigidbody projectile;
    public float speed = 20f;
    public AudioSource audioSource;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            audioSource.Play();
            projectile.AddForce(new Vector3(-10, 0, 0) * speed);
        }
    }
}
