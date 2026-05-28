using UnityEngine;

public class Super_EnegyBomb : MonoBehaviour {

    [SerializeField] private float bombSpeed;
    [SerializeField] private float lerpTimer, maxLerpTimer, timeElapsed;

    // Update is called once per frame
    void Update() {
        lerpTimer += 1 * Time.deltaTime;
        if (lerpTimer < maxLerpTimer) {
            float t = bombSpeed / timeElapsed;
            t = Mathf.Clamp01(t);
            transform.localScale += Vector3.Lerp(new Vector3(0.01f, 0.01f, 0.01f), new Vector3(20, 20, 20), t);
        }
        else if(lerpTimer >= maxLerpTimer)
        {
            lerpTimer = maxLerpTimer;
            Destroy(gameObject);
        }
        timeElapsed += Time.deltaTime;
    }
}
