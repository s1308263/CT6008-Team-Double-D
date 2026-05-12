using System.Collections;
using UnityEngine;

public class BoatShoot : MonoBehaviour
{
    [Header("Turret Balancing")]
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject gun;
    [SerializeField] float bulletSpeed;
    [SerializeField] float reloadTime;
    [SerializeField] float turretRange;
    [SerializeField] float trackingSpeed;


    //Auto Variables
    Transform player;
    Quaternion LookRotation;
    float sqrTurretRange;
    float timer;


    private void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;
        timer = reloadTime;
    }

    private void FixedUpdate()
    {
        StartCoroutine(LookAt());
        RangeTracking();
    }
    void RangeTracking()
    {
        float sqrTurretRange = turretRange * turretRange;
        float sqrDistance = (player.position - transform.position).sqrMagnitude;

        if (sqrDistance < sqrTurretRange)
        {
            timer -= Time.deltaTime;
            if(timer <= 0)
            {
                StartCoroutine(Shoot());
                timer = reloadTime;
            }
        }
    }
    IEnumerator LookAt()
    {
        LookRotation = Quaternion.LookRotation(player.position - transform.position).normalized;
        float time = 0;
        while (time < 1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, LookRotation, time);
            time += Time.deltaTime * trackingSpeed;
            yield return null;
        }
        yield return null;
    }
    IEnumerator Shoot()
    {
        GameObject newBullet = Instantiate(bullet, gun.transform.position, Quaternion.identity);
        newBullet.transform.rotation = LookRotation;
        newBullet.GetComponent<Rigidbody>().AddForce(transform.forward * bulletSpeed);
        yield return null;
    }
}
