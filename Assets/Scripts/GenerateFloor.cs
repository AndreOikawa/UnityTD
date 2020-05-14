using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateFloor : MonoBehaviour
{
    public GameObject tilePrefab;
    public int width = 32;
    public int length = 32;
    public int height = 3;
    public float scale = 1.01f;
    private GameObject[,,] tiles;
    void Start()
    {
        tiles = new GameObject[width, height, length];
        Vector3 tileScale = tilePrefab.transform.localScale;
        for (int x = 0; x < width; x++) {
            for (int z = 0; z < length; z++) {
                int y = CalculateHeights(x, z);
                GameObject tile = Instantiate(tilePrefab, new Vector3(x * tileScale.x,  y * tileScale.y, z * tileScale.z), Quaternion.identity);
                tile.transform.parent = GameObject.Find("Tiles").transform;
                tile.layer = 8;
                tiles[x,y,z] = tile;
            }
        }

        
    }
    
    int CalculateHeights(int x, int y)
    {
        float xCoord = (float) x / width * scale;
        float yCoord = (float) y / length * scale;

        return flatten(Mathf.PerlinNoise(xCoord, yCoord));
    }

    int flatten(float height) {
        int temp = ((int)(height * 10));
        int retval = 0;
        if (temp <= 3) retval = 0;
        else if (temp <= 6)  retval = 1;
        else retval = 2;

        return retval;
    }
}
