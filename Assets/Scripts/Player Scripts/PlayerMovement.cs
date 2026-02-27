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

    [Header("Charge Shot Properties:")]
    [SerializeField] private GameObject chargeShotPrefab_big;
    [SerializeField] private GameObject chargeShotPrefab_mid;
    [SerializeField] private GameObject chargeShotPrefab_small;
    [SerializeField] private float chargeTimer;
    [SerializeField] private float maxChargeTime;
    [SerializeField] private float bigShotSpeed;
    [SerializeField] private float midShotSpeed;
    [SerializeField] private float smallShotSpeed;

    [Header("Landing_Mode Settings:")]
    [SerializeField] private float lM_MoveForce;
    [SerializeField] private float lM_MaxVelocity;
    [SerializeField] private float lM_Damping;
    [SerializeField] private float lM_RotSpeed;

    [Header("Dogfight_Mode Settings:")]
    [SerializeField] private float dM_Acceleration;
    [SerializeField] private float dM_Handling;
    [SerializeField] private float dM_MaxVelocity;
    [SerializeField] private float dM_Damping;
    [SerializeField] private float dM_RotSpeed;
    [SerializeField] private float dM_Mass;

    private Rigidbody rb;

    private Vector2 moveInput;

    private float dM_currentRotX;
    private float dM_currentRotY;

    bool canShoot = true, leftPressed, rightPressed, dF_Mode, isThrusting, autoShoot, isShootingAuto, startChargeTimer, canShootFullCharge, canShootMidCharge, canShootSmallCharge;

    void Awake() {
        rb = GetComponent<Rigidbody>();
        rb.maxLinearVelocity = 3;
    }

    void Start() {
        dM_currentRotX = 0.0f;
        dM_currentRotY = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        //Landing mode movement
        if (dF_Mode == false)
        {
            //Debug.Log("moveInput = " + moveInput);
            rb.AddForce(moveInput.x * lM_MoveForce, moveInput.y * lM_MoveForce, 0);
            if (leftPressed == true || rightPressed == true) {
                StartCoroutine(LM_RotShip());
            }
        }
        //Dogfight mode movement
        else if (dF_Mode == true)
        {
            //set handling and acceleration
            float movementHorizontal = Input.GetAxis("Horizontal") * dM_Handling;
            float movementVertical = Input.GetAxis("Vertical") * dM_Acceleration * 2;

            bool invertXRot = false;
            bool invertYRot = (dM_currentRotX < -180f);

            if (isThrusting == true) {
                //Add force as thrust
                rb.AddForce(transform.right * movementVertical, ForceMode.Acceleration);
            }

            //Set rotation of ship
            dM_currentRotX = (dM_currentRotX + (invertXRot ? -movementHorizontal : movementHorizontal)) % 360f;
            dM_currentRotY = (dM_currentRotY + (invertYRot ? -movementHorizontal : movementHorizontal)) % 360f;

            Quaternion rotX = Quaternion.AngleAxis(dM_currentRotX, Vector3.right);
            Quaternion rotY = Quaternion.AngleAxis(dM_currentRotY, Vector3.up);

            //apply rotation
            Quaternion rotation = rotX * rotY;
            transform.rotation = rotation;
        }
        if (autoShoot == true && isShootingAuto == false)
        {
            StartCoroutine(AutoFire());
        }
        else { StopCoroutine(AutoFire()); }

        if (startChargeTimer == true)
        {
            transform.GetChild(1).transform.gameObject.SetActive(true);
            chargeTimer += 1 * Time.deltaTime;
            Debug.Log("chargeTimer begun");
            if (chargeTimer >= maxChargeTime && chargeTimer >= maxChargeTime / 2)
            {
                Debug.Log("timer at max");
                transform.GetChild(3).transform.gameObject.SetActive(true);
                canShootMidCharge = false;
                chargeTimer = maxChargeTime;
                canShootFullCharge = true;
            }
            else if (chargeTimer >= maxChargeTime / 2 && chargeTimer <= maxChargeTime)
            {
                Debug.Log("Can Shoot MID");
                transform.GetChild(2).transform.gameObject.SetActive(true);
                canShootSmallCharge = false;
                canShootMidCharge = true;
            }

            else if (chargeTimer <= maxChargeTime / 2)
            {
                Debug.Log("Can Shoot SMALL");
                canShootSmallCharge = true;
            }
        }
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

    public void OnThrust(InputAction.CallbackContext context) {
        if (context.performed == true) {
            isThrusting = true;
        }
        else {
            isThrusting = false;
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

    public void OnFire_Charge(InputAction.CallbackContext context) {
        if (context.performed == true) {
            startChargeTimer = true;
        }

        else {
            if (canShootFullCharge == true && canShootMidCharge == false && canShootSmallCharge == false)
            {
                var newBigChargeshot = Instantiate(chargeShotPrefab_big, bulletSpawn);
                Debug.Log("Charge shot");
                newBigChargeshot.transform.SetParent(null);
                newBigChargeshot.transform.localScale = new Vector3(1, 1, 1);
                newBigChargeshot.transform.GetComponent<ChargeShotScript>().speed = bigShotSpeed;
                canShootFullCharge = false;
            }

            else if (canShootFullCharge == false && canShootMidCharge == true && canShootSmallCharge == false)
            {
                var newMidChargeshot = Instantiate(chargeShotPrefab_mid, bulletSpawn);
                Debug.Log("Charge shot");
                newMidChargeshot.transform.SetParent(null);
                newMidChargeshot.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                newMidChargeshot.transform.GetComponent<ChargeShotScript>().speed = midShotSpeed;
                canShootMidCharge = false;
            }

            else if (canShootFullCharge == false && canShootMidCharge == false && canShootSmallCharge == true)
            {
                var newSmallChargeshot = Instantiate(chargeShotPrefab_mid, bulletSpawn);
                Debug.Log("Charge shot");
                newSmallChargeshot.transform.SetParent(null);
                newSmallChargeshot.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
                newSmallChargeshot.transform.GetComponent<ChargeShotScript>().speed = midShotSpeed;
                canShootSmallCharge = false;
            }
            transform.GetChild(1).transform.gameObject.SetActive(false);
            transform.GetChild(2).transform.gameObject.SetActive(false);
            transform.GetChild(3).transform.gameObject.SetActive(false);
            startChargeTimer = false;
            chargeTimer = 0;
        }
    }

    public void OnChangeMode(InputAction.CallbackContext context) {
        switch (context.performed) {
            case true:
                dF_Mode = true;
                break;
            case false:
                dF_Mode = false;
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


            //ADD ENEMY DAMAGE CALC HERE
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

    //IEnumerator DF_RotShip() {
    //    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 180, 0), lM_RotSpeed * Time.deltaTime);
    //    yield return new WaitForSeconds(dM_RotSpeed);
    //}
}
