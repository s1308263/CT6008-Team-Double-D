using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class BulletScript : MonoBehaviour {

    [SerializeField] private float speed;
    GameObject target;
    Rigidbody rb;
    Vector3 aim;

    private void Awake() {
        target = GameObject.FindWithTag("Player");
        rb = GetComponent<Rigidbody>();
        rb.maxLinearVelocity = 5f;
        aim = target.transform.position - rb.position;


    }

    void Update() {
        rb.AddForce(aim * speed, ForceMode.VelocityChange);
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.collider.tag == "Player") {
            Destroy(gameObject);
        }if (collision.collider.tag == "Missile")
        {
            Destroy(gameObject);
            Destroy(collision.gameObject);
        }
    }
}

