using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateFloor : MonoBehaviour
{
    
    public int width = 64;
    public int length = 64;
    public int height = 5;
    public float scale = 20f;
    public int nodeSize = 4;

    
    // Start is called before the first frame update
    void Start()
    {
        if (width % nodeSize != 0) width += width % nodeSize;
        if (length % nodeSize != 0) length += length % nodeSize;

        Terrain terrain = GetComponent<Terrain>();
        terrain.terrainData = GenerateTerrain(terrain.terrainData);
    }

    TerrainData GenerateTerrain(TerrainData terrainData) 
    {
        terrainData.heightmapResolution = width + 1;
        terrainData.size = new Vector3(width, height, length);
        terrainData.SetHeights(0,0,GenerateHeights());

        return terrainData;
    }

    float[,] GenerateHeights() 
    {
        float[,] heights = new float[width, length];
        for (int x = 0; x < width; x += nodeSize) 
        {
            for (int y = 0; y < length; y += nodeSize) 
            {
                for (int i = 0; i < nodeSize; i++) 
                {
                    for (int j = 0; j < nodeSize; j++) 
                    {
                        heights[x + i, y + j] = CalculateHeights(x, y);
                    }
                    Debug.Log(heights[x,y]);
                }
                
            }
        }

        return heights;
    }

    float CalculateHeights(int x, int y)
    {
        float xCoord = (float) x / width * scale;
        float yCoord = (float) y / length * scale;

        return (float)((int)(Mathf.PerlinNoise(xCoord, yCoord) * 10)) / 10;
    }
    
}
