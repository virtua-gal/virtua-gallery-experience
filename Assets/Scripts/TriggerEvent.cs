using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerEvent : MonoBehaviour
{
    [SerializeField] UnityEvent OnTrigger;

    void OnTriggerEnter()
    {
        OnTrigger.Invoke();
    }
}
