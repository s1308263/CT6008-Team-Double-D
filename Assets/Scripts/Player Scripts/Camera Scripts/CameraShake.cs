using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class CameraShake : MonoBehaviour {

    public CameraShake instance;

    [SerializeField] private float shakeForce = 1f;
    private void Awake() {
        if(instance == null) {
            instance = this;
        }
    }

    public void CineCameraShake(CinemachineImpulseSource impulseSource) {
        impulseSource.GenerateImpulseWithForce(shakeForce);
    }
}
