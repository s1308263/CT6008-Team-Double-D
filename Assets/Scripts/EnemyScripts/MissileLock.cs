using UnityEngine;

public class MissileLock : MonoBehaviour
{
    [SerializeField] float lockTime;
    [SerializeField] EnemyScripts enemyScript;

    private void Awake()
    {
        enemyScript = GetComponentInParent<EnemyScripts>();
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            lockTime += Time.deltaTime;
            if (lockTime >= 10)
            {
                enemyScript.FireMissile();
                lockTime = 0;
            }
        }

    }

}
