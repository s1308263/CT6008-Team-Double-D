using UnityEngine;

public class MissileLock : MonoBehaviour
{
    [SerializeField] float lockTime;
    [SerializeField] EnemyScripts enemyScript;
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            lockTime += Time.deltaTime;
            if (lockTime >= 5)
            {
                enemyScript.FireMissile();
                lockTime = 0;
            }
        }

    }

}
