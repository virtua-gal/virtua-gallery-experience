using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSound : MonoBehaviour
{
    public AudioSource hitAudio;

    void Start()
    {
        hitAudio = GetComponent<AudioSource>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Cybertruck"))
        {
            hitAudio.Play();
        }
    }
}
