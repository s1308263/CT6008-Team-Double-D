using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {

    [Header("Shooting Properties:")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawn;
    [SerializeField] private float bulletFireRate;
    [SerializeField] private float bulletDeath;

    [Header("Landing_Mode Settings:")]
    [SerializeField] private float lM_MoveForce;
    [SerializeField] private float lM_MaxVelocity;
    [SerializeField] private float lM_Damping;
    [SerializeField] private float lM_RotSpeed;

    [Header("DF_Mode Settings:")]
    [SerializeField] private float dM_Acceleration;
    [SerializeField] private float dM_Handling;
    [SerializeField] private float dM_MaxVelocity;
    [SerializeField] private float dM_Damping;

    private Rigidbody rb;

    private Vector2 moveInput;

    private float dM_currentRotX;
    private float dM_currentRotY;

    bool canShoot = true, leftPressed, rightPressed, dF_Mode;

    void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    void Start() {
        dM_currentRotX = 0.0f;
        dM_currentRotY = 0.0f;
    }

        // Update is called once per frame
        void Update() {

        if (dF_Mode == false) {
            Debug.Log("moveInput = " + moveInput);
            rb.AddForce(moveInput.x * lM_MoveForce, moveInput.y * lM_MoveForce, 0);

            if (leftPressed == true || rightPressed == true) {
                StartCoroutine(RotShip());
            }
        }
        else if(dF_Mode == true)
        {
            float movementHorizontal = Input.GetAxis("Horizontal") * dM_Handling;
            float movementVertical = Input.GetAxis("Vertical") * dM_Acceleration * 2;

            rb.AddForce(transform.right * movementVertical, ForceMode.Acceleration);

            bool invertXRot = false;
            bool invertYRot = (dM_currentRotX < -180f);

            dM_currentRotX = (dM_currentRotX + (invertXRot ? -movementHorizontal : movementHorizontal)) % 360f;
            dM_currentRotY = (dM_currentRotY + (invertYRot ? -movementHorizontal : movementHorizontal)) % 360f;

            Quaternion rotX = Quaternion.AngleAxis(dM_currentRotX, Vector3.right);
            Quaternion rotY = Quaternion.AngleAxis(dM_currentRotY, Vector3.forward);

            Quaternion rotation = rotX * rotY;
            transform.rotation = rotation;
        }
    }

    //Move inputs
    private void OnMove(InputValue value) {
        moveInput = value.Get<Vector2>();
        //rotate player in moving direction
        if (dF_Mode == false) {
            rb.useGravity = false;
            if (moveInput.x <= -0.85f) {
                rightPressed = false;
                leftPressed = true;

            }
            else if (moveInput.x >= 0.85f) {
                leftPressed = false;
                rightPressed = true;
            }
            //Sets ship velocity and damping
            if (rb.linearVelocity.magnitude > lM_MaxVelocity) {
                rb.maxLinearVelocity = lM_MaxVelocity;
                rb.linearDamping = lM_Damping;
            }
        }
        else if (dF_Mode == true)
        {
            Debug.Log("IN DF_MODE");
            rb.useGravity = true;
            rb.maxLinearVelocity = dM_MaxVelocity;
            rb.linearDamping = dM_Damping;
            //lM_MoveForce = 10;
            Debug.Log("DF_MODE SETTINGS SET");
        }
    }

    //fire main gun (auto)
    //void OnFire_Auto(InputValue value) {
    //    if (value.isPressed && canShoot == true) {
    //        StartCoroutine(AutoFire());
    //        canShoot = false;
    //    }
    //    else if (canShoot == false) {
    //        StopCoroutine(AutoFire());
    //        canShoot = true;
    //    }
    //}

    void OnChangeMode(InputValue value) {
        if (value.isPressed && dF_Mode == false) {
            Debug.Log("Mode Changed to DOGFIGHTING");
            dF_Mode = true;
            Debug.Log("DOGFIGHT MODE = true");
        }

        else {
            Debug.Log("Mode Changed to LANDING");
            dF_Mode = false;
            Debug.Log("DOGFIGHT MODE = false");
        }
    }

    //Main weapon auto-fire
    //IEnumerator AutoFire() {
        //if (canShoot == true) {
        //    var bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
        //    //Destroy(bullet, bulletDeath);
        //    yield return new WaitForSeconds(bulletFireRate);
        //    StartCoroutine(AutoFire());
        //}
    //    yield return null;
    //}

    IEnumerator RotShip() {
        switch (leftPressed, rightPressed) {
            case (true, false):
            canShoot = false;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 180, 0), lM_RotSpeed * Time.deltaTime);
            yield return new WaitForSeconds(lM_RotSpeed);
            canShoot = true;
            break;
            case (false, true):
            canShoot = false;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, 0), lM_RotSpeed * Time.deltaTime);
            yield return new WaitForSeconds(lM_RotSpeed);
            canShoot = true;
            break;
        }
    }
}
