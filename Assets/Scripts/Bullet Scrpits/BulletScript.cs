using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class BulletScript : MonoBehaviour {

    [SerializeField] private float speed;
    [SerializeField] private EnemyStats stats;
    GameObject target;
    Rigidbody rb;
    Vector3 aim;
    float timer = 3f;

    private void Awake() {
        target = GameObject.FindWithTag("Player");
        rb = GetComponent<Rigidbody>();
        rb.maxLinearVelocity = 10f;
        aim = target.transform.position - rb.position;
        StartCoroutine(Die());

    }

    void Update() {
        rb.AddForce(aim * stats.speed, ForceMode.VelocityChange);
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.collider.tag == "Player") {
            collision.transform.GetComponent<PlayerHealth>().RemoveHealth();
            Destroy(gameObject);
        }
        if (collision.collider.tag == "Missile")
        {
            Destroy(gameObject);
            Destroy(collision.gameObject);
        }
    }
    IEnumerator Die()
    {
        yield return new WaitForSeconds(timer);
        Destroy(gameObject);
    }
}

