using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BatteryLife : MonoBehaviour
{
    public int initialCharge = 100;
    public static int charge;
    public int reduceCharge = 10;
    public Slider slider;

    public GameObject textObject;
    
    // Start is called before the first frame update
    void Start()
    {
        charge = initialCharge;
        slider.value = charge;
        InvokeRepeating("ReduceCharge", 3f, 3f);
    }

    void ReduceCharge()
    {
        charge -= reduceCharge;
        slider.value = charge;
        if(charge <= 0)
        {
            textObject.SetActive(true);
        }
    }
}
