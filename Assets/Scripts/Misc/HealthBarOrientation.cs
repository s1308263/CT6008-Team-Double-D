using UnityEngine;

public class HealthBarOrientation : MonoBehaviour
{
    void Update()
    {
        Quaternion lookRotation = Camera.main.transform.rotation;
        transform.rotation = lookRotation;
    }
}
