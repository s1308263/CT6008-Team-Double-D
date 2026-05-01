using System.Collections;
using System.Reflection;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UI;
using UnityEngine.VFX;

public class EnemyScripts : MonoBehaviour
{
    GameObject player;
    public Quaternion LookRotation;
    public float dashPower = 5, 
        dashCD = 1, 
        maxSpeed = 10, 
        rotationSpeed = 3;
    [SerializeField] GameObject missile, bullet;
    public EnemyStats stats;
    public int health;
    public GameObject waveSpawner;
    public EnemySpawnScript waveSpawnerScript;
    public Canvas enemyCanvas;
    public Slider healthbar;
    public PlayerMovement playerMovementScript;
    Vector3 targetPos;
    bool playerLost;
    bool ableToMove;
    bool patrolRunning;
    public GameObject deathParticle;
    GameObject explosion;
    float downTime = 3;

    Rigidbody rb;
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
        waveSpawner = GameObject.FindWithTag("Spawner");
        waveSpawnerScript = waveSpawner.GetComponent<EnemySpawnScript>();
        playerMovementScript = player.GetComponent<PlayerMovement>();

    }
        void FixedUpdate()
    {
        downTime -= Time.deltaTime;
        if(downTime <= 0)
        {
            if (canMove == true)
            {
                //Enemy Movement (Follow Player)
                StartCoroutine(Burst());
            }
            if (!patrolRunning)
            {
                //Enemy Rotation (Face Player)
                StartCoroutine(LookAt());
            }
            //Damage?
            Die();
            if (transform.position == targetPos)
            {
                ableToMove = true;
            }
            if (player.transform.position.y <= 20)
            {
                playerLost = true;
            }
        } 
    }
    void OnCollisionEnter(Collision collision)
    {
       
            //Add damage to player later

            //Destroy Enemy on Impact
            Die();
        
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
        if (playerLost && playerMovementScript.dF_Mode == false && ableToMove)
        {
            targetPos = GetRandomPointAround(transform.position, 2f, 5f);
            LookRotation = Quaternion.LookRotation(targetPos - transform.position);
            ableToMove = false;
        }
        else
        {
            LookRotation = Quaternion.LookRotation(player.transform.position - transform.position);
            float time = 0;
            while (time < .5f)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, LookRotation, time);
                time += Time.deltaTime * rotationSpeed;
                yield return null;
            }
        }
        yield return null;
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
        newBullet.transform.rotation = LookRotation;
        newBullet.GetComponent<Rigidbody>().AddForce (transform.forward * 100f);
    }
    public void Die()
    {
        if (health <= 0)
        {
            waveSpawnerScript.waves[waveSpawnerScript.currentWave].enemiesLeft--;
            explosion = Instantiate(deathParticle);
            explosion.transform.position = transform.position;
            Destroy(gameObject);
        }
    }
    public void Damage(int damage)
    {
        health -= damage;
        healthbar.value -= damage;
    }
    Vector3 GetRandomPointAround(Vector3 center, float minRadius, float maxRadius)
    {
        Vector2 direction = Random.insideUnitCircle.normalized;
        float distance = Random.Range(minRadius, maxRadius);

        Vector3 offset = new Vector3(direction.x, 0f, direction.y);
        return center + offset * distance;
    }   
}
