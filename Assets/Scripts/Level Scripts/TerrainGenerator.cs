using UnityEngine;
using System.Collections;
using JetBrains.Annotations;

public class TerrainGenerator : MonoBehaviour {

    public enum DrawMode { NoiseMap, ColourMap, Mesh };
    public DrawMode drawMode;

    public int width;
    public int height;
    public float n_scale;

    public int octaves;
    [Range(0, 1)]
    public float persistance;
    public float lacunarity;

    public int seed;
    public Vector2 offset;

    public float meshHeightMultiplier;

    public bool autoUpdate;

    public TerrainType[] regions;

    public bool canRandColours, randomize;

    private void Awake() {
        if(randomize == true) {
            Randomize();
        }
        else {
            CreateMap();
        }
    }

    public void CreateMap()
    {
        float[,] n_Map = TerrainNoise.GenNoiseMap(width, height, seed, n_scale, octaves, persistance, lacunarity, offset);

        Color[] colourMap = new Color[width * height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float currentHeight = n_Map[x, y];
                for (int i = 0; i < regions.Length; i++)
                {
                    if (currentHeight <= regions[i].height)
                    {
                        colourMap[y * width + x] = regions[i].colour;
                        break;
                    }
                }
            }

            TerrainDisplay display = FindFirstObjectByType<TerrainDisplay>();
            if (drawMode == DrawMode.NoiseMap)
            {
                display.DrawTexture(TextureGenerator.TexFromHeightMap(n_Map));
            }
            else if (drawMode == DrawMode.ColourMap)
            {
                display.DrawTexture(TextureGenerator.TexFromColourMap(colourMap, width, height));
            }
            else if (drawMode == DrawMode.Mesh)
            {
                display.DrawMesh(MeshGenerator.GenerateMesh(n_Map, meshHeightMultiplier), TextureGenerator.TexFromColourMap(colourMap, width, height));
            }
        }

        void OnValidate()
        {
            if (width < 1) { width = 1; }
            if (height < 1) { height = 1; }
            if (lacunarity < 1) { lacunarity = 1; }
            if (octaves < 0) { octaves = 0; }
        }
    }

        public void Randomize() {
        seed = Random.Range(0, 1000);
        meshHeightMultiplier = Random.Range(150, 300);
        offset.x = Random.Range(0, 1000);
        offset.y = Random.Range(0, 1000);
        n_scale = Random.Range(1, 30);
        CreateMap();
    }


    [System.Serializable]
    public struct TerrainType {
        public string name;
        public float height;
        public Color colour;
    }
}