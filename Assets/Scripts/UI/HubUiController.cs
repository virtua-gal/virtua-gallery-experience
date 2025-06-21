using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HubUiController : MonoBehaviour
{
    [SerializeField] private CarController car;
    [SerializeField] private TextMeshProUGUI speedText;
    [SerializeField] private TextMeshProUGUI rpmText;

    private void Update()
    {
        speedText.text =  string.Format("{0:0.0}", car.GetCurrentSpeed()) + "<br><size=0.5em>mph</size>";
        rpmText.text =  string.Format("{0:0}", car.GetCurrentRpm()) + "<br><size=0.5em>rpm</size>";
    }
}
