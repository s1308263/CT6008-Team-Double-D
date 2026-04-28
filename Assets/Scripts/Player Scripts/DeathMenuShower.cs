using Unity.VisualScripting;
using UnityEngine;

public class DeathMenuShower : MonoBehaviour {

    [SerializeField] GameObject ingameMenu;

    public float timer;

    private void Awake() {
        timer = 0;
        ingameMenu = GameObject.FindWithTag("IngameMenu");
    }

    private void Update() {
        timer += 1 * Time.deltaTime;
        if (timer >= 0.5f) {
            timer = 0.5f;
            ingameMenu.transform.GetChild(0).transform.gameObject.SetActive(true);
        }
    }
}
