using UnityEngine;
using UnityEngine.Rendering;

public class EnemySpawnScript : MonoBehaviour
{
    Collider myCollider;
    Vector3 spawnPoint;
    float spawnTime;
    [SerializeField]GameObject enemy;
    void Awake()
    {
        myCollider = GetComponent<Collider>();
    }


    void Update()
    {
        spawnTime += Time.deltaTime;
        if(spawnTime >= 5)
        {
            SpawnEnemy();
            spawnTime = 0;
        }
    }
    public static Vector3 SpawnPoint(Bounds bounds)
    {
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            0);

    }

    void SpawnEnemy()
    {
        
        Vector3 spawnPoint = SpawnPoint(myCollider.bounds);
        GameObject newEnemy = Instantiate(enemy, spawnPoint, Quaternion.identity);
    }
}
