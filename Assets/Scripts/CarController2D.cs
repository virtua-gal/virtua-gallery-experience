using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController2D : MonoBehaviour
{
    private JointMotor2D backMotor, frontMotor;

    public WheelJoint2D wheelFront, wheelBack;

    public float forwardSpeed;
    public float reverseSpeed;

    public float torque;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");

        if(x > 0)
        {
            backMotor.motorSpeed = forwardSpeed;
            frontMotor.motorSpeed = forwardSpeed;

            backMotor.maxMotorTorque = torque;
            frontMotor.maxMotorTorque = torque;

            wheelFront.motor = frontMotor;
            wheelBack.motor = backMotor;
        }

        else if(x < 0)
        {
            backMotor.motorSpeed = reverseSpeed;
            frontMotor.motorSpeed = reverseSpeed;

            backMotor.maxMotorTorque = torque;
            frontMotor.maxMotorTorque = torque;

            wheelFront.motor = frontMotor;
            wheelBack.motor = backMotor;
        }
        else
        {
            backMotor.motorSpeed = 0;
            frontMotor.motorSpeed = 0;

            wheelFront.motor = frontMotor;
            wheelBack.motor = backMotor;
        }
    }
}
