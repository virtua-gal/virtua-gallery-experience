using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TemperatureController : MonoBehaviour
{
    [SerializeField] private Slider slider;

    public float temperature = 25f;
    public float maxTemp = 45;

    [Range(0f,4f)]
    [SerializeField] float heatSpeed;

    // Update is called once per frame
    void Update()
    {
        if(temperature < maxTemp)
        {
            temperature += Time.deltaTime * heatSpeed;
            slider.value = temperature;
        } 
    }
}
