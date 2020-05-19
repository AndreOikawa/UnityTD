using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField]
    private Camera camera;

    [SerializeField]
    private LayerMask mask;
    [SerializeField]
    private GameObject projectileObj;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1")) {
            Shoot();
        }
    }

    void Shoot() {
        RaycastHit hit;
        float weaponRange = 100f;
        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, weaponRange, mask)) {
            Debug.Log("hit " + hit.collider.name);
            var projectile = Instantiate(projectileObj, camera.transform.position, Quaternion.identity);
            projectile.transform.parent = GameObject.Find("Projectiles").transform;
            
        }
    }
}
