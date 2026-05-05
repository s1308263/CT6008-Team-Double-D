using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class MissileScript : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] float thrust, maxSpeed;
    Rigidbody rb;
    public int health;
    public EnemyStats stats;
    void Awake()
    {
        player = GameObject.FindWithTag("Player");
        rb = GetComponent<Rigidbody>();
        rb.maxLinearVelocity = maxSpeed;
        health = stats.compartmentHealth;
    }
    void FixedUpdate()
    {
        transform.LookAt(player.transform.position);
        StartCoroutine(LookAt());
        StartCoroutine(KillMissile());

    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            stats.compartmentHealth -= stats.missileDamge;
            collision.transform.GetComponent<PlayerHealth>().RemoveHealth();
            //Destroy missile on Impact
            Destroy(gameObject);
        }
    }
    IEnumerator KillMissile()
    {
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }
    IEnumerator LookAt()
    {
        Quaternion LookRotation = Quaternion.LookRotation(player.transform.position - transform.position).normalized;

        float time = 0;
        while (time < 1)
        {
            rb.AddForce(transform.forward * thrust, ForceMode.Impulse);
            transform.rotation = Quaternion.Slerp(transform.rotation, LookRotation, time);
            time += Time.deltaTime * 1f;

            yield return null;
        }
    }
}
