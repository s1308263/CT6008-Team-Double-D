using UnityEngine;

public class Timer : MonoBehaviour {

    public float timer, maxTimer;
    public GameObject enemySpawner;

    void Update() {
        timer += 1 * Time.deltaTime;
        if (timer > maxTimer) {
            timer = maxTimer;
            enemySpawner.SetActive(true);
            Destroy(gameObject);
        }
    }
}
