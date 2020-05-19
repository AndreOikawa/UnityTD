using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateFloor : MonoBehaviour
{
    enum Top {
        NOTHING,
        PATH,
        GROUND,
        GENERATED
    }
    enum Side {
        NORTH,
        SOUTH,
        EAST,
        WEST, 
        GENERATED
    }

    #region public vars
    public int tileDimensions = 2;
    public int width = 32;
    public int length = 32;
    public int height = 3;
    public float scale = 1.01f;
    public bool debug = false;
    public GameObject waypointPrefab;
    #endregion
    
    #region private vars
    private Top[,,] tiles;
    private Texture tileTop;
    private Texture tileSide;
    private Texture tilePath;
    #endregion
    void Awake()
    {
        PopulateTextures();
        GenerateTiles();
        GenerateBoundary();
    }
    void GenerateBoundary() {
        GameObject quad = GameObject.CreatePrimitive( PrimitiveType.Quad );
        quad.isStatic = true;
        // quad.GetComponent<Renderer>().material.mainTexture = texture; 
        quad.transform.localScale = new Vector3(width * tileDimensions, length * tileDimensions,1);

        Vector3 pos = new Vector3(width, -1, length);
        Vector3 rotation = new Vector3(90,0,0);
        quad.transform.Translate(pos);
        quad.transform.Rotate(rotation, Space.Self);
        quad.transform.parent = GameObject.Find("Boundary").transform;

    }
    void PopulateTextures() {
        tileTop = Resources.Load<Texture>("Materials/GroundTop") as Texture;
        tileSide = Resources.Load<Texture>("Materials/GroundSide") as Texture;
        tilePath = Resources.Load<Texture>("Materials/GroudPath") as Texture;
    }
    void GenerateTiles() {
        CreateMap();
        PaintTiles();
    }

    void CreateMap() {
        tiles = new Top[width, height, length];
        // Vector3 tileScale = tilePrefab.transform.localScale;
        for (int x = 0; x < width; x++) {
            for (int z = 0; z < length; z++) {
                int y = CalculateHeights(x, z);
                if (y == height) y--;
                // for (; y >= 0; y--)
                    tiles[x,y,z] = Top.GROUND;
            }
        }

        CreatePath();
    }

    void CreatePath() {
        int xStart = 1;
        int zStart = 1;
        int xEnd = width - 2;
        int zEnd = length - 2;

        int prevY = -1;
        bool first = true;
        for (int x = xStart; x <= xEnd; x++) {
            int y = HighestTop(x,zStart);
            if (prevY != y) {
                
                if (!first) {
                    var prev = new Vector3(x-1,prevY+0.5f,zStart);
                    CreateWaypoint(prev * tileDimensions);
                    var pos = new Vector3(x,y + 0.5f,zStart);
                    CreateWaypoint(pos * tileDimensions);
                }
                prevY = y;
            }
            first = false;
            tiles[x,y,zStart] = Top.PATH;
        }

        first = true;
        for (int z = zStart; z <= zEnd; z++) {
            int y = HighestTop(xEnd,z);
            if (prevY != y) {
                
                if (!first) {
                    var prev = new Vector3(xEnd,prevY+0.5f,z-1);
                    CreateWaypoint(prev * tileDimensions);
                    var pos = new Vector3(xEnd,y+0.5f,z);
                    CreateWaypoint(pos * tileDimensions);
                
                }
                prevY = y;
            }
            first = false;
            tiles[xEnd,y,z] = Top.PATH;
        }

        var last = new Vector3(xEnd, HighestTop(xEnd, zEnd) + 0.5f, zEnd);
        CreateWaypoint(last * tileDimensions);
    }

    void CreateWaypoint(Vector3 pos) {
        var waypoint = Instantiate(waypointPrefab, pos, Quaternion.identity);
        waypoint.transform.parent = GameObject.Find("Waypoints").transform;
        Waypoint.AddWaypoint(waypoint.transform);
    }
    int HighestTop(int x, int z) {
        int y = height - 1;
        for (; y >= 0; y--) {
            if (tiles[x,y,z] != Top.NOTHING) {
                if (debug) Debug.Log(y);
                return y;
            }
        }
        return y;
    }

    void PaintTiles() {
        for (int y = height - 1; y >= 0; y--) {
            for (int x = 0; x < width; x++) {
                for (int z = 0; z < length; z++) {
                    
                    if (tiles[x,y,z] != Top.NOTHING) {
                        GenerateTop(x,y,z, tiles[x,y,z]);
                        // east
                        if (x > 0 && tiles[x-1,y,z] == Top.NOTHING) {
                            GenerateSide(x,y,z, Side.EAST);
                        }
                        // west
                        if (x < width - 1 && tiles[x+1,y,z] == Top.NOTHING) {
                            GenerateSide(x,y,z, Side.WEST);
                        }
                        // north
                        if (z > 0 && tiles[x,y,z-1] == Top.NOTHING) {
                            GenerateSide(x,y,z, Side.NORTH);
                        }
                        // south
                        if (z < length - 1 && tiles[x,y,z+1] == Top.NOTHING) {
                            GenerateSide(x,y,z, Side.SOUTH);
                        }
                    }
                    
                }
            }
        }
    }
    void GenerateTop(int x, int y, int z, Top type) {
        Vector3 newPos = new Vector3(x * tileDimensions, y * tileDimensions, z * tileDimensions);
        Vector3 rotation = new Vector3(90,0,0);
        Texture texture = tileTop;
        if (type == Top.PATH) texture = tilePath;
        CreateTile(newPos, rotation, texture, true);
    }

    void GenerateSide(int x, int y, int z, Side side) {
        Vector3 newPos = new Vector3(x * tileDimensions, y * tileDimensions, z * tileDimensions);
        Vector3 offset = new Vector3(0,0,0);
        Vector3 rotation = new Vector3(0,0,0);

        if (side == Side.EAST) {
            offset = new Vector3(-tileDimensions/2, -tileDimensions/2,0);
            rotation = new Vector3(0,90,0);
        } else if (side == Side.WEST) {
            offset = new Vector3(tileDimensions/2,-tileDimensions/2,0);
            rotation = new Vector3(0,-90,0);
        } else if (side == Side.NORTH) {
            offset = new Vector3(0,-tileDimensions/2, -tileDimensions/2);
            rotation = new Vector3(0,0,0);
        } else if (side == Side.SOUTH) {
            offset = new Vector3(0,-tileDimensions/2,tileDimensions/2);
            rotation = new Vector3(0,180,0);
        }
        newPos += offset;
        CreateTile(newPos, rotation, tileSide, false);
    }

    void CreateTile(Vector3 pos, Vector3 rotation, Texture texture, bool isTop) {
        GameObject quad = GameObject.CreatePrimitive( PrimitiveType.Quad );
        quad.isStatic = true;
        quad.GetComponent<Renderer>().material.mainTexture = texture; 
        quad.transform.localScale = new Vector3(tileDimensions,tileDimensions,1);
        
        quad.transform.Translate(pos);
        quad.transform.Rotate(rotation, Space.Self);
        quad.transform.parent = GameObject.Find("Tiles").transform;

        if (isTop) quad.layer = 8;
        else quad.GetComponent<MeshCollider>().enabled = false;
    }
    int CalculateHeights(int x, int y)
    {
        float xCoord = (float) x / width * scale;
        float yCoord = (float) y / length * scale;

        return flatten(Mathf.PerlinNoise(xCoord, yCoord));
    }

    int flatten(float h) {
        int temp = ((int)(h * height));
        // int retval = 0;
        // if (temp <= 3) retval = 0;
        // else if (temp <= 6)  retval = 1;
        // else retval = 2;

        // return retval;
        return temp;
    }

}
