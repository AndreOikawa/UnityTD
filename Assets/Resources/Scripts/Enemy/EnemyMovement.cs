using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 10f;
    public bool debug = false;
    
    public CharacterController controller;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public float jumpHeight = 3f;
    public float jumpEarlyDist = 2f;
    public LayerMask groundMask;
    bool isGrounded;
    
    public float gravity = -20f;
    public float terminalVelocity = 20f;
    

    private Vector3 velocity;
    private List<Transform> waypoints;
    private int current;
    // Start is called before the first frame update
    void Start()
    {
        waypoints = Waypoint.points;
        current = 0;
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = waypoints[current].position - transform.position;
        // transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);
        // if (debug) Debug.Log(transform.position);
        Vector3 xz = transform.position;
        xz.y = waypoints[current].position.y;
        

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < -2f) {
            velocity.y = -2f;
        }

        // float x = Input.GetAxis("Horizontal");
        // float z = Input.GetAxis("Vertical");
        float y = dir.y;
        dir.y = 0;
        // Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(dir.normalized * speed * Time.deltaTime);

        if (y > 1 && isGrounded) {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        if (Mathf.Abs(velocity.magnitude) > terminalVelocity) {
            float scaleFactor = terminalVelocity/velocity.magnitude;
            velocity *= scaleFactor;
        }
        controller.Move(velocity * Time.deltaTime);

        if (debug) Debug.Log(velocity.y);

        // if (transform.position.y < -4f) {
        //     if (debug) Debug.Log("outofbounds" + transform.position.y);
        //     transform.position = new Vector3(10,10,10);
        // }
        if (Vector3.Distance(xz, waypoints[current].position) < 0.7f) {
            current++;
            if (debug) Debug.Log(current);
            if (current >= waypoints.Count) {
                Destroy(gameObject);
                // current = 0;
            }
        }
        
    }
}
