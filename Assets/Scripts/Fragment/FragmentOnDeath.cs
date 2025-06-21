using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FragmentOnDeath : MonoBehaviour
{
    private Health health;
    private Fragmentable fragmentable;

    private void Awake()
    {
        health = GetComponent<Health>();
        fragmentable = GetComponent<Fragmentable>();
    }

    private void OnEnable()
    {
        health.OnDie += health_OnDie;
    }

    private void OnDisable()
    {
        health.OnDie -= health_OnDie;
    }

    private void health_OnDie()
    {
        fragmentable.Fragment();
    }

    
}
