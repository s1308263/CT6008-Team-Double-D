using UnityEngine;

public class CooldownShrink : MonoBehaviour
{
    [SerializeField] private float shrinkSpeed;
    [SerializeField] private float shrinkTimer;

    Vector3 originScale;

    void Awake()
    {
        //originScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        shrinkTimer += 1 * Time.deltaTime;
        transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(0.0125f, 0.065f, 0.025f), shrinkSpeed * Time.deltaTime);

        if (shrinkTimer >= shrinkSpeed)
        {
            shrinkTimer = shrinkSpeed;
        }
    }
}
