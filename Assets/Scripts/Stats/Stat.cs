using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat : MonoBehaviour
{
    [SerializeField] protected int maxAmount = 10;
    [SerializeField] protected int startingAmount = 10;
    protected int currentAmount;
    public int Current 
    {
        get
        {
            return currentAmount;
        }
    }

    public event Action OnChange;

    private void Awake()
    {
        currentAmount = startingAmount;
    }

    public virtual void Subtract(int amount)
    {
        currentAmount = Mathf.Max(currentAmount - amount, 0, currentAmount - amount);
        OnChange?.Invoke();
    }

    public virtual void Add(int amount)
    {
        currentAmount = Mathf.Min(currentAmount + amount, maxAmount, currentAmount + amount);
        OnChange?.Invoke();
    }

    public float GetFraction()
    {
        return (float)currentAmount / (float)maxAmount;
    }
}