using UnityEngine;

public class CamScript : MonoBehaviour {

    [SerializeField] private Transform player;
    [SerializeField] private float damping;
    [SerializeField] private Vector3 offset;

    private Vector3 velocity;
    private Vector3 camOffset;

    // Update is called once per frame
    void Update() {
        Vector3 destination = player.position + offset + camOffset;
        transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, damping);
    }
}
