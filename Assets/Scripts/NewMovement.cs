using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class NewMovement : MonoBehaviour {

    public float acceleration, rotationSpeed;

    bool isThrusting, rightPressed, leftPressed;

    Rigidbody rb;

    Vector2 moveRotate;

    private void Awake() {
        rb = transform.GetComponent<Rigidbody>();
    }

    private void FixedUpdate() {
        if (isThrusting == true) {
            rb.AddForce(transform.right * acceleration * 2, ForceMode.Acceleration);
        }

        if (rightPressed == true) {
            transform.Rotate(-Vector3.forward * +rotationSpeed * Time.deltaTime);
            transform.GetChild(0).transform.Rotate(-Vector3.right * +rotationSpeed * Time.deltaTime);
        }
        else if (leftPressed == true) {
            transform.Rotate(Vector3.forward * +rotationSpeed * Time.deltaTime);
            transform.GetChild(0).transform.Rotate(Vector3.right * +rotationSpeed * Time.deltaTime);
        }
    }

    //Thrust input
    public void OnThrust(InputAction.CallbackContext context) {
        if (context.performed == true) {
            isThrusting = true;
        }
        else {
            isThrusting = false;
        }
    }

    //Movement inputs
    public void OnMove(InputAction.CallbackContext context) {
        if (context.performed == true && context.ReadValue<Vector2>().x < 0 || context.performed == true && context.ReadValue<Vector2>().x > 0) {
            moveRotate = context.ReadValue<Vector2>();
            if (moveRotate.x <= -0.85f) {
                rightPressed = false;
                leftPressed = true;
            }
            else if (moveRotate.x >= 0.85f) {
                leftPressed = false;
                rightPressed = true;
            }
        }
        else { rightPressed = false; leftPressed = false; }
    }
}
