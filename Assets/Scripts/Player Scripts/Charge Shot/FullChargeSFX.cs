using UnityEngine;
using UnityEngine.Audio;

public class FullChargeSFX : MonoBehaviour {

    AudioSource audioSource;
    public AudioClip fullChargeClip;

    public float timer = 0;
    public float maxTimer = 1.03f;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0.5f;
        audioSource.PlayOneShot(fullChargeClip);
        timer = 0;
    }

    private void Update() {
        timer += 1 * Time.deltaTime;
        if (timer >= maxTimer) {
            audioSource.PlayOneShot(fullChargeClip);
            timer = 0;
        }
    }
}
