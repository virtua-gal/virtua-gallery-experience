using UnityEngine;

public class RobotMover : MonoBehaviour
{
    public Rigidbody rb;
    public float moveSpeed = 1.0f;
    public float rotationSpeed = 1.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Tank controls
        // X input rotates the robot on the Y axis
        // Y input moves the robot back and forth
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.MoveRotation(rb.rotation * Quaternion.AngleAxis(Time.fixedDeltaTime * rotationSpeed * Input.GetAxis("Horizontal"), Vector3.up));
        rb.linearVelocity = rb.transform.forward * Time.fixedDeltaTime * moveSpeed * Input.GetAxis("Vertical");
    }
}
