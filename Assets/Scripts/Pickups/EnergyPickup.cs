using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyPickup : Pickup
{
    [SerializeField] private int restoreAmount = 1;

    protected override void PickupEffect(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) {
            if (other.TryGetComponent<Energy>(out Energy energy)) {
                energy.Add(restoreAmount);
            }

            Destroy(gameObject);
        }
    }
}
