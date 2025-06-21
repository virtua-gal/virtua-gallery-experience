using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : Stat
{
    public event Action OnDie;

    public override void Subtract(int amount)
    {
        base.Subtract(amount);

        if (currentAmount <= 0) {
            Die();
        }
    }

    private void Die()
    {
        OnDie?.Invoke();
    }
}