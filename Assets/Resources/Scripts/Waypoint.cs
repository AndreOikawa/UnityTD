using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{

    public static List<Transform> points = new List<Transform>();
    public bool debug = false;
    // Start is called before the first frame update
    public static void AddWaypoint(Transform point) {
        points.Add(point);
    }
    void Start()
    {
        // points = new List<Transform>();
        // if (debug) Debug.Log("There are " + transform.childCount + " waypoints.");
        // points = new Transform[transform.childCount];
        // for (int i = 0; i < points.Length; i++) {
        //     points[i] = transform.GetChild(i);
        //     if (debug) Debug.Log(points[i].transform.position);
        // }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
