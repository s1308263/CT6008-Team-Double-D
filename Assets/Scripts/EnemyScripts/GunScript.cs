using System.Collections;
using UnityEngine;

public class GunScript : MonoBehaviour
{
    EnemyScripts enemyScript;
    GameObject bullet;
    GameObject rail;


    void OnAwake()
    {
        enemyScript = GetComponent<EnemyScripts>();
    }
    private void OnTriggerEnter(Collider other)
    {
        enemyScript.rotationSpeed = 5;

    }
    IEnumerator Shoot()
    {
        yield return new WaitForSeconds(2);
        GameObject newBullet = Instantiate(bullet, rail.transform.position, Quaternion.identity);
    }
}
