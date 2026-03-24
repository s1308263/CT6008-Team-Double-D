using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour {

    [Header("Camera Shake Settings")]
    [SerializeField] private float shakeDuration;
    [SerializeField] private float shakeAmount;
    [SerializeField] private float shakeDamping;

    private Vector3 startPos;

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
