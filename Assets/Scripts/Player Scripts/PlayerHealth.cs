using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHealth : MonoBehaviour {

    [SerializeField] private GameObject explosionPrefab, menuManager, heart, shield;

    [SerializeField] private TextMeshProUGUI healthCounter;

    [SerializeField] AudioSource outOfBoundsSource;

    [SerializeField] private float deathTimer, maxDeathTimer, timeSpeed;

    public int currenthealth;
    public int maxHealth;

    GameObject newExplosion;

    bool isDead = false, hasSpawnedExplo1 = false, hasSpawnedExplo2 = false;

    private void Awake() {
        UpdateHealthBar();
    }

    private void OnCollisionEnter(Collision collision) {
        if(collision.collider.tag == "Ground" || collision.collider.tag == "Enemy") {
            InstKill();
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
        if (currenthealth < maxHealth) {
            currenthealth++;
            UpdateHealthBar();
        }
        if(currenthealth > 1) {
            shield.SetActive(true);
        }
    }

    public void RemoveHealth() {
        currenthealth--;
        transform.GetChild(4).gameObject.GetComponent<ParticleSystem>().Play();
        if (currenthealth > 0) {
            //ADD INVINCIBLE FRAMES HERE MAYBE?
        }
        if (currenthealth == 1) {
            //ADD FLASHING OF SOME KIND, MAYBE MATERIAL SWITCH?
            shield.SetActive(false);
        }
        if (currenthealth <= 0) {
            isDead = true;
            SpawnExplosion();
            newExplosion.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }
        UpdateHealthBar();
    }

    public void AddMaxHealth() {
        maxHealth++;
        UpdateHealthBar();
    }

    public void RemoveMaxHealth() {
        maxHealth--;
        UpdateHealthBar();
    }

    public void SpawnExplosion() {
        newExplosion = Instantiate(explosionPrefab);
        newExplosion.transform.position = transform.position;
    }

    public void InstKill() {
        SpawnExplosion();
        newExplosion.transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
        menuManager.SetActive(true);
        Time.timeScale = 0.5f;
        outOfBoundsSource.Stop();
        currenthealth = 0;
        Destroy(gameObject);
    }

    private void UpdateHealthBar() {
        healthCounter.text = currenthealth + "/" + maxHealth;
    }


    public void DEBUG_AddHealth(InputAction.CallbackContext context) {
        if (context.performed == true) {
            AddMaxHealth();
            AddHealth();
        }
    }

    public void DEBUG_RemoveHealth(InputAction.CallbackContext context) {
        if(context.performed == true) {
            RemoveMaxHealth();
            RemoveHealth();
        }
    }
}
