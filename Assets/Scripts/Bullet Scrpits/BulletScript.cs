using UnityEngine;

public class BulletScript : MonoBehaviour {

    [SerializeField] private float speed;

    Rigidbody rb;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update() {
        rb.linearVelocity = transform.right * speed;
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.collider.tag != "Player") {
            Destroy(gameObject);
        }
    }
}

