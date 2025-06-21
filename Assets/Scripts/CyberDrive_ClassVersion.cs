using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CyberDrive_ClassVersion : MonoBehaviour
{
    [SerializeField] WheelCollider frontRight;
    [SerializeField] WheelCollider frontLeft;
    [SerializeField] WheelCollider backRight;
    [SerializeField] WheelCollider backLeft;

    [SerializeField] Transform frontRightTransform;
    [SerializeField] Transform frontLeftTransform;
    [SerializeField] Transform backRightTransform;
    [SerializeField] Transform backLeftTransform;

    public float acceleration = 500f;
    public float brakeforce = 300f;

    public float maxTurnAngle = 15f;

    private float currentAcceleration = 0f;
    private float currentBrakeforce = 0f;
    private float currentTurnAngle = 0f;

    // Update is called once per frame
    void FixedUpdate()
    {
        currentAcceleration = acceleration * Input.GetAxis("Vertical");

        if(Input.GetKey(KeyCode.Space))
        {
            currentBrakeforce = brakeforce;
        }
        else
        {
            currentBrakeforce = 0f;
        }

        frontRight.motorTorque = currentAcceleration;
        frontLeft.motorTorque = currentAcceleration;

        frontRight.brakeTorque = currentBrakeforce;
        frontLeft.brakeTorque = currentBrakeforce;
        backRight.brakeTorque = currentBrakeforce;
        backLeft.brakeTorque = currentBrakeforce;

        currentTurnAngle = maxTurnAngle * Input.GetAxis("Horizontal");
        frontLeft.steerAngle = currentTurnAngle;
        frontRight.steerAngle = currentTurnAngle;

        UpdateWheel(frontLeft, frontLeftTransform);
        UpdateWheel(frontRight, frontRightTransform);
        UpdateWheel(backLeft, backLeftTransform);
        UpdateWheel(backRight, backRightTransform);

    }

    void UpdateWheel(WheelCollider col, Transform trans)
    {
        Vector3 position;
        Quaternion rotation;
        col.GetWorldPose(out position, out rotation);

        trans.position = position;
        trans.rotation = rotation;
    }
}
