using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

public class BoatEnemyMove : MonoBehaviour
{
    BoatEnemyManager manager;
    float speed = 5f;
    float targetX;
    float fixedY = 0f;
    float fixedZ = 0f;
    public EnemyStats stats;
    public int health;
    public GameObject waveSpawner;
    public EnemySpawnScript waveSpawnerScript;
    public CameraShake cameraShakeScript;
    public GameObject deathParticle;
    GameObject explosion;
    public Slider healthbar;
    private Camera mainCam;

    void Awake()
    {
        mainCam = Camera.main;
        manager = GameObject.FindWithTag("Spawner").GetComponent<BoatEnemyManager>();
        fixedY = transform.position.y;
        fixedZ = transform.position.z;
        MissileLock lockScript = GetComponent<MissileLock>();
        health = stats.boatEnemyHealth;
        waveSpawner = GameObject.FindWithTag("Spawner");
        waveSpawnerScript = waveSpawner.GetComponent<EnemySpawnScript>();
        cameraShakeScript = mainCam.GetComponent<CameraShake>();
        manager.Register(this);
        healthbar.maxValue = stats.health;
        healthbar.value = stats.health;
    }

    void OnDestroy()
    {
        manager.Unregister(this);
    }

    public void SetTargetX(float x)
    {
        targetX = x;
    }

    void FixedUpdate()
    {
        float currentX = transform.position.x;

        float diff = targetX - currentX;

        if (Mathf.Abs(diff) > 0.05f)
        {
            float move = diff;
            move = Mathf.Clamp(move, -1f, 1f);
            currentX += move * speed * Time.deltaTime;
        }
        Die();

        transform.position = new Vector3(currentX, fixedY, fixedZ);
    }
    public void Die()
    {
        if (health <= 0)
        {
            waveSpawnerScript.score += 200;
            waveSpawnerScript.waves[waveSpawnerScript.currentWave].enemiesLeft--;
            explosion = Instantiate(deathParticle);
            explosion.transform.position = transform.position;
            cameraShakeScript.CamShake();
            Destroy(gameObject);
        }
    }
    public void Damage(int damage)
    {
        Debug.Log("BoatHit");
        health -= damage;
        healthbar.value -= damage;
    }
}
