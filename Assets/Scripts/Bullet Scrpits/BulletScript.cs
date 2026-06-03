using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class BulletScript : MonoBehaviour {

    [SerializeField] float speed;
    [SerializeField] EnemyStats stats;
    [SerializeField] GameObject deathParticle;
    GameObject explosion;
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
            Impact();
        }
        if (collision.collider.tag == "Missile")
        {
            Impact();
            Destroy(collision.gameObject);
        }
        if (collision.collider.tag == "Enemy" || collision.collider.tag == "BoatEnemy")
        {
            Impact();
        }
    }
    IEnumerator Die()
    {
        yield return new WaitForSeconds(timer);
        explosion = Instantiate(deathParticle);
        explosion.transform.position = transform.position;
        Destroy(gameObject);
    }
    IEnumerator Impact()
    {
        explosion = Instantiate(deathParticle);
        explosion.transform.position = transform.position;
        Destroy(gameObject);
        yield return null;
    }
}

