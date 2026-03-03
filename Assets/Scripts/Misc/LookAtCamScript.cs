using UnityEngine;

public class LookAtCamScript : MonoBehaviour {

    [SerializeField] private Camera cam;

    // Update is called once per frame
    void Update() {
        transform.LookAt(cam.transform);
    }
}
