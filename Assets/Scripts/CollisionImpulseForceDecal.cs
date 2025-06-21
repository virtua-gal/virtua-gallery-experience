using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CollisionImpulseForceDecal : MonoBehaviour
{
    public AudioSource hitSound;
    public AudioClip[] hitClips;
    public MeshRenderer windowCrack;
    public float hitThreshold = 450;
    public TMP_Text forceText;
    // public Transform shatterPrefab;

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        float collisionForce = collision.impulse.magnitude / Time.fixedDeltaTime;
        Debug.Log(collisionForce);
        forceText.SetText(collisionForce.ToString("0.##"));
        if(this.tag == "Window" && collision.collider.tag == "Projectile")
        {
            
            if(collisionForce > hitThreshold)
            {
                hitSound.clip = hitClips[Random.Range(0, hitClips.Length)];
                hitSound.Play();
                windowCrack.enabled = true;
            }
            // ContactPoint contact = collision.contacts[0];
            // Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
            // Vector3 pos = contact.point;
            // Instantiate(shatterPrefab, pos, rot);
        }
        
    }
}