using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeployParachute : MonoBehaviour
{
    public Animator anim;
    public Rigidbody rb;
    public GameObject chute;
    public GameObject cloth;
    public float parachuteEffectiveness;
    public float deployHeight;
    public bool deployed;

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if(!deployed)
        {
            if(Physics.Raycast(transform.position, Vector3.down, out hit, deployHeight))
            {
                if(hit.collider.tag == "Environment")
                {
                    OpenParachute();
                }
            }
        }
        
    }

    public void OpenParachute()
    {
        deployed = true;
        rb.linearDamping = parachuteEffectiveness;
        anim.SetTrigger("Open Chute");
        deployed = false;
    }

    void OnCollisionEnter()
    {
        // anim.SetTrigger("Close Chute");
        StartCoroutine(CloseChute());
    }

    IEnumerator CloseChute()
    {
        anim.SetTrigger("Chute Close");
        cloth.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        chute.SetActive(false);
    }
}
