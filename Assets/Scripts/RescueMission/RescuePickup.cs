using TMPro;
using UnityEngine;

public class RescuePickup : MonoBehaviour {

    GameObject player;

    private void Awake() {
        player = GameObject.Find("Player");
        player.GetComponent<Player_Rescue>().totalNeedRescuing++;
        transform.parent.position = new Vector3(Random.Range(-475, 475), 0, 0);
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
            transform.parent.position = new Vector3(Random.Range(-475, 475), 0, 0);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag == "Player" && transform.GetChild(0).gameObject != null && transform.GetChild(0).transform.GetChild(0).gameObject != null || player.GetComponent<PlayerMovement>().dF_Mode == true) {
            transform.GetChild(0).transform.GetComponent<RescueMovement>().hasLanded = false;
            transform.GetChild(0).transform.GetChild(0).transform.GetComponent<TextMeshPro>().SetText("Help!");
        }
    }
}
