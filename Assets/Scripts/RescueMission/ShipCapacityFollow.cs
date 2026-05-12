using UnityEngine;

public class ShipCapacityFollow : MonoBehaviour {

    [SerializeField] private float speed, minSpeed;
    [SerializeField] private int followNumber;
    [SerializeField] private GameObject player;

    void Awake() {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        speed = player.GetComponent<PlayerMovement>().currentSpeed;
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        if (distanceToPlayer > 3f) {
            if (speed > minSpeed) {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(player.transform.position.x, player.transform.position.y, 0), speed * Time.deltaTime);
            }
            else {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(player.transform.position.x, player.transform.position.y, 0), 10 * Time.deltaTime);
            }
        }
        else {
            transform.position = this.transform.position;
        }
    }
}
