using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateTerrain : MonoBehaviour
{
    
    public int width = 64;
    public int length = 64;
    public int height = 5;
    public float scale = 20f;
    public int nodeSize = 8;

    
    // Start is called before the first frame update
    void Start()
    {
        if (width % nodeSize != 0) width += width % nodeSize;
        if (length % nodeSize != 0) length += length % nodeSize;

        Terrain terrain = GetComponent<Terrain>();
        terrain.terrainData = GenerateTerrainData(terrain.terrainData);
    }

    TerrainData GenerateTerrainData(TerrainData terrainData) 
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

        // smoothen
        for (int x = 1; x < width - 1; x++) {
            for (int y = 1; y < length - 1; y++) {
                float avg = heights[x,y];
                int diff = 1;
                // right 
                if (heights[x,y] != heights[x+1,y]) {
                    avg += heights[x+1,y];
                    diff++;
                }
                // left
                if (heights[x,y] != heights[x-1,y]) {
                    avg += heights[x-1,y];
                    diff++;
                }
                // up
                if (heights[x,y] != heights[x,y+1]) {
                    avg += heights[x,y+1];
                    diff++;
                }
                // down
                if (heights[x,y] != heights[x,y-1]) {
                    avg += heights[x,y-1];
                    diff++;
                }
                heights[x,y] = avg/diff;
            }
        }

        return heights;
    }

    float CalculateHeights(int x, int y)
    {
        float xCoord = (float) x / width * scale;
        float yCoord = (float) y / length * scale;

        return flatten(Mathf.PerlinNoise(xCoord, yCoord));
    }

    float flatten(float height) {
        int temp = ((int)(height * 10));
        float retval = 0;
        if (temp <= 3) retval = 0;
        else if (temp <= 6)  retval = 0.5f;
        else retval = 1.0f;

        return retval;
    }
    
}
