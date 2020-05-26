using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectEnemy : MonoBehaviour
{
    public Transform GetClosestEnemy(Vector3 position) {
        int numChildren = transform.childCount;
        if (numChildren == 0) return null;
        Transform retval = transform.GetChild(0);
        float minDist = (position - retval.position).magnitude;
        for (int i = 1; i < numChildren; i++) {
            Transform curr = transform.GetChild(i);
            float currDist = (position - curr.position).magnitude;
            if (minDist > currDist) {
                retval = curr;
                minDist = currDist;
            }
        }

        return retval;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
