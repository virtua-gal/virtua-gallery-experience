using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventOnCollision : MonoBehaviour
{
    [SerializeField] UnityEvent OnCollision;

    public string tagName;

    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == tagName)
        {
            OnCollision.Invoke();
        } 
    }
}
