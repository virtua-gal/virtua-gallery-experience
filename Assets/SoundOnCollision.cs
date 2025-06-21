using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundOnCollision : MonoBehaviour
{
    [SerializeField] AudioSource oneShotPlayer;

    [SerializeField] AudioClip clipToPlay;
     
    void OnCollisionEnter(Collision other)
    {
        oneShotPlayer.clip = clipToPlay;
        oneShotPlayer.volume = other.impulse.magnitude;
        oneShotPlayer.Play();
    }
}
