using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {

    [SerializeField] private GameObject explosionPrefab, menuManager, healthAnchor, camera;

    [SerializeField] AudioSource audioSource;
    [SerializeField] private AudioClip playerHitClip;

    [SerializeField] private float deathTimer, maxDeathTimer, timeSpeed, invincibleTimer = 1f, maxInvincibleTimer;

    public int currenthealth;
    public int maxHealth;

    public Sprite emptyHeart;
    public Sprite fullHeart;
    public Image[] hearts;

    [SerializeField] private Material mainMaterial, invincibleMaterial;

    GameObject newExplosion;

    bool isDead = false, hasSpawnedExplo1 = false, hasSpawnedExplo2 = false, hasBeenHit, canDamage;

    private CinemachineImpulseSource impulseSource;

    private void Awake() {
        camera = GameObject.Find("Main Camera");
        audioSource = GetComponent<AudioSource>();
        mainMaterial = transform.GetChild(2).transform.GetComponent<MeshRenderer>().material;
        canDamage = true;
        UpdateHealthBar();
        impulseSource = GetComponent<CinemachineImpulseSource>();
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
                camera.GetComponent<CameraShake>().CineCameraShake(impulseSource);
                newExplosion.transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
                menuManager.SetActive(true);
                Destroy(gameObject);
            }
            else if (deathTimer >= maxDeathTimer / 3 * 2) {
                if (hasSpawnedExplo2 == false) {
                    SpawnExplosion();
                    camera.GetComponent<CameraShake>().CineCameraShake(impulseSource);
                    newExplosion.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                    hasSpawnedExplo2 = true;
                }
            }
            else if (deathTimer >= maxDeathTimer / 3) {
                if (hasSpawnedExplo1 == false)
                {
                    SpawnExplosion();
                    camera.GetComponent<CameraShake>().CineCameraShake(impulseSource);
                    newExplosion.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                    hasSpawnedExplo1 = true;
                }
            }
        }

        for (int i = 0;i < hearts.Length;i++) {
            if(i < currenthealth) {
                hearts[i].sprite = fullHeart;
            }
            else {
                hearts[i].sprite = emptyHeart;
            }
            if (i < maxHealth) {
                hearts[i].enabled = true;
            }
            else {
                hearts[i].enabled = false;
            }
        }

        if(hasBeenHit == true) {
            invincibleTimer += 1 * Time.deltaTime;
            if(invincibleTimer < maxInvincibleTimer) {
                Debug.Log("PLAYER IS INVINCIBLE");
                canDamage = false;
            }
            else {
                invincibleTimer = maxInvincibleTimer;
                Debug.Log("PLAYER IS NOT INVINCIBLE");
                canDamage = true;
                transform.GetChild(2).transform.GetComponent<MeshRenderer>().material = mainMaterial;
            }
        }
    }

    public void AddHealth() {
        if (currenthealth < maxHealth) {
            currenthealth++;
            UpdateHealthBar();
        }
    }

    public void RemoveHealth() {
        if (canDamage == true) {
            currenthealth--;
            camera.GetComponent<CameraShake>().CineCameraShake(impulseSource);
            transform.GetChild(4).gameObject.GetComponent<ParticleSystem>().Play();
            if (currenthealth > 0) {
                invincibleTimer = 0;
                transform.GetChild(2).transform.GetComponent<MeshRenderer>().material = invincibleMaterial;
                hasBeenHit = true;
            }
            if (currenthealth == 1) {
                //ADD smoke/fire coming from player
            }
            if (currenthealth <= 0) {
                isDead = true;
                SpawnExplosion();
                newExplosion.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            }
            audioSource.PlayOneShot(playerHitClip);
            UpdateHealthBar();
        }
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
        audioSource.Stop();
        Destroy(gameObject);
    }

    private void UpdateHealthBar() {
        
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
