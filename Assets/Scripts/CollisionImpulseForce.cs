using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CollisionImpulseForce : MonoBehaviour
{
    public AudioSource hitSound;
    public AudioClip[] hitClips;
    public MeshRenderer[] windowCrack;
    public float hitThreshold = 450;
    public TMP_Text forceText;

    void OnCollisionEnter(Collision collision)
    {
        float collisionForce = collision.impulse.magnitude / Time.fixedDeltaTime;
        forceText.SetText(collisionForce.ToString("0.##"));
        if(this.tag == "Window" && collision.collider.tag == "Projectile")
        {
            if(collisionForce > hitThreshold)
            {
                hitSound.clip = hitClips[Random.Range(0, hitClips.Length)];
                hitSound.Play();
                if(windowCrack[0].enabled == false)
                {
                    windowCrack[0].enabled = true;
                }
                else
                {
                    windowCrack[1].enabled = true;
                }
            }
        } 
    }
}
