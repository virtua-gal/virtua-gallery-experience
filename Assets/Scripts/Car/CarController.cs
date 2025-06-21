using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    // [SerializeField] private int maxDamageMultiplier = 3;
    [SerializeField] private int damageMultiplier = 1;
    [SerializeField, Range(0f, 50f)] private float boostStrength = 10f;

    [SerializeField] private WheelController wheelController;

    private const float speedMultiplier = 4;

    private Rigidbody body;
    private Health health;
    private Energy energy;
    private Booster booster;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        health = GetComponent<Health>();
        energy = GetComponent<Energy>();
        booster = GetComponent<Booster>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift)) {
            if (booster.TryBoost(energy.Current)) {
                energy.Subtract(Mathf.RoundToInt(booster.EnergyCostPerSecond * Time.fixedDeltaTime));
            }
        }
    }

    public float GetCurrentSpeed()
    {
        return body.linearVelocity.magnitude * speedMultiplier;
    }

    public float GetCurrentRpm()
    {
        return wheelController.GetFrontRpm();
    }

    private void OnCollisionEnter(Collision other)
    {
        //Damage others
        if (other.gameObject.TryGetComponent<Health>(out Health otherHealth)) {
            int damage = 1 * damageMultiplier;
            otherHealth.Subtract(damage);
        }
    }
}
