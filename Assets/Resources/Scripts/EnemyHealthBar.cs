using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthBar : MonoBehaviour
{
    private void LateUpdate() {
        
        transform.LookAt(GameObject.Find("/Player/Camera").transform.forward + transform.position);
        // transform.Rotate(0,180,0);
    }
}
