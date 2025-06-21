using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Overheat : MonoBehaviour
{
    private Animator anim;
    [SerializeField] TemperatureController heat;

    private Rigidbody[] ragdollRigidbodies;

    void Awake()
    {
        ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();
        DisableRagdoll();
    }
    
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if(heat.temperature >= 35)
        {
            anim.SetBool("Overheating", true);
        }
        if(heat.temperature >= 45)
        {
            EnableRagdoll();
        }
    }

    private void EnableRagdoll()
    {
        anim.enabled = false;
        foreach(var rigidbody in ragdollRigidbodies)
        {
            rigidbody.isKinematic = false;
        }
    }

    private void DisableRagdoll()
    {
        foreach(var rigidbody in ragdollRigidbodies)
        {
            rigidbody.isKinematic = true;
        }
    }
}
