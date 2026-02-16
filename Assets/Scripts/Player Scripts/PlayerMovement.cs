using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawn;

    [SerializeField] private float bulletFireRate, moveForce, maxVelocity, damping, bulletDeath, rotSpeed;

    private Rigidbody rb;

    private Vector2 moveInput;

    bool canShoot = true, isFiring, leftPressed, rightPressed;

    void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update() {
        Debug.Log("moveInput = " + moveInput);
        rb.AddForce(moveInput.x * moveForce, moveInput.y * moveForce, 0);

        if (leftPressed == true || rightPressed == true) {
            StartCoroutine(RotShip());
        }
        //if (rightPressed == true) {
        //    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0,0,0), rotSpeed * Time.deltaTime);
        //}
    }

    //Move inputs
    private void OnMove(InputValue value) {
        moveInput = value.Get<Vector2>();
        //rotate player in moving direction
        if (moveInput.x <= -0.85f) {
            rightPressed = false;
            leftPressed = true;

        }
        else if (moveInput.x >= 0.85f) {
            leftPressed = false;
            rightPressed = true;
        }
        //Sets ship velocity and damping
        if (rb.linearVelocity.magnitude > maxVelocity) {
            rb.maxLinearVelocity = maxVelocity;
            rb.linearDamping = damping;
        }
    }

    //fire main gun (auto)
    void OnFire_Auto(InputValue value) {
        if (value.isPressed && canShoot == true) {
            StartCoroutine(AutoFire());
            canShoot = false;
        }
        else if (canShoot == false) {
            StopCoroutine(AutoFire());
            canShoot = true;
        }
    }

    //Main weapon auto-fire
    IEnumerator AutoFire() {
        if (canShoot == true) {
            var bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
            //Destroy(bullet, bulletDeath);
            yield return new WaitForSeconds(bulletFireRate);
            StartCoroutine(AutoFire());
        }
    }

    IEnumerator RotShip() {
        switch (leftPressed, rightPressed) {
            case (true, false):
            canShoot = false;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 180, 0), rotSpeed * Time.deltaTime);
            yield return new WaitForSeconds(rotSpeed);
            canShoot = true;
            break;
            case (false, true):
            canShoot = false;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, 0), rotSpeed * Time.deltaTime);
            yield return new WaitForSeconds(rotSpeed);
            canShoot = true;
            break;
        }
    }
}
