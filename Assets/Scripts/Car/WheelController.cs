using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelController : MonoBehaviour
{
    [SerializeField] private float acceleration = 500f;
    [SerializeField] private float breakingForce = 300f;
    [SerializeField] private float maxTurnAngle = 15f;

    [SerializeField] private WheelCollider frontRight;
    [SerializeField] private WheelCollider frontLeft;
    [SerializeField] private WheelCollider backRight;
    [SerializeField] private WheelCollider backLeft;

    [SerializeField] private Transform frontRightTransform;
    [SerializeField] private Transform frontLeftTransform;
    [SerializeField] private Transform backRightTransform;
    [SerializeField] private Transform backLeftTransform;

    private float currentAcceleration = 0f;
    private float currentBreakForce = 0f;
    private float currentTurnAngle = 0f;

    private void FixedUpdate()
    {
        //Take care of steering
        Steer();
        
        //Get forward/reverse acceleration from the vertical axis
        Accelerate();

        //If we're pressing space, give currentBreakForce a value
        Brake();

        //Update wheel meshes
        UpdateWheelPoses();
    }

    public float GetFrontRpm()
    {
        return (frontLeft.rpm + frontRight.rpm) / 2.0f;
    }

    public float GetBackRpm()
    {
        return (backLeft.rpm + backRight.rpm) / 2.0f;
    }

    // public float GetFrontFriction()
    // {
    //     return (frontLeft.forwardFriction + frontRight.forwardFriction) / 2.0f;
    // }

    private void Steer()
    {
        currentTurnAngle = maxTurnAngle * Input.GetAxis("Horizontal");
        frontLeft.steerAngle = currentTurnAngle;
        frontRight.steerAngle = currentTurnAngle;
    }

    private void Accelerate()
    {
        currentAcceleration = acceleration * Input.GetAxis("Vertical");

        //Apply acceleration to front wheels
        frontRight.motorTorque = currentAcceleration;
        frontLeft.motorTorque = currentAcceleration;
    }

    private void Brake()
    {
        if (Input.GetKey(KeyCode.Space)) {
            currentBreakForce = breakingForce;
        } else {
            currentBreakForce = 0f;
        }

        frontRight.brakeTorque = currentBreakForce;
        frontLeft.brakeTorque = currentBreakForce;
        backRight.brakeTorque = currentBreakForce;
        backLeft.brakeTorque = currentBreakForce;
    }

    private void UpdateWheelPoses()
    {
        UpdateWheelPoses(frontLeft, frontLeftTransform);
        UpdateWheelPoses(frontRight, frontRightTransform);
        UpdateWheelPoses(backLeft, backLeftTransform);
        UpdateWheelPoses(backRight, backRightTransform);
    }

    private void UpdateWheelPoses(WheelCollider col, Transform trans)
    {
        Vector3 position;
        Quaternion rotation;

        col.GetWorldPose(out position, out rotation);

        //Set wheel transform state
        trans.position = position;
        trans.rotation = rotation;
    }
}
