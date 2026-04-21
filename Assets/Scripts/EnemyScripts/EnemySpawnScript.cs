using System.Collections;
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


    private void Start()
    {
        for (int i = 0; i < waves.Length; i++)
        {
            waves[i].enemiesLeft = waves[i].enemies.Length;
        }
    }
    void Awake()
    {
        readyToCountDown = true;
        myCollider = GetComponent<Collider>();
        waveCountdown = waves[currentWave].timeToNextWave;        
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
        if(waveCountdown <= 0)
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
            20,
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
                yield return new WaitForSeconds(waves[currentWave].timeToNextEnemy);
            }
        }
    }

    [System.Serializable]
    public class Wave 
    {
        public EnemyScripts[] enemies;
        public float timeToNextWave;
        public float timeToNextEnemy;

        public int enemiesLeft;
    }
}
