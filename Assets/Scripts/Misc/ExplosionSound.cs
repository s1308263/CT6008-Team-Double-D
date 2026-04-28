using UnityEngine;

public class ExplosionSound : MonoBehaviour {

    [SerializeField] private AudioClip clip;
    [SerializeField] private AudioSource source;

    private void Awake() {
        source = GetComponent<AudioSource>();
        source.PlayOneShot(clip);
        source.pitch = Random.Range(0.3f, 1f);
        source.volume = 0.3f;
    }
}
