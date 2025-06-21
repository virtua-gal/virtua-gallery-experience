using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetObject_MattVersion : MonoBehaviour
{
    [SerializeField] private KeyCode restartKey = KeyCode.R;

    private Vector3 startPos;
    private Quaternion startRot;

    private Rigidbody body;

    public BatteryLife batteryLife;
    public GameObject youDiedText;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        SetCheckpoint(transform);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) {
            transform.position = startPos;
            transform.rotation = startRot;

            if (body == null) return;

            body.linearVelocity = Vector3.zero;
            BatteryLife.charge = batteryLife.initialCharge;
            batteryLife.slider.value = BatteryLife.charge;
            youDiedText.SetActive(false);
        }
    }

    public void SetCheckpoint(Transform newCheckpoint)
    {
        startPos = newCheckpoint.position;
        startRot = newCheckpoint.rotation;
    }
}






