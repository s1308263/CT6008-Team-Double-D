using System.Collections;
using UnityEngine;

public class CamScript : MonoBehaviour {

    [Header("Camera Follow Settings")]
    [SerializeField] private Transform player;
    [SerializeField] private float damping;
    [SerializeField] private Vector3 offset;

    [Header("Camera Shake Settings")]
    [SerializeField] private float shakeDuration;
    [SerializeField] private float shakeAmount;
    [SerializeField] private float shakeDamping;

    private Vector3 velocity;
    private Vector3 camOffset;
    private Vector3 startPos;

    // Update is called once per frame
    void Update() {
        Vector3 destination = player.position + offset + camOffset;
        transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, damping);
        startPos = Vector3.SmoothDamp(transform.position, destination, ref velocity, damping);
    }

    public void CamShake()
    {
        StartCoroutine(Shake());
    }

    private IEnumerator Shake()
    {
        float time = 0f;

        while(time < shakeDuration)
        {
            float amount = shakeAmount * Mathf.Exp(-shakeDamping * time);
            float xOffset = Random.Range(-1f, 1f) * amount;
            float yOffset = Random.Range(-1f, 1f) * amount;

            transform.localPosition = new Vector3(startPos.x + xOffset, startPos.y + yOffset, startPos.z);
            time += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = startPos;
    }
}
