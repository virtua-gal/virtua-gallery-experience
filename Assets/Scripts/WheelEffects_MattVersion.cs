using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelEffects_MattVersion : MonoBehaviour
{
    [SerializeField] private TrailRenderer driftTrail;
    [SerializeField] private ParticleSystem smokeParticle;

    [SerializeField] private Rigidbody carBody;
    [SerializeField] private WheelCollider wheelCollider;

    private float driftValue = 1f;

    private void Update()
    {
        driftValue = Vector3.Dot(carBody.linearVelocity.normalized, wheelCollider.transform.forward);

        if (! IsDrifting()) {
            driftTrail.emitting = false;
            
            if(smokeParticle.isPlaying) smokeParticle.Stop();

            return;
        }

        driftTrail.emitting = true;
        if(!smokeParticle.isPlaying) smokeParticle.Play();
    }

    private bool IsDrifting()
    {
        if (! wheelCollider.isGrounded) return false;

        if (carBody.linearVelocity.magnitude <= 0.1f) return false;

        return driftValue <= -0.95f;
    }
}
