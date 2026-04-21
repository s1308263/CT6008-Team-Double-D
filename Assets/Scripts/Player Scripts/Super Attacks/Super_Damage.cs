using UnityEngine;

public class Super_Damage : MonoBehaviour {
    private void OnTriggerEnter(Collider other) {
        Debug.Log("SUPER HIT");
        if (other.tag == "Enemy") {
            Debug.Log("SUPER HIT ENEMY");
            other.transform.GetComponent<EnemyScripts>().Damage(1000);
        }
    }
}
