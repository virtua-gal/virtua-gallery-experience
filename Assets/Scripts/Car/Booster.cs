using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booster : MonoBehaviour
{
    [field: SerializeField] public float EnergyCostPerSecond { get; private set; } = 10f;

    [SerializeField, Range(0f, 50f)] private float boostStrength = 10f;
    
    float timeToShowBoostEffects = 0.25f;
    float timeLastBoosted = Mathf.Infinity;

    [SerializeField] private ParticleSystem boosterEffect;


    private Rigidbody body;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (timeLastBoosted < timeToShowBoostEffects) {
            if (!boosterEffect.isPlaying) boosterEffect.Play();
            timeLastBoosted += Time.deltaTime;
        } else {
            if (boosterEffect.isPlaying) boosterEffect.Stop();
        }
    }

    public bool TryBoost(int availableEnergy)
    {
        if (availableEnergy <= EnergyCostPerSecond * Time.fixedDeltaTime) return false;

        Boost();
        return true;
    }

    private void Boost()
    {
        Vector3 boostDir = transform.forward.normalized;
        float boostMultiplier = body.mass * boostStrength;
        body.AddForce(boostDir * boostMultiplier, ForceMode.Force);
        timeLastBoosted = 0f;
    }

}
