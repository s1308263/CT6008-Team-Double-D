using Unity.VisualScripting;
using UnityEngine;

public class Level_Complete : MonoBehaviour {

    [SerializeField] private GameObject enemySpawner;

    [SerializeField] private int wavesComplete;

    private void Awake() {
        wavesComplete = 0;
    }

    void Update() {
        WavesCheck();
    }

    void WavesCheck() { 
        if (enemySpawner.GetComponent<EnemySpawnScript>().activeEnemies == null) {
            wavesComplete++;
        }



        //for(int i = 0; i < enemySpawner.GetComponent<EnemySpawnScript>().waves.Length;i++) {

        //}
    }

    void NextLevel() {
        
    }
}
