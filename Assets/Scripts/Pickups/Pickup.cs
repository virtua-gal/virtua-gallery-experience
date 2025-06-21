using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pickup : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PickupEffect(other);
    }

    protected abstract void PickupEffect(Collider other);
}
