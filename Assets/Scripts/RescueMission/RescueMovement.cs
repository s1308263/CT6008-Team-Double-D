using Unity.VisualScripting;
using UnityEngine;

public class RescueMovement : MonoBehaviour {

    [SerializeField] GameObject player, safeHouse, rescueFollow;
    [SerializeField] float speed;

    public GameObject newFollow;

    public bool hasLanded, needsRescuing, isDroppedOff = false, canFitOnShip;

    private void Awake() {
        hasLanded = false;
        player = GameObject.FindWithTag("Player");
        if (safeHouse == null) {
            safeHouse = GameObject.FindWithTag("SafeHouse");
        }
    }

    private void OnCollisionEnter(Collision collider) {
        if(collider.transform.tag == "Player" && needsRescuing == true && canFitOnShip == true) {
            collider.transform.GetComponent<Player_Rescue>().AddPassenger();
            if (transform.parent != null) {
                GameObject parent = transform.parent.gameObject;
                needsRescuing = false;
                Destroy(parent);
            }
            newFollow = Instantiate(rescueFollow);
            newFollow.transform.position = collider.transform.position;
            for(int i = 0; i < player.GetComponent<Player_Rescue>().followArray.Length; i++ ) {
                if (player.GetComponent<Player_Rescue>().followArray[i] == null) {
                    player.GetComponent<Player_Rescue>().followArray[i++] = newFollow;
                }
            }
            Destroy(gameObject);
        }
        else if (collider.transform.tag == "SafeHouse") {
            player.GetComponent<Player_Rescue>().Rescued();
            transform.GetChild(1).gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }

    void Update() {
        if (hasLanded == true && needsRescuing == true && isDroppedOff == false) {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(player.transform.position.x, player.transform.position.y, 0), speed);
        }

        else if(isDroppedOff == true) {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(safeHouse.transform.position.x, safeHouse.transform.position.y, 0), speed);
        }
    }
}
