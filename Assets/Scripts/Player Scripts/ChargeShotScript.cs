using UnityEngine;

public class ChargeShotScript : MonoBehaviour {

    [SerializeField] private float maxSpeed, life, maxLife, triggerDelay, maxDelay;

    public float speed;

    GameObject localEnemy;
    Rigidbody rb;

    bool fastSpeedSet, slowSpeedSet;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
        rb.maxLinearVelocity = maxSpeed;
        speed = speed * Time.deltaTime;
    }

    // Update is called once per frame
    void Update() {
        rb.AddForce(transform.forward * speed, ForceMode.Acceleration);
        life += 1 * Time.deltaTime;
        triggerDelay += 1 * Time.deltaTime;
        if (triggerDelay >= maxDelay) {
            transform.GetChild(0).transform.gameObject.SetActive(true);
            triggerDelay = maxDelay;
        }
        if (transform.GetChild(0).GetComponent<ChargeShotTriggerScript>().firstEnemy != null) {
            localEnemy = transform.GetChild(0).GetComponent<ChargeShotTriggerScript>().firstEnemy;
            if (localEnemy != null) {
                transform.LookAt(localEnemy.transform.position);
            }
        }
        if(life >= maxLife) {
            TrailRenderer tr;
            tr = GetComponent<TrailRenderer>();
            tr.enabled = false;
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Enemy") {
            Debug.Log("CHARGE SHOT DAMAGED: " + collision.gameObject.name);

            //DAMAGE CALC HERE//////////////////////////////////////////////////////////////////////////////////////////
        }
        if (collision.gameObject.tag != "Player") {
            TrailRenderer tr;
            tr = GetComponent<TrailRenderer>();
            tr.enabled = false;
            Destroy(gameObject);
        }
    }
}
