using UnityEngine;
using UnityEngine.ProBuilder;

public class AdvancedEnemy : MonoBehaviour
{
    public EnemyStats stats;
    float movementSpeed = 5f;
    int health;
    public GameObject enemySpawner;
    public EnemySpawnScript spawnerScript;
    GameObject player;
    public float smoothing;
    Vector3 velocity = Vector3.zero;
    float holdRadius = 15f;



    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
        enemySpawner = GameObject.FindWithTag("Spawner");
        spawnerScript = enemySpawner.GetComponent<EnemySpawnScript>();
        health = stats.health;

    }
    void Start()
    {
        
    }

    void Update()
    {
        if (player == null) return;

        Vector3 currentPos = transform.position;
        Vector3 targetPos = player.transform.position;

        targetPos.z = currentPos.z;

        float distance = Vector3.Distance(currentPos, targetPos);

        if (distance > holdRadius) {
            Vector3 direction = (targetPos - currentPos).normalized;
            Vector3 desiredPos = targetPos - direction * holdRadius;
            transform.position = Vector3.SmoothDamp(currentPos, desiredPos, ref velocity, smoothing, movementSpeed);
        }
        else
        {
            Vector3 direction = (currentPos - targetPos).normalized;
            Vector3 desiredPos = direction - targetPos;
            transform.position = Vector3.SmoothDamp(currentPos, desiredPos, ref velocity, smoothing, movementSpeed);
        }
        }
}
