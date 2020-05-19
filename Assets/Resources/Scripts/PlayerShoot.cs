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
    [SerializeField]
    private float bulletSpeed = 1000f;
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
        // RaycastHit hit;
        // float weaponRange = 100f;
        // if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, weaponRange, mask)) {
        //     Debug.Log("hit " + hit.collider.name);
            
            
        // }
        var projectile = Instantiate(projectileObj, camera.transform.position + camera.transform.forward * 2, Quaternion.identity);
        projectile.GetComponent<Rigidbody>().AddForce(camera.transform.forward * bulletSpeed);
        projectile.transform.parent = GameObject.Find("Projectiles").transform;
    }
}
