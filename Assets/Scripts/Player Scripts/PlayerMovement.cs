using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine.UIElements.InputSystem;

public class PlayerMovement : MonoBehaviour {

    [Header("Auto Shooting Properties:")]
    [SerializeField] private Transform bulletSpawn;
    [SerializeField] private float autoFireRate;
    [SerializeField] private float fireLength;
    [SerializeField] private ParticleSystem bulletParticle;
    [SerializeField] private ParticleSystem bulletImpactParticle;
    [SerializeField] private TrailRenderer bulletTrail;

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
    [SerializeField] private float dM_RotSpeed;
    [SerializeField] private float dM_Mass;
    [SerializeField] private float dM_XRotMultiplier;
    [SerializeField] private float dM_YRotMultiplier;

    private Rigidbody rb;

    private Vector2 moveInput;

    private float dM_currentRotX;
    private float dM_currentRotY;

    bool canShoot = true, leftPressed, rightPressed, dF_Mode, autoShoot, isShootingAuto, modeChangePress = false;

    void Awake() {
        rb = GetComponent<Rigidbody>();
        rb.maxLinearVelocity = 3;
    }

    void Start() {
        dM_currentRotX = 0.0f;
        dM_currentRotY = 0.0f;
    }

        // Update is called once per frame
        void Update() {
        //Landing mode movement
        if (dF_Mode == false) {
            //Debug.Log("moveInput = " + moveInput);
            rb.AddForce(moveInput.x * lM_MoveForce, moveInput.y * lM_MoveForce, 0);
            if (leftPressed == true || rightPressed == true) {
                StartCoroutine(LM_RotShip());
            }
        }
        //Dogfight mode movement
        else if (dF_Mode == true) {
            //set handling and acceleration
            float movementHorizontal = Input.GetAxis("Horizontal") * dM_Handling;
            float movementVertical = Input.GetAxis("Vertical") * dM_Acceleration * 2;

            //Add force as thrust
            rb.AddForce(transform.right * movementVertical, ForceMode.Acceleration);

            bool invertXRot = false;
            bool invertYRot = (dM_currentRotX < -180f);

            //Set rotation of ship
            dM_currentRotX = (dM_currentRotX + (invertXRot ? -movementHorizontal : movementHorizontal)) % 360f;
            dM_currentRotY = (dM_currentRotY + (invertYRot ? -movementHorizontal : movementHorizontal)) % 360f;

            Quaternion rotX = Quaternion.AngleAxis(dM_currentRotX, Vector3.right);
            Quaternion rotY = Quaternion.AngleAxis(dM_currentRotY, Vector3.up);

            //apply rotation
            Quaternion rotation = rotX * rotY;
            transform.rotation = rotation;

            if(transform.rotation.z <= 170 || transform.rotation.z >= -170) {
                StartCoroutine(DF_RotShip());
            }
        }
        if (autoShoot == true && isShootingAuto == false) {
            StartCoroutine(AutoFire());
        }
        else { StopCoroutine(AutoFire()); }
    }

    //Move inputs
    public void OnMove(InputAction.CallbackContext context) {
        moveInput = context.ReadValue<Vector2>();
        //rotate player in moving direction
        if (dF_Mode == false) {
            rb.mass = 1;
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
            rb.mass = dM_Mass;
            rb.useGravity = true;
            rb.maxLinearVelocity = dM_MaxVelocity;
            rb.linearDamping = dM_Damping;
            Debug.Log("DF_MODE SETTINGS SET");
        }
    }

    //fire main gun (auto)
    public void OnFire_Auto(InputAction.CallbackContext context) {
        if (context.performed == true) {
            autoShoot = true;
        }
        else {
            autoShoot = false;
        }
    }

    public void OnChangeMode(InputAction.CallbackContext context) {
        switch (context.performed) {
            case true:
                if (modeChangePress == true) {
                    dF_Mode = true;
                    modeChangePress = false;
                }
                else {
                    dF_Mode = false;
                    modeChangePress = true;
                }
                    break;
        }
        
    }

    IEnumerator AutoFire() {
        isShootingAuto = true;
        RaycastHit hit;
        if (Physics.Raycast(bulletSpawn.position, bulletSpawn.right, out hit, fireLength)) {
            Debug.Log("Raycast hit something!");
            Debug.DrawRay(bulletSpawn.position, bulletSpawn.right * fireLength, Color.green, 1);
            TrailRenderer trail = Instantiate(bulletTrail, bulletSpawn.position, Quaternion.identity);
            StartCoroutine(BulletTrail(trail, hit));

            //ADD ENEMY DAMAGE CALC HERE/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            yield return new WaitForSeconds(autoFireRate * hit.distance / fireLength);
        }
        else {
            Debug.DrawRay(bulletSpawn.position, bulletSpawn.right * fireLength, Color.red, 1);
            TrailRenderer trailRend = Instantiate(bulletTrail, bulletSpawn.position, Quaternion.identity);
            float time = 0;
            Vector3 startPos = trailRend.transform.position;
            Vector3 endPos = startPos + (bulletSpawn.right * fireLength);

            while (time < 1) {
                trailRend.transform.position = Vector3.Lerp(startPos, endPos, time);
                time += Time.deltaTime / trailRend.time;
                yield return null;
            }
            trailRend.transform.position = endPos;
            Destroy(trailRend.gameObject, trailRend.time);
        }
        yield return new WaitForSeconds(autoFireRate);
        isShootingAuto = false;
    }

    IEnumerator BulletTrail(TrailRenderer Trail, RaycastHit Hit) {

        float time = 0;
        Vector3 startPos = Trail.transform.position;

        while (time < 1) {
            Trail.transform.position = Vector3.Lerp(startPos, Hit.point, time);
            time += Time.deltaTime / Trail.time;
            yield return null;
        }
        Trail.transform.position = Hit.point;
        Instantiate(bulletImpactParticle, Hit.point, Quaternion.LookRotation(Hit.normal));
        Destroy(Trail.gameObject, Trail.time);
    }

    IEnumerator LM_RotShip() {
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

    IEnumerator DF_RotShip() {
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 180, 0), lM_RotSpeed * Time.deltaTime);
        yield return new WaitForSeconds(dM_RotSpeed);
    }
}
