using UnityEngine;

public class CooldownShrink : MonoBehaviour
{
    [SerializeField] private float shrinkSpeed;
    public float shrinkTimer;

    private void Awake()
    {
        transform.GetComponentInParent<PlayerMovement>().isCoolingDown = true;
    }

    // Update is called once per frame
    void Update()
    {
        shrinkTimer += 1 * Time.deltaTime;
        transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(0.1f, 0.1f, 0.1f), shrinkSpeed * Time.deltaTime);  //0.0125f, 0.065f, 0.025f

        if (shrinkTimer >= shrinkSpeed)
        {
            shrinkTimer = shrinkSpeed;
            transform.GetComponentInParent<PlayerMovement>().isCoolingDown = false;
            transform.GetComponentInParent<PlayerMovement>().canStartCharge = true;
        }
        else {
            
            transform.GetComponentInParent<PlayerMovement>().canStartCharge = false; 
        }
    }
}
