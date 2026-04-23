using UnityEngine;

public class IndicatorFollow : MonoBehaviour
{
    public Transform target;
    public Transform player;



    private void Awake()
    {

    }
    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 direction = (target.position - player.position).normalized;
        transform.position = player.position + direction * 2f; // distance from player
    }
}
