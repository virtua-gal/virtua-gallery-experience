using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeployChute : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private float groundCheckDistance = 7;
    [SerializeField] private float parachuteDrag = 20;
    [SerializeField] private bool deployed;
    [SerializeField] private DestroyAfterTime destroyAfterTime;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;

        if(!deployed)
        {
            if(Physics.Raycast(transform.position, Vector3.down, out hit, groundCheckDistance))
            {
                if(hit.collider.tag == "Ground")
                {
                    deployed = true;
                    anim.SetTrigger("OpenChute");
                    rb.linearDamping = parachuteDrag;
                }
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Ground" && deployed)
        {
            anim.SetTrigger("CloseChute");
            destroyAfterTime.enabled = true;
        } 
    }
}
