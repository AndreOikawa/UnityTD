using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateFloor : MonoBehaviour
{
    enum Side {
        NORTH,
        SOUTH,
        EAST,
        WEST
    }
    public int tileDimensions = 2;
    public int width = 32;
    public int length = 32;
    public int height = 3;
    public float scale = 1.01f;
    private bool[,,] tiles;
    private Texture tileTop;
    private Texture tileSide;
    void Start()
    {
        PopulateTextures();
        GenerateTiles();

    }
    void PopulateTextures() {
        tileTop = Resources.Load<Texture>("Materials/test1") as Texture;
        tileSide = Resources.Load<Texture>("Materials/test") as Texture;
    }
    void GenerateTiles() {
        CreateMap();
        PaintTiles();
    }

    void CreateMap() {
        tiles = new bool[width, height, length];
        // Vector3 tileScale = tilePrefab.transform.localScale;
        for (int x = 0; x < width; x++) {
            for (int z = 0; z < length; z++) {
                int y = CalculateHeights(x, z);
                // GenerateTop(x,y,z);
                // GenerateSide(x,y,z);
                for (; y >= 0; y--)
                    tiles[x,y,z] = true;
            }
        }
    }
    void PaintTiles() {
        for (int y = 2; y >= 0; y--) {
            for (int x = 0; x < width; x++) {
                for (int z = 0; z < length; z++) {
                    if (tiles[x,y,z]) {
                        GenerateTop(x,y,z);
                        // east
                        if (x > 0 && !tiles[x-1,y,z]) {
                            GenerateSide(x,y,z, Side.EAST);
                        }
                        // west
                        if (x < width - 1 && !tiles[x+1,y,z]) {
                            GenerateSide(x,y,z, Side.WEST);
                        }
                        // north
                        if (z > 0 && !tiles[x,y,z-1]) {
                            GenerateSide(x,y,z, Side.NORTH);
                        }
                        // south
                        if (z < length - 1 && !tiles[x,y,z+1]) {
                            GenerateSide(x,y,z, Side.SOUTH);
                        }
                    }
                    
                }
            }
        }
    }
    void GenerateTop(int x, int y, int z) {
        GameObject quad = GameObject.CreatePrimitive( PrimitiveType.Quad );
                
        quad.GetComponent<Renderer>().material.mainTexture = tileTop; 
        quad.transform.localScale = new Vector3(tileDimensions,tileDimensions,1);
        
        quad.transform.Translate(x * tileDimensions, y * tileDimensions, z * tileDimensions);
        quad.transform.Rotate(90,0,0, Space.Self);
        quad.transform.parent = GameObject.Find("Tiles").transform;
        quad.layer = 8;
    }

    void GenerateSide(int x, int y, int z, Side side) {
        GameObject quad = GameObject.CreatePrimitive( PrimitiveType.Quad );
        
        quad.GetComponent<Renderer>().material.mainTexture = tileSide; 
        quad.transform.localScale = new Vector3(tileDimensions,tileDimensions,1);
        quad.transform.Translate(x * tileDimensions, y * tileDimensions, z * tileDimensions);
        
        quad.GetComponent<MeshCollider>().enabled = false;

        if (side == Side.EAST) {
            quad.transform.Translate(-tileDimensions/2, -tileDimensions/2,0);
            quad.transform.Rotate(0,90,0, Space.Self);
        } else if (side == Side.WEST) {
            quad.transform.Translate(tileDimensions/2,-tileDimensions/2,0);
            quad.transform.Rotate(0,-90,0, Space.Self);
        } else if (side == Side.NORTH) {
            quad.transform.Translate(0,-tileDimensions/2, -tileDimensions/2);
            quad.transform.Rotate(0,0,0, Space.Self);
        } else if (side == Side.SOUTH) {
            quad.transform.Translate(0,-tileDimensions/2,tileDimensions/2);
            quad.transform.Rotate(0,180,0, Space.Self);
        }
        
        quad.transform.parent = GameObject.Find("Tiles").transform;
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
