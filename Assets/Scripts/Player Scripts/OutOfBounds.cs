using UnityEngine;

public class OutOfBounds : MonoBehaviour {

    public float deathTimer;
    [SerializeField] private float maxDeathCount;

    public AudioSource source;

    private GameObject player;
    bool startTimer = false;

    private void Awake() {
        source = GetComponentInParent<AudioSource>();
        if (player == null) {
            player = GameObject.Find("Player").gameObject;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player") {
            startTimer = true;
            source.Play();
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag == "Player") {
            startTimer = false;
            deathTimer = 0;
            source.Stop();
        }
    }

    private void Update() {
        if (player != null && startTimer == true) {
            deathTimer += 1 * Time.deltaTime;
            if (deathTimer > maxDeathCount) {
                deathTimer = 0;
                player.GetComponent<PlayerHealth>().InstKill();
            }
        }
    }
}
