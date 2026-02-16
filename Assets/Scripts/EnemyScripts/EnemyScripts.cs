using System.Collections;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyScripts : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] float strength, cd, maxSpeed, rotationSpeed;
    [SerializeField] Collider range;
    [SerializeField] GameObject missile;
    [SerializeField] Transform rail;

    Rigidbody rb;
    bool canMove = true;
    void Start(){
        player = GameObject.FindWithTag("Player");
        rb = GetComponent<Rigidbody>();
        rb.maxLinearVelocity = maxSpeed;
        MissileLock lockScript = GetComponent<MissileLock>();
    }
    void FixedUpdate()
    {
        //Enemy Movement (Follow Player)
        //transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
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
        rb.AddForce(transform.forward * strength, ForceMode.Impulse);
    }
    IEnumerator Burst(){
        {
            burst();
            canMove = false;
            yield return new WaitForSeconds(cd);
            canMove = true;
        }
    }
    IEnumerator LookAt(){
        Quaternion LookRotation = Quaternion.LookRotation(player.transform.position - transform.position);
        float time = 0;
        while (time < 1)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, LookRotation, time);
            time += Time.deltaTime * rotationSpeed;
            yield return null;
        }
    }
    public void FireMissile()
    {
        GameObject newMissile = Instantiate(missile, rail.transform.position, Quaternion.identity);
    }
}
