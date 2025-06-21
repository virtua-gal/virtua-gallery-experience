using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CyberCollect : MonoBehaviour
{
    public int addChargeAmount = 10;

    public BatteryLife batteryLife;

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Battery")
        {
            Destroy(collision.gameObject, 0);
            BatteryLife.charge += addChargeAmount;
            BatteryLife.charge = Mathf.Clamp(BatteryLife.charge, 0, 100);
            batteryLife.slider.value = BatteryLife.charge;
        }
    }
}
