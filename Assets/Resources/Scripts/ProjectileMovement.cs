using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
    [SerializeField]
    private float speed = 0.1f;
    [SerializeField]
    private CharacterController controller;
    private Vector3 velocity;
    [SerializeField]
    private float gravity = -20f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 dir = transform.forward;
        dir.y = 0;
        // transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);
        // if (debug) Debug.Log(transform.position);
        
        controller.Move(dir.normalized * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
        // Debug.Log("moving");
    }

    //Detect collisions between the GameObjects with Colliders attached
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("hit: " + collision.gameObject.name);
        Destroy(gameObject);
    }
}
