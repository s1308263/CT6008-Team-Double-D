using UnityEngine;

public class Super_EnergyWave : MonoBehaviour
{

    public GameObject energyWavePrefab, energyWave;

    public float chargeGrow, maxCharge, waveLifeTimer, maxWaveLifeTime;

    bool hasSpawnedWave = false;

    // Update is called once per frame
    void Update()
    {
        chargeGrow += 1 * Time.deltaTime;
        if (chargeGrow < maxCharge)
        {
            transform.localScale += Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(2,2,2), 1 * Time.deltaTime); //0.65f, 3.4375f, 1.29375f
        }
        else if (chargeGrow >= maxCharge)
        {
            chargeGrow = maxCharge;
            if (hasSpawnedWave == false)
            {
                energyWave = Instantiate(energyWavePrefab, transform);
                energyWave.transform.position = transform.position;
                hasSpawnedWave = true;
            }
            waveLifeTimer += 1 * Time.deltaTime;
            if (waveLifeTimer < maxWaveLifeTime)
            {
                energyWave.transform.localScale += Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(20,0, 0), 1 * Time.deltaTime);
            }
            else if (waveLifeTimer >= maxWaveLifeTime) { 
                waveLifeTimer = maxWaveLifeTime;
                Destroy(gameObject);
            }
        }
    }
}
