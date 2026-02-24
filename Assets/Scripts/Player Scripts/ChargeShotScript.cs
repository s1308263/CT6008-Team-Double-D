using UnityEngine;

public class ChargeShotScript : MonoBehaviour {

    public float speed, maxSpeed;

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
        if (transform.GetChild(0).GetComponent<ChargeShotTriggerScript>().firstEnemy != null) {
            localEnemy = transform.GetChild(0).GetComponent<ChargeShotTriggerScript>().firstEnemy;
            if (localEnemy != null) {
                transform.LookAt(localEnemy.transform.position);
            }
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Enemy") {
            Debug.Log("CHARGE SHOT DAMAGED: " + collision.gameObject.name);

            //DAMAGE CALC HERE//////////////////////////////////////////////////////////////////////////////////////////
        }
        TrailRenderer tr;
        tr = GetComponent<TrailRenderer>();
        tr.enabled = false;
        Destroy(gameObject);
    }
}
