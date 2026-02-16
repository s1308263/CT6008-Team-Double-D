using System;
using UnityEngine;

public class LUFTTEST : MonoBehaviour {
    public float acceleration;
    public float handling;

    private float currentRotationX;
    private float currentRotationY;

    private Rigidbody rb;

    void Start()
    {
        currentRotationX = 0.0f;
        currentRotationY = 0.0f;

        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float movementHorizontal = Input.GetAxis("Horizontal") * handling;
        float movementVertical = Input.GetAxis("Vertical") * acceleration;

        rb.AddForce(transform.right * movementVertical, ForceMode.Acceleration);

        bool invertXRotation = false;
        bool invertYRotation = (currentRotationX < -180f);

        currentRotationX = (currentRotationX + (invertXRotation ? -movementHorizontal : movementHorizontal)) % 360f;
        currentRotationY = (currentRotationY + (invertYRotation ? -movementHorizontal : movementHorizontal)) % 360f;

        Quaternion rotationX = Quaternion.AngleAxis(currentRotationX, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(currentRotationY, Vector3.up);

        Quaternion rotation = rotationX * rotationY;
        transform.rotation = rotation;
    }
}
