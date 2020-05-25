using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBattle : MonoBehaviour
{
    [SerializeField]
    private EnemyHealthBar healthBar;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        healthBar.SetMaxHealth(100f);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// OnCollisionEnter is called when this collider/rigidbody has begun
    /// touching another rigidbody/collider.
    /// </summary>
    /// <param name="other">The Collision data associated with this collision.</param>
    void OnCollisionEnter(Collision other)
    {
        Debug.Log("hit by: " + other.gameObject.name);
        TakeDamage(10f);
        if (healthBar.IsDead()) Destroy(gameObject);
    }
    private void TakeDamage(float val) {
        healthBar.DamageTaken(val);
    }
}
