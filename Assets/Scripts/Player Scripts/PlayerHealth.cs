using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHealth : MonoBehaviour {

    [SerializeField] private GameObject explosionPrefab, menuManager;

    [SerializeField] private float deathTimer, maxDeathTimer, timeSpeed;

    public int currenthealth;

    GameObject newExplosion;

    bool isDead = false, hasSpawnedExplo1 = false, hasSpawnedExplo2 = false;

    private void OnCollisionEnter(Collision collision) {
        if(collision.collider.tag == "Ground" || collision.collider.tag == "Enemy") {
            SpawnExplosion();
            newExplosion.transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
            menuManager.SetActive(true);
            Destroy(gameObject);
            Time.timeScale = 0.5f;
        }
    }
    private void Update() {
        if(isDead == true) {
            deathTimer += 1 * Time.deltaTime;
            Time.timeScale = Mathf.Lerp(1, timeSpeed, deathTimer);
            transform.GetComponent<PlayerMovement>().enabled = false;
            if (deathTimer >= maxDeathTimer) {
                deathTimer = maxDeathTimer;
                SpawnExplosion();
                newExplosion.transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
                menuManager.SetActive(true);
                Destroy(gameObject);
            }
            else if (deathTimer >= maxDeathTimer / 3 * 2) {
                if (hasSpawnedExplo2 == false) {
                    SpawnExplosion();
                    newExplosion.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                    hasSpawnedExplo2 = true;
                }
            }
            else if (deathTimer >= maxDeathTimer / 3) {
                if (hasSpawnedExplo1 == false)
                {
                    SpawnExplosion();
                    newExplosion.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                    hasSpawnedExplo1 = true;
                }
            }
        }
    }

    public void AddHealth() {
        currenthealth++;
    }

    public void RemoveHealth() {
        currenthealth--;
        if (currenthealth == 1) {
            transform.GetChild(7).gameObject.GetComponent<ParticleSystem>().Play();
        }
        if (currenthealth <= 0) {
            isDead = true;
            SpawnExplosion();
            newExplosion.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }
    }

    public void SpawnExplosion() {
        newExplosion = Instantiate(explosionPrefab);
        newExplosion.transform.position = transform.position;
    }

    public void DEBUG_AddHealth(InputAction.CallbackContext context) {
        if (context.performed == true) {
            AddHealth();
        }
    }

    public void DEBUG_RemoveHealth(InputAction.CallbackContext context) {
        if(context.performed == true) {
            RemoveHealth();
        }
    }
}
