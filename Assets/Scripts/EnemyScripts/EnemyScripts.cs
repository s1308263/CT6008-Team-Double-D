using System.Collections;
using System.Reflection;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UI;
using UnityEngine.VFX;

public class EnemyScripts : MonoBehaviour
{

    [Header("Enemy Stats: ")]
    [SerializeField] public float dashPower = 5;
    [SerializeField] public float dashCD = 1;
    [SerializeField] public float maxSpeed = 10;
    [SerializeField] public float rotationSpeed = 2;

    [Header("Scripts & Dependencies: ")]
    [SerializeField] GameObject missile;
    [SerializeField] GameObject bullet;
    [SerializeField] private EnemyStats stats;
    [SerializeField] private PlayerMovement playerMovementScript;
    [SerializeField] private EnemySpawnScript waveSpawnerScript;
    [SerializeField] private GameObject waveSpawner;
    [SerializeField] private GameObject deathParticle;
    [SerializeField] private Canvas enemyCanvas;
    [SerializeField] private Slider healthbar;

    [Header("Down Time: ")]
    [SerializeField] float downTime = 3;

    GameObject player;
    GameObject explosion;
    Rigidbody rb;
    Quaternion LookRotation;
    Vector3 targetPos;
    bool patrolRunning;
    int health;
    bool canMove = true;
    void Start(){
        player = GameObject.FindWithTag("Player");
        rb = GetComponent<Rigidbody>();
        rb.maxLinearVelocity = maxSpeed;
        MissileLock lockScript = GetComponent<MissileLock>();
        health = stats.health;
        waveSpawner = GameObject.FindWithTag("Spawner");
        waveSpawnerScript = waveSpawner.GetComponent<EnemySpawnScript>();
        playerMovementScript = player.GetComponent<PlayerMovement>();
    }
    void Awake()
    {
        player = GameObject.FindWithTag("Player");
        rb = GetComponent<Rigidbody>();
        rb.maxLinearVelocity = maxSpeed;
        MissileLock lockScript = GetComponent<MissileLock>();
        health = stats.health;
        healthbar.maxValue = stats.health;
        healthbar.value = stats.health;
        waveSpawner = GameObject.FindWithTag("Spawner");
        waveSpawnerScript = waveSpawner.GetComponent<EnemySpawnScript>();
        playerMovementScript = player.GetComponent<PlayerMovement>();
    }
    void FixedUpdate()
    {
        //Spawn asleep
        downTime -= Time.deltaTime;
        if (downTime <= 0)
        {
            //Move forward
            if (canMove == true)
            {
                StartCoroutine(Burst());
            }
            //Taking Damage Script
            Die();

            //Patrol vs Chase Rotation Functions.
            if (player.transform.position.y <= 20 && playerMovementScript.dF_Mode == false)
            {
                patrolRunning = true;
            }
            else
            {
                patrolRunning = false;
            }
            if (patrolRunning == true)
            {
                StopCoroutine(LookAt());
                StartCoroutine(Patrol());
            }
            else if (patrolRunning == false)
            {
                StopCoroutine(Patrol());
                StartCoroutine(LookAt());
            }
        }
    }
    //Destroy on Collision
        void OnCollisionEnter(Collision collision)
        {
            //Destroy Enemy on Impact
            Die();
        }
    //Move Forward (Force)
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
    //Look at player
    IEnumerator LookAt(){
            LookRotation = Quaternion.LookRotation(player.transform.position - transform.position);
            float time = 0;
            while (time < 1f)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, LookRotation, time);
                time += Time.deltaTime * rotationSpeed;
                yield return null;
            }    
        yield return null;
    }
    //Look towards random Location (Patrol)
    IEnumerator Patrol()
    {
        Debug.Log(targetPos.ToString());
        if (targetPos == new Vector3(0,0,0) || transform.position == targetPos)
        {
            targetPos = GetRandomPointAround(transform.position, 2f, 10f);
        }
        LookRotation = Quaternion.LookRotation(targetPos - transform.position);
        float time = 0;
        while(time <1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, LookRotation, time);
            time += Time.deltaTime * rotationSpeed;
        }
        yield return null;
    }
    //Fire a missile
    public void FireMissile()
    {
        Vector3 railPos = transform.position + transform.forward *1.5f;
        GameObject newMissile = Instantiate(missile, railPos, Quaternion.identity);
    }
    //Fire bullets
    public void Fire()
    {
        Vector3 railPos = transform.position + transform.forward *1.5f;
        GameObject newBullet = Instantiate(bullet, railPos, Quaternion.identity);
        newBullet.transform.rotation = LookRotation;
        newBullet.GetComponent<Rigidbody>().AddForce (transform.forward * 100f);
    }
    //Enemy Death
    public void Die()
    {
        if (health <= 0)
        {
            waveSpawnerScript.score += 100;
            waveSpawnerScript.waves[waveSpawnerScript.currentWave].enemiesLeft--;
            explosion = Instantiate(deathParticle);
            explosion.transform.position = transform.position;
            Destroy(gameObject);
        }
    }
    //Taking Damage
    public void Damage(int damage)
    {
        health -= damage;
        healthbar.value -= damage;    
    }
    //Generate Random Location
    Vector3 GetRandomPointAround(Vector3 center, float minRadius, float maxRadius)
    {
        Vector2 direction = Random.insideUnitCircle.normalized;
        float distance = Random.Range(minRadius, maxRadius);

        Vector3 offset = new Vector3(direction.x, direction.y, 0f);
        return center + offset * distance;
    }   
}
