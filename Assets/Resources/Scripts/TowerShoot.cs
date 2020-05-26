using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerShoot : MonoBehaviour
{
    [SerializeField]
    private float attackSpeed = 1f;
    [SerializeField]
    private float attackRadius = 5f;
    [SerializeField]
    private float projectileSpeed = 10f;
    [SerializeField]
    private GameObject projectileObj;
    private Transform target;
    private float time;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Shoot() {
        var shootPosition = transform.position + new Vector3(0,5,0);
        var projectile = Instantiate(projectileObj, shootPosition, Quaternion.identity);
        projectile.GetComponent<Rigidbody>().AddForce((target.position - shootPosition) * projectileSpeed);
        projectile.transform.parent = GameObject.Find("Projectiles").transform;
    }
    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        target = GameObject.Find("Enemies").GetComponent<DetectEnemy>().GetClosestEnemy(transform.position);
        if (target != null && time >= attackSpeed) {
            Shoot();
            time = 0f;
        }
    }
}
