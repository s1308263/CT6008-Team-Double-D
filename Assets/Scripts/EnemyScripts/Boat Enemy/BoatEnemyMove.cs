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
    public GameObject deathParticle;
    GameObject explosion;
    public Slider healthbar;

    void Awake()
    {
        manager = GameObject.FindWithTag("Spawner").GetComponent<BoatEnemyManager>();
        fixedY = transform.position.y;
        fixedZ = transform.position.z;
        MissileLock lockScript = GetComponent<MissileLock>();
        health = stats.boatEnemyHealth;
        waveSpawner = GameObject.FindWithTag("Spawner");
        waveSpawnerScript = waveSpawner.GetComponent<EnemySpawnScript>();

        manager.Register(this);
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
}
