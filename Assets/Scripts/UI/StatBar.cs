using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatBar : MonoBehaviour
{
    [SerializeField] private Image amountImage;
    [SerializeField] private Stat stat;

    private void Start()
    {
        UpdateAmount();
    }
    
    private void OnEnable()
    {
        stat.OnChange += UpdateAmount;
    }

    private void OnDisable()
    {
        stat.OnChange -= UpdateAmount;
    }

    private void UpdateAmount()
    {
        amountImage.fillAmount = stat.GetFraction();
    }
}
