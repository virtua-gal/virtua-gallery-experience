using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : Pickup
{
    [SerializeField] private int restoreAmount = 1;

    protected override void PickupEffect(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) {
            if (other.TryGetComponent<Health>(out Health health)) {
                health.Add(restoreAmount);
            }

            Destroy(gameObject);
        }
    }
}
