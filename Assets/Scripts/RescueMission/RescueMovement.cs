using UnityEngine;

public class RescueMovement : MonoBehaviour {

    [SerializeField] Transform playerTransform;
    [SerializeField] GameObject safeHouse;
    [SerializeField] float speed;

    public bool hasLanded, needsRescuing, isDroppedOff = false, canFitOnShip;

    private void Awake() {
        hasLanded = false;
        playerTransform = GameObject.FindWithTag("Player").transform;
        if (safeHouse == null) {
            safeHouse = GameObject.FindWithTag("SafeHouse");
        }
    }

    private void OnCollisionEnter(Collision collider) {
        if(collider.transform.tag == "Player" && needsRescuing == true && canFitOnShip == true) {
            //Spawn follower here
            collider.transform.GetComponent<Player_Rescue>().AddPassenger();
            if (transform.parent != null) {
                GameObject parent = transform.parent.gameObject;
                needsRescuing = false;
                Destroy(parent);
            }
            Destroy(gameObject);
        }
        else if (collider.transform.tag == "SafeHouse") {
            playerTransform.gameObject.GetComponent<Player_Rescue>().Rescued();
            Destroy(gameObject);
        }
    }

    void Update() {
        if (hasLanded == true && needsRescuing == true && isDroppedOff == false) {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(playerTransform.position.x, playerTransform.position.y, 0), speed);
        }

        else if(isDroppedOff == true) {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(safeHouse.transform.position.x, safeHouse.transform.position.y, 0), speed);
        }
    }
}
