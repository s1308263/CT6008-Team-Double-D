using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine.UIElements.InputSystem;

public class PlayerMovement : MonoBehaviour {

    [Header("General Settings")]
    [SerializeField] private GameObject cam;

    [Header("Auto Shooting Properties:")]
    [SerializeField] private Transform bulletSpawn;
    [SerializeField] private float autoFireTimer;
    [SerializeField] private float maxAutoFireRate;
    [SerializeField] private float fireLength;
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
    [SerializeField] private float maxCooldown;

    public float cooldownTimer;
    public bool canStartCharge, isCoolingDown;

    bool startChargeTimer, canShootFullCharge, canShootMidCharge, canShootSmallCharge, canCooldown;

    [Header("Super Attack Properties:")]
    [SerializeField] private GameObject chargeSpherePrefab;

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
    private Vector2 moveRotate;

    public AudioSource audioSource;
    public AudioClip basicShotClip, ricochetClip, chargeClip, fullChargeClip, chargeReadyClip;

    private float dM_currentRotX;
    private float dM_currentRotY;

    bool leftPressed, rightPressed, dF_Mode, isThrusting, autoShoot, isShootingAuto, canFireSuper = true, dF_rightPressed, dF_leftPressed;

    bool fullChargeIsPlaying = false;

    void Awake() {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        rb.maxLinearVelocity = 3;
        canCooldown = true;
        isCoolingDown = true;
        fullChargeIsPlaying = false;
    }

    void Start() {
        dM_currentRotX = 0.0f;
        dM_currentRotY = 0.0f;
    }

    // Update is called once per frame
    void FixedUpdate() {
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
            //if (dF_rightPressed == true) {
            //    Debug.Log("ROTATING LEFT");
            //    transform.Rotate(-Vector3.forward * +dM_Handling * Time.deltaTime);
            //    transform.GetChild(5).transform.Rotate(-Vector3.right * +dM_Handling * Time.deltaTime, 1, Space.World);
            //}
            //else if (dF_leftPressed == true) {
            //    Debug.Log("ROTATING RIGHT");
            //    transform.Rotate(Vector3.forward * +dM_Handling * Time.deltaTime);
            //    transform.GetChild(5).transform.Rotate(Vector3.right * +dM_Handling * Time.deltaTime, 1, Space.World);
            //}


            //set handling and acceleration
            float movementHorizontal = Input.GetAxis("Horizontal") * dM_Handling;

            bool invertXRot = false;
            bool invertYRot = (dM_currentRotX < -180f);

            if (isThrusting == true) {
                //Add force as thrust
                rb.AddForce(transform.right * dM_Acceleration * 2, ForceMode.Impulse);
            }

            //Set rotation of ship
            dM_currentRotX = (dM_currentRotX + (invertXRot ? -movementHorizontal : movementHorizontal)) % 360f;
            dM_currentRotY = (dM_currentRotY + (invertYRot ? -movementHorizontal : movementHorizontal)) % 360f;

            Quaternion rotX = Quaternion.AngleAxis(dM_currentRotX, Vector3.right);
            Quaternion rotY = Quaternion.AngleAxis(dM_currentRotY, Vector3.up);

            //apply rotation
            Quaternion dF_Rotation = rotX * rotY;
            transform.rotation = dF_Rotation;
        }

        //Auto-fire attack
        if (autoShoot == true && isShootingAuto == false) {
            autoFireTimer += 1 * Time.deltaTime;
            if (autoFireTimer >= maxAutoFireRate) {
                autoFireTimer = 0;
                AutoFire();
            }
        }

        //Charge attack cooldown
        if (canCooldown == true) {
            cooldownTimer += 1 * Time.deltaTime;
            if (cooldownTimer >= maxCooldown) {
                cooldownTimer = maxCooldown;
            }
        }

        //Charge attack
        if (startChargeTimer == true) {
            transform.GetChild(1).transform.gameObject.SetActive(true);
            chargeTimer += 1 * Time.deltaTime;
            Debug.Log("chargeTimer begun");
            //Max charge attack
            if (chargeTimer >= maxChargeTime && chargeTimer >= maxChargeTime / 2) {
                Debug.Log("charge timer at MAX");
                audioSource.Stop();
                transform.GetChild(3).transform.gameObject.SetActive(true);
                canShootMidCharge = false;
                chargeTimer = maxChargeTime;
                canShootFullCharge = true;
                //cam.transform.GetComponent<CameraShake>().CamShake();
            }


            //Mid charge attack
            else if (chargeTimer >= maxChargeTime / 2 && chargeTimer <= maxChargeTime) {
                Debug.Log("Can Shoot MID");
                transform.GetChild(2).transform.gameObject.SetActive(true);
                canShootSmallCharge = false;
                canShootMidCharge = true;
            }

            //Small charge attack
            else if (chargeTimer <= maxChargeTime / 2) {
                Debug.Log("Can Shoot SMALL");
                canShootSmallCharge = true;
            }
        }

        //charge shot cooldown timer
        if (cooldownTimer >= maxCooldown && isCoolingDown == true) {
            cooldownTimer = maxCooldown;
            transform.GetChild(4).gameObject.SetActive(true);
        }
        else if (cooldownTimer == maxCooldown && isCoolingDown == false) {
            transform.GetChild(4).GetComponent<CooldownShrink>().shrinkTimer = 0;
            transform.GetChild(4).gameObject.transform.localScale = new Vector3(1.25f, 6.5f, 2.5f);
            transform.GetChild(4).gameObject.SetActive(false);
        }
    }

    //landing mode movement inputs
    public void OnMove(InputAction.CallbackContext context) {
        moveInput = context.ReadValue<Vector2>();
        //rotate player in moving direction
        if (dF_Mode == false) {
            rb.mass = 1;
            if (moveInput.x <= -0.85f) {
                rightPressed = false;
                leftPressed = true;

            }
            else if (moveInput.x >= 0.85f) {
                leftPressed = false;
                rightPressed = true;
            }
            //Sets ship velocity and damping
            rb.maxLinearVelocity = lM_MaxVelocity;
            rb.linearDamping = lM_Damping;
        }
        //Dogfight mode settings applied
        else if (dF_Mode == true) {
            Debug.Log("IN DF_MODE");
            rb.mass = dM_Mass;
            rb.maxLinearVelocity = dM_MaxVelocity;
            rb.linearDamping = dM_Damping;
            Debug.Log("DF_MODE SETTINGS SET");
        }
    }

    //Dogfight mode movement inputs
    public void OnDM_Rotate(InputAction.CallbackContext context) {
        if (dF_Mode == true) {
            moveRotate = context.ReadValue<Vector2>();
            if (context.performed == true && context.ReadValue<Vector2>().x < 0 || context.performed == true && context.ReadValue<Vector2>().x > 0) {
                moveRotate = context.ReadValue<Vector2>();
                if (moveRotate.x <= -0.85f) {
                    dF_rightPressed = false;
                    dF_leftPressed = true;
                }
                else if (moveRotate.x >= 0.85f) {
                    dF_leftPressed = false;
                    dF_rightPressed = true;
                }
            }
            else { dF_rightPressed = false; dF_leftPressed = false; }
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

    //Fire main gun (auto)
    public void OnFire_Auto(InputAction.CallbackContext context) {
        if (context.performed == true) {
            autoShoot = true;
        }
        else {
            autoShoot = false;
            autoFireTimer = 0;
        }
    }

    //Fire charge shot
    public void OnFire_Charge(InputAction.CallbackContext context) {
        if (context.performed == true && canStartCharge == true && isCoolingDown == false) {
            startChargeTimer = true;
            canCooldown = false;
            audioSource.PlayOneShot(chargeClip);
        }

        else {
            //Fire large charge shot
            if (canShootFullCharge == true && canShootMidCharge == false && canShootSmallCharge == false) {
                audioSource.Stop();
                var newBigChargeshot = Instantiate(chargeShotPrefab_big, bulletSpawn);
                Debug.Log("big charge shot");
                newBigChargeshot.transform.SetParent(null);
                newBigChargeshot.transform.localScale = new Vector3(1, 1, 1);
                newBigChargeshot.transform.GetComponent<ChargeShotScript>().speed = bigShotSpeed;
                canShootFullCharge = false;
            }
            //Fire medium charge shot
            else if (canShootFullCharge == false && canShootMidCharge == true && canShootSmallCharge == false) {
                audioSource.Stop();
                var newMidChargeshot = Instantiate(chargeShotPrefab_mid, bulletSpawn);
                Debug.Log("mid charge shot");
                newMidChargeshot.transform.SetParent(null);
                newMidChargeshot.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                newMidChargeshot.transform.GetComponent<ChargeShotScript>().speed = midShotSpeed;
                canShootMidCharge = false;
            }
            //Fire small charge shot
            else if (canShootFullCharge == false && canShootMidCharge == false && canShootSmallCharge == true) {
                audioSource.Stop();
                var newSmallChargeshot = Instantiate(chargeShotPrefab_small, bulletSpawn);
                Debug.Log("small charge shot");
                newSmallChargeshot.transform.SetParent(null);
                newSmallChargeshot.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
                newSmallChargeshot.transform.GetComponent<ChargeShotScript>().speed = smallShotSpeed;
                canShootSmallCharge = false;
            }
            //reset charge shot settings
            transform.GetChild(1).transform.gameObject.SetActive(false);
            transform.GetChild(2).transform.gameObject.SetActive(false);
            transform.GetChild(3).transform.gameObject.SetActive(false);
            startChargeTimer = false;
            canCooldown = true;
            chargeTimer = 0;
            if (cooldownTimer >= maxCooldown && isCoolingDown == false) {
                cooldownTimer = 0;
            }
        }
    }

    public void OnFire_Super(InputAction.CallbackContext context) {
        if (context.performed == true && canFireSuper == true) {
            GameObject energyWave = Instantiate(chargeSpherePrefab);
            energyWave.transform.SetParent(bulletSpawn, false);
            energyWave.transform.position = bulletSpawn.position;
            canFireSuper = false;
        }
    }

    //Dogfight mode input
    public void OnChangeMode(InputAction.CallbackContext context) {
        switch (context.performed) {
            case true:
            if (dF_Mode == false) {
                dF_Mode = true;
                rb.useGravity = true;
                transform.GetChild(8).GetComponent<TrailRenderer>().emitting = true;
                transform.GetChild(9).GetComponent<TrailRenderer>().emitting = true;
                dM_currentRotX = transform.rotation.x;
                dM_currentRotY = transform.rotation.y;
            }
            else {
                dF_Mode = false;
                rb.useGravity = false;
                transform.GetChild(8).GetComponent<TrailRenderer>().emitting = false;
                transform.GetChild(9).GetComponent<TrailRenderer>().emitting = false;
            }
            break;
        }
    }

    private void AutoFire() {
        audioSource.PlayOneShot(basicShotClip);
        audioSource.volume = 0.2f;
        audioSource.pitch = 1;
        isShootingAuto = true;
        RaycastHit hit;
        if (Physics.Raycast(bulletSpawn.position, bulletSpawn.right, out hit, fireLength)) {
            Debug.Log("Raycast hit something!");
            Debug.DrawRay(bulletSpawn.position, bulletSpawn.right * fireLength, Color.green, 1);
            TrailRenderer trail = Instantiate(bulletTrail, bulletSpawn.position, Quaternion.identity);
            StartCoroutine(BulletTrail(trail, hit));


            //ADD ENEMY DAMAGE CALC HERE////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //if (hit.rigidbody.tag == "Enemy")
            //{
            //    hit.transform.gameObject.GetComponent<EnemyScripts>().Damage(1);
            //}
            //maybe works?????????? - Alex
        }
        else {
            Debug.DrawRay(bulletSpawn.position, bulletSpawn.right * fireLength, Color.red, 1);
            TrailRenderer trailRend = Instantiate(bulletTrail, bulletSpawn.position, Quaternion.identity);
            StartCoroutine(BulletTrail(trailRend, hit));
        }
        isShootingAuto = false;
    }

    IEnumerator BulletTrail(TrailRenderer Trail, RaycastHit Hit) {

        float time = 0;
        Vector3 startPos = Trail.transform.position;

        if (Hit.collider != null) {
            while (time < 1) {
                Trail.transform.position = Vector3.Lerp(startPos, Hit.point, time);
                time += Time.deltaTime / Trail.time;
                yield return null;
            }
            Trail.transform.position = Hit.point;
            Instantiate(bulletImpactParticle, Hit.point, Quaternion.LookRotation(Hit.normal));
            audioSource.PlayOneShot(ricochetClip);
            audioSource.pitch = Random.Range(0.7f, 1.2f);
            audioSource.volume = 0.2f;
            Destroy(Trail.gameObject, Trail.time);
        }

        else {
            Vector3 endPos = startPos + (bulletSpawn.right * fireLength);

            while (time < 1) {
                Trail.transform.position = Vector3.Lerp(startPos, endPos, time);
                time += Time.deltaTime / Trail.time;
                yield return null;
            }
            Trail.transform.position = endPos;
            Destroy(Trail.gameObject, Trail.time);
        }
    }

    IEnumerator LM_RotShip() {
        switch (leftPressed, rightPressed) {
            case (true, false):
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 180, 0), lM_RotSpeed * Time.deltaTime);
            yield return new WaitForSeconds(lM_RotSpeed);
            break;
            case (false, true):
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, 0), lM_RotSpeed * Time.deltaTime);
            yield return new WaitForSeconds(lM_RotSpeed);
            break;
        }
    }
}