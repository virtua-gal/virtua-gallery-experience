using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetObject : MonoBehaviour
{
    [SerializeField] private KeyCode restartKey = KeyCode.R;

    private Vector3 startPos;
    private Quaternion startRot;

    private Rigidbody body;

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
        if (Input.GetKeyDown(restartKey)) {
            transform.position = startPos;
            transform.rotation = startRot;

            if (body == null) return;

            body.linearVelocity = Vector3.zero;
        }
    }

    public void SetCheckpoint(Transform newCheckpoint)
    {
        startPos = newCheckpoint.position;
        startRot = newCheckpoint.rotation;
    }
}
