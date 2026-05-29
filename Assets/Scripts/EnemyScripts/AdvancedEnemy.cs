using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.UI;

public class AdvancedEnemy : MonoBehaviour
{
    [Header("Script Dependencies")]
    public EnemyStats stats;
    public EnemySpawnScript spawnerScript;
    public GameObject enemySpawner;

    //Components
    GameObject player;
    Rigidbody rb;
    public GameObject bullet;
    [SerializeField] private GameObject deathParticle;
    [SerializeField] private CameraShake cameraShakeScript;
    [SerializeField] private Canvas enemyCanvas;
    [SerializeField] private Slider healthbar;
    [SerializeField] private Camera mainCam;
    [SerializeField] private Transform visuals;
    GameObject explosion;

    private CinemachineImpulseSource impulseSource;

    //Values
    Vector3 currentVelocity;
    float reload = 1f;
    float fireTimer;
    float holdRadius = 15f;
    float retreatRadius = 10f;
    float movementSpeed = 7f;
    int health;
    bool isDead = false;


    void Awake()
    {
        //Getting all the dependencies
        player = GameObject.FindWithTag("Player");
        enemySpawner = GameObject.FindWithTag("Spawner");
        spawnerScript = enemySpawner.GetComponent<EnemySpawnScript>();
        health = stats.advancedEnemyHealth;
        healthbar.maxValue = stats.advancedEnemyHealth;
        healthbar.value = stats.advancedEnemyHealth;
        rb = GetComponent<Rigidbody>();
        mainCam = Camera.main;
        cameraShakeScript = mainCam.GetComponent<CameraShake>();
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }
    void FixedUpdate()
    {
        if (player == null)
            return;
        Die();

        Vector3 rawDirection = player.transform.position - transform.position;
        rawDirection.z = 0f;
        Vector3 direction = rawDirection.normalized;
        float distance = Vector2.Distance(new Vector2(transform.position.x, transform.position.y), new Vector2(player.transform.position.x, player.transform.position.y));
        Vector3 lookDirection = player.transform.position - transform.position;
        lookDirection.z = 0f;
        visuals.rotation = Quaternion.LookRotation(Vector3.forward, lookDirection);

        //Follow player if too far
        if (distance > holdRadius)
        {
            Move(direction);
        }
        //Move away if too close
        else if (distance < retreatRadius)
        {
            Move(-direction);
            fireTimer -=Time.deltaTime;
            if (fireTimer <= 0f)
            {
                Fire();
                fireTimer = reload;
            }
        }
        //Stop at a distance
        else
        {
            StopMoving();
            fireTimer -=Time.deltaTime;
            if( fireTimer <= 0f)
            {
                Fire();
                fireTimer = reload;
            }
        }
    }
    void Move(Vector3 direction)
    {
        Vector3 targetVelocity = direction * movementSpeed;
        currentVelocity = Vector3.Lerp(currentVelocity, targetVelocity, 10f * Time.fixedDeltaTime);
        rb.linearVelocity = currentVelocity;
    }
    void StopMoving()
    {
        currentVelocity = Vector3.Lerp(currentVelocity, Vector3.zero, 10f * Time.fixedDeltaTime);
        rb.linearVelocity = currentVelocity;
    }
    public void Fire()
    {
        Vector3 railPos = transform.position + visuals.right * 1.5f;
        GameObject newBullet = Instantiate(bullet, railPos, Quaternion.identity);
        newBullet.GetComponent<Rigidbody>().AddForce(transform.right * 100f);
    }
    public void Die()
    {
        if (isDead)
            return;

        if (health <= 0)
        {
            isDead = true;
            spawnerScript.score += 500;
            spawnerScript.waves[spawnerScript.currentWave].enemiesLeft--;
            explosion = Instantiate(deathParticle);
            explosion.transform.position = transform.position;
            if (cameraShakeScript != null && impulseSource != null)
            {
                cameraShakeScript.CineCameraShake(impulseSource);
            }
            GetComponent<Collider>().enabled = false;
            foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
            {
                renderer.enabled = false;
            }
            enemyCanvas.gameObject.SetActive(false);
            rb.linearVelocity = Vector3.zero;
            rb.isKinematic = true;
            Destroy(gameObject, 0.05f);
        }
    }
    public void Damage(int damage)
    {
        health -= damage;
        healthbar.value -= damage;
    }
}
