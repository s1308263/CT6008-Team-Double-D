using UnityEngine;

public class ShootingScript : MonoBehaviour
{
    [SerializeField] EnemyScripts enemyScript;
    float reload;

    private void Awake()
    {
        enemyScript = GetComponentInParent<EnemyScripts>();
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            reload += Time.deltaTime;
            if (reload >= 1)
            {
                enemyScript.Fire();
                reload = 0;
            }
        }
    }
}
