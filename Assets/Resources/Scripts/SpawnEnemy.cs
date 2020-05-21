using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private GameObject enemyPrefab;
    [SerializeField]
    private float waveTimer = 2f;
    [SerializeField]
    private bool debug = false;
    private float time = 0f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time > waveTimer) {
            var enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
            enemy.transform.parent = GameObject.Find("Enemy").transform;
            time = 0f;
            if (debug) Debug.Log("spawned");
        }
    }
}
