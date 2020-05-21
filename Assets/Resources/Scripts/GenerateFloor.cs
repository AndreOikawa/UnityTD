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
    [SerializeField]
    private GameObject enemySpawnerPrefab;
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
    void CreateEnemySpawner(Vector3 pos) {
        var enemySpawner = Instantiate(enemySpawnerPrefab, pos, Quaternion.identity);
        enemySpawner.transform.parent = GameObject.Find("EnemySpawner").transform;
        
    }


    
    void MakePathLines(Vector2Int begin, Vector2Int end) {
        Debug.Log("Begin: " + begin + " End: " + end);
        int prevY = -1;
        bool first = true;
        
        while (begin != end) {
            int y = HighestTop(begin.x,begin.y);
            if (prevY != y) {
                if (!first) {
                    var prev = new Vector3(begin.x,prevY+0.5f,begin.y);
                    if (begin.x == end.x) {
                        prev.z += (begin.y > end.y ? 1: -1);
                    } else {
                        prev.x += (begin.x > end.x ? 1: -1);
                    }
                    CreateWaypoint(prev * tileDimensions);
                    var pos = new Vector3(begin.x,y + 0.5f,begin.y);
                    CreateWaypoint(pos * tileDimensions);
                }
                prevY = y;
                first = false;
            }
            tiles[begin.x,y,begin.y] = Top.PATH;
            if (begin.x == end.x) {
                begin.y += (begin.y > end.y ? - 1: 1);
            } else {
                begin.x += (begin.x > end.x ? - 1: 1);
            }
        }
        int lasty = HighestTop(end.x, end.y);
        var last = new Vector3(end.x, lasty + 0.5f, end.y);
        tiles[end.x, lasty, end.y] = Top.PATH;
        CreateWaypoint(last * tileDimensions);
        
        
    }
    void CreatePath() {
        int xStart = 4;
        int zStart = 4;
        int xEnd = width - 5;
        int zEnd = length - 5;

        var spawnPos = new Vector3(xStart, HighestTop(xStart,zStart), zStart);

        CreateEnemySpawner(spawnPos * tileDimensions);
        Vector2Int start = new Vector2Int(xStart, zStart);
        Vector2Int end = new Vector2Int(xEnd/2,zStart);
        MakePathLines(start,end);
        start = end;
        end = new Vector2Int(xEnd/2, zEnd/2);
        MakePathLines(start,end);
        start = end;
        end = new Vector2Int(xStart, zEnd/2);
        MakePathLines(start,end);
        start = end;
        end = new Vector2Int(xStart, zEnd);
        MakePathLines(start,end);
        start = end;
        end = new Vector2Int(3 * xEnd / 4, zEnd);
        MakePathLines(start,end);
        start = end;
        end = new Vector2Int(3 * xEnd / 4, zStart);
        MakePathLines(start,end);
        start = end;
        end = new Vector2Int(xEnd, zStart);
        MakePathLines(start,end);
        start = end;
        end = new Vector2Int(xEnd, zEnd);
        MakePathLines(start,end);
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
