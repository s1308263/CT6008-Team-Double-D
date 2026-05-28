using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Threading;

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
    List<GameObject> activeEnemies = new List<GameObject>();
    Canvas UI;
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI enemiesLeftText;
    [SerializeField] GameObject warningSign;
    float roundTime = 150;
    public int score;
    bool isLerping;
    Image image;


    [SerializeField] private GameObject canvas;

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
        image = warningSign.GetComponent<Image>();
    }
    void Update()
    {
        if (currentWave >= waves.Length)
        {
            Debug.Log("Finish");
            //End Game
            canvas.GetComponent<Level_Complete>().NextLevel();
            return;
        }
        if (readyToCountDown == true)
        {
            if (!isLerping)
            {
                //WarningUI();
            }
            image.enabled = true;
            waveCountdown -= Time.deltaTime;
        }
        if (waveCountdown <= 0)
        {
            readyToCountDown = false;
            image.enabled = false;
            waveCountdown = waves[currentWave].timeToNextWave;
            StartCoroutine(SpawnWave());
        }
        if (waves[currentWave].enemiesLeft == 0)
        {
            readyToCountDown = true;
            currentWave++;
        }
        if (roundTime > 0)
        {
            roundTime -= Time.deltaTime;
        }
        else
        {
            roundTime = 0;
        }
        scoreText.text = "Score: " + score;
        enemiesLeftText.text = "Enemies Left:" + waves[currentWave].enemiesLeft;
        int minutes = Mathf.FloorToInt(roundTime / 60);
        int seconds = Mathf.FloorToInt(roundTime % 60);
        timerText.text = string.Format("{0:00} : {1:00}", minutes, seconds);
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
            2,
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

    //void WarningUI()
    //{
    //    isLerping = true;
    //    Debug.Log("hey");
    //    if (waveCountdown >= waves[currentWave].timeToNextWave)
    //    {
    //        Debug.Log("LerpOut1");
    //        Image image = warningSign.GetComponent<Image>();
    //        image.enabled = true;

    //        Debug.Log("LerpOut2");
    //        image.enabled = false;
    //    }
    //    else if (waveCountdown <= waves[currentWave].timeToNextWave /2)
    //    {
    //        Debug.Log("LerpIn");
    //        Image image = warningSign.GetComponent<Image>();
    //        image.enabled = true;
    //        image.color = new Color(image.color.r, image.color.g, image.color.b, Mathf.Lerp(0, 1, 200));
    //        image.enabled = false;
    //    }
    //    isLerping = false;
    //}
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
