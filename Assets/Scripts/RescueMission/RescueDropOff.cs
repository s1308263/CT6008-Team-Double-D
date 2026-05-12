using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class RescueDropOff : MonoBehaviour {

    [SerializeField] private GameObject victim, rescueVictim, followVictim;
    GameObject player;

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            player = other.gameObject;
            if (player.GetComponent<PlayerMovement>().dF_Mode == false) {
                StartCoroutine(SpawnVictim());
            }
        }
    }
    

    private IEnumerator SpawnVictim() {
        foreach (int rescuedVictim in player.GetComponent<Player_Rescue>().canFitInShip) {
            if (rescuedVictim != 0) {
                victim = Instantiate(rescueVictim);
                victim.transform.position = player.transform.position;
                victim.GetComponent<RescueMovement>().isDroppedOff = true;
                victim.GetComponent<RescueMovement>().needsRescuing = false;
                victim.transform.GetChild(0).transform.GetComponent<TextMeshPro>().SetText(" ");
                victim.transform.GetChild(1).transform.gameObject.SetActive(false);
                player.GetComponent<Player_Rescue>().RemovePassenger();
                yield return new WaitForSeconds(1);
            }
        }
    }
}
