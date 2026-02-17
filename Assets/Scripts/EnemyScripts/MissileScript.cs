using System.Collections;
using UnityEngine;

public class MissileScript : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] float thrust, maxSpeed;
    Rigidbody rb;
    void Awake()
    {
        player = GameObject.FindWithTag("Player");
        rb = GetComponent<Rigidbody>();
        rb.maxLinearVelocity = maxSpeed;
    }
    void FixedUpdate()
    {
        StartCoroutine(Follow());
        StartCoroutine(KillMissile());
        transform.LookAt(player.transform.position);
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //Add damage to player later

            //Destroy Enemy on Impact
            Destroy(gameObject);
        }
    }
    IEnumerator Follow()
    {
        rb.AddForce(transform.forward * thrust, ForceMode.Impulse);
        yield return null;

    }
    IEnumerator KillMissile()
    {
        yield return new WaitForSeconds(7);
        Destroy(gameObject);
    }
    IEnumerator LookAt()
    {
        Quaternion LookRotation = Quaternion.LookRotation(player.transform.position - transform.position);
        float time = 0;
        while (time < 1)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, LookRotation, time);
            rb.AddForce(transform.forward * thrust, ForceMode.Impulse);
            time += Time.deltaTime * 1.5f;
            yield return null;
        }
    }
}
