using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemySpawnScript : MonoBehaviour
{
    Collider myCollider;
    //The timer between waves itself (also for UI)
    public float waveCountdown;
    public int currentWave = 0;
    public Wave[] waves;
    private bool readyToCountDown;
    public GameObject plane;
    public GameObject rd;
    public List<GameObject> activeEnemies = new List<GameObject>();

    private void Start()
    {
        for (int i = 0; i < waves.Length; i++)
        {
            waves[i].enemiesLeft = waves[i].enemies.Length + waves[i].boatEnemies.Length;
        }
    }
    void Awake()
    {
        readyToCountDown = true;
        myCollider = GetComponent<Collider>();
        waveCountdown = waves[currentWave].timeToNextWave;
        plane = GameObject.FindGameObjectWithTag("Player");
    }
    void Update()
    {
        if (currentWave >= waves.Length)
        {
            Debug.Log("Finish");
            //End Game
            return;
        }
        if (readyToCountDown == true)
        {
            waveCountdown -= Time.deltaTime;
        }
        if (waveCountdown <= 0)
        {
            readyToCountDown = false;
            waveCountdown = waves[currentWave].timeToNextWave;
            StartCoroutine(SpawnWave());
        }
        if (waves[currentWave].enemiesLeft == 0)
        {
            readyToCountDown = true;
            currentWave++;
        }
    }
    public static Vector3 SpawnPoint(Bounds bounds)
    {
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            50,
            0);
    }
    public static Vector3 BoatSpawnPoint(Bounds bounds)
    {
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            0,
            0);
    }
    private IEnumerator SpawnWave()
    {

        if (currentWave < waves.Length)
        {
            for (int i = 0; i < waves[currentWave].enemies.Length; i++)
            {
                Vector3 spawnPoint = SpawnPoint(myCollider.bounds);
                GameObject newEnemy = Instantiate(waves[currentWave].enemies[i].gameObject, spawnPoint, Quaternion.identity);
                activeEnemies.Add(newEnemy);
                SpawnIndicator(newEnemy);
                yield return new WaitForSeconds(waves[currentWave].timeToNextEnemy);
            }
            for (int i = 0; i < waves[currentWave].boatEnemies.Length; i++)
            {
                Vector3 spawnPoint = BoatSpawnPoint(myCollider.bounds);
                GameObject newBoatEnemy = Instantiate(waves[currentWave].boatEnemies[i].gameObject, spawnPoint, Quaternion.identity);
                activeEnemies.Add(newBoatEnemy);
                SpawnIndicator(newBoatEnemy);
                yield return new WaitForSeconds(waves[currentWave].timeToNextEnemy);
            }
        }
    }

    void SpawnIndicator(GameObject enemy)
    {
        GameObject newIndicator = Instantiate(rd);

        newIndicator.transform.SetParent(plane.transform, false);
        newIndicator.GetComponent<IndicatorFollow>().player = plane.transform;

        IndicatorFollow follow = newIndicator.GetComponent<IndicatorFollow>();
        follow.target = enemy.transform;
    }
    [System.Serializable]
    public class Wave 
    {
        public EnemyScripts[] enemies;
        public BoatEnemyMove[] boatEnemies;
        public float timeToNextWave;
        public float timeToNextEnemy;

        public int enemiesLeft;
    }
}
