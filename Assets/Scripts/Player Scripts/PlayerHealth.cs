using UnityEngine;

public class PlayerHealth : MonoBehaviour {

    public int currenthealth;

    public void AddHealth() {
        currenthealth++;
    }

    public void RemoveHealth() {
        currenthealth--;
        if (currenthealth <= 0) {
            Destroy(gameObject);
        }
    }
}
