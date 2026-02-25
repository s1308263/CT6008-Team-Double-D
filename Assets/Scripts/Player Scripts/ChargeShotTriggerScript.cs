using UnityEngine;

public class ChargeShotTriggerScript : MonoBehaviour {

    public GameObject firstEnemy;
    public bool enemyInTrigger;
    
    bool enemyFound;

    private void OnTriggerEnter(Collider collider) {
        if (collider.tag == "Enemy") {
            enemyInTrigger = true;
            if (enemyFound == false) {
                firstEnemy = collider.gameObject;
                enemyFound = true;
            }
        }
    }

    private void OnTriggerExit(Collider collider) {
        if (collider.tag == "Enemy") {
            enemyInTrigger = false;
        }
    }
}
