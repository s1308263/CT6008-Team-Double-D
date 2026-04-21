using Unity.VisualScripting;
using UnityEngine;

public class DeathMenuShower : MonoBehaviour {

    [SerializeField] GameObject deathMenu;

    public float timer;

    private void Awake() {
        timer = 0;
    }

    private void Update() {
        timer += 1 * Time.deltaTime;
        if (timer >= 0.5f) {
            timer = 0.5f;
            deathMenu.SetActive(true);
        }
    }
}
