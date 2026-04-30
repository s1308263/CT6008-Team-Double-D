using UnityEngine;

public class BackgroundScroll : MonoBehaviour {

    public Vector2 offset;

    float timer;

    // Update is called once per frame
    void Update()
    {
        timer += 1 * Time.deltaTime;
        if(timer >= 0.5f)
        {
            offset = transform.GetComponent<TerrainGenerator>().offset;

        }
        offset.x -= 2f * Time.deltaTime;
        offset.y += 2f * Time.deltaTime;
        transform.GetComponent<TerrainGenerator>().offset = offset;
        transform.GetComponent<TerrainGenerator>().randomize = false;
        transform.GetComponent<TerrainGenerator>().CreateMap();
        if(transform.GetComponent<TerrainGenerator>().offset.x <= -40 || transform.GetComponent<TerrainGenerator>().offset.y >= 40)
        {
            offset.x = 0;
            offset.y = 0;
            transform.GetComponent<TerrainGenerator>().offset = offset;
        }
    }
}
