using System.Collections;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyScripts : MonoBehaviour
{
    GameObject player;
    float dashPower = 5, 
        dashCD = 1, 
        maxSpeed = 10, 
        rotationSpeed = 3;
    [SerializeField] GameObject missile, bullet;

    Rigidbody rb;
    bool canMove = true;
    void Start(){
        player = GameObject.FindWithTag("Player");
        rb = GetComponent<Rigidbody>();
        rb.maxLinearVelocity = maxSpeed;
        MissileLock lockScript = GetComponent<MissileLock>();
    }
    void Awake()
    {
        player = GameObject.FindWithTag("Player");
        rb = GetComponent<Rigidbody>();
        rb.maxLinearVelocity = maxSpeed;
        MissileLock lockScript = GetComponent<MissileLock>();
    }
        void FixedUpdate()
    {
        //Enemy Movement (Follow Player)
        if (canMove == true) {
            StartCoroutine(Burst());
        }
        //Enemy Rotation (Face Player)
        StartCoroutine(LookAt());       

    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player"){
            //Add damage to player later

            //Destroy Enemy on Impact
            Destroy(gameObject);
        }
    }
    void burst(){
        rb.AddForce(transform.forward * dashPower, ForceMode.Impulse);
    }
    IEnumerator Burst(){
        {
            burst();
            canMove = false;
            yield return new WaitForSeconds(dashCD);
            canMove = true;
        }
    }
    IEnumerator LookAt(){
        Quaternion LookRotation = Quaternion.LookRotation(player.transform.position - transform.position);
        float time = 0;
        while (time < .5f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, LookRotation, time);
            time += Time.deltaTime * rotationSpeed;
            yield return null;
        }
    }
    public void FireMissile()
    {
        Vector3 railPos = transform.position + transform.forward *1.5f;
        GameObject newMissile = Instantiate(missile, railPos, Quaternion.identity);
    }

    public void Fire()
    {
        Vector3 railPos = transform.position + transform.forward *1.5f;
        GameObject newBullet = Instantiate(bullet, railPos, Quaternion.identity);
    }
}
