using TMPro;
using UnityEngine;

public class RescuePickup : MonoBehaviour {

    GameObject player;

    int spawnPos;

    private void Awake() {
        player = GameObject.Find("Player");
        player.GetComponent<Player_Rescue>().totalNeedRescuing++;
        ChangePos();
    }

    private void Update() {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.right, out hit, 10) || Physics.Raycast(transform.position, transform.right / 2, out hit, 10)) {
            if(hit.collider.tag == "Rescue Platform") {
                Debug.Log("Rescue platform collision, moving platform");
                ChangePos();
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            player.GetComponent<Player_Rescue>().rescuePickupInScene = gameObject;
            player.GetComponent<Player_Rescue>().CapacityCheck();
            if (player.GetComponent<PlayerMovement>().dF_Mode == false && transform.GetChild(0).transform.GetComponent<RescueMovement>().canFitOnShip == true) {
                transform.GetChild(0).transform.GetComponent<RescueMovement>().hasLanded = true;
                transform.GetChild(0).transform.GetChild(0).transform.GetComponent<TextMeshPro>().SetText("Thank you!");
            }
            else {
                transform.GetChild(0).transform.GetComponent<RescueMovement>().hasLanded = false;
                transform.GetChild(0).transform.GetChild(0).transform.GetComponent<TextMeshPro>().SetText("There is no room...");
            }
        }
        else if(other.tag == "Rescue Platform") {
            transform.parent.position = new Vector3(Random.Range(-475, 475), 8.5f, 0);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag == "Player" && transform.GetChild(0).gameObject != null && transform.GetChild(0).transform.GetChild(0).gameObject != null || player.GetComponent<PlayerMovement>().dF_Mode == true) {
            transform.GetChild(0).transform.GetComponent<RescueMovement>().hasLanded = false;
            transform.GetChild(0).transform.GetChild(0).transform.GetComponent<TextMeshPro>().SetText("Help!");
        }
    }

    private void ChangePos() {
        spawnPos = Random.Range(0, 2);
        if (spawnPos == 0) {
            transform.parent.position = new Vector3(Random.Range(-475, -60), 8.5f, 0);
        }
        else { transform.parent.position = new Vector3(Random.Range(60, 475), 8.5f, 0); }
    }
}
