using UnityEngine;

public class IndicatorFollow : MonoBehaviour
{
    public Transform target;
    public Transform player;
    public Camera cam;
    SpriteRenderer sr;
    public float minScale = .25f;
    public float maxScale = 4f;

    public float minDistance = 50f;   // closest distance
    public float maxDistance = 200f;  // farthest distance

    private void Awake()
    {
        if (cam == null)
        {
            cam = Camera.main;
        }
        sr = GetComponent<SpriteRenderer>();
    }
    void FixedUpdate()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 viewPos = cam.WorldToViewportPoint(target.position);
        bool onScreen = viewPos.z > 0 && viewPos.x > 0 && viewPos.x < 1 && viewPos.y > 0 && viewPos.y < 1;
        if (sr != null)
        {
            sr.enabled = !onScreen;
        }
        if (onScreen)
        {
            return;
        }
        Vector3 direction = (target.position - player.position).normalized;
        transform.position = player.position + direction * 3f;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
        float distance = Vector3.Distance(player.position, target.position);

        float t = Mathf.InverseLerp(minDistance, maxDistance, distance);
        float scale = Mathf.Lerp(maxScale, minScale, t);
        transform.localScale = Vector3.one * scale;
    }
}
