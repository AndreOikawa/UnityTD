using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuild : MonoBehaviour
{
    private Camera camera;

    [SerializeField]
    private LayerMask mask;
    [SerializeField]
    private GameObject buildingObj;
    [SerializeField]
    private float bulletSpeed = 1000f;
    // Start is called before the first frame update
    private PlayerVariables playerVariables;
    void Start()
    {
        camera = transform.GetComponentInChildren<Camera>();
        playerVariables = GetComponent<PlayerVariables>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && playerVariables.CurrentMode == PlayerVariables.ClickType.BUILD) {
            Build();
        }
    }

    void Build() {
        RaycastHit hit;
        float weaponRange = 10f;
        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, weaponRange, mask)) {
            // Debug.Log("hit " + hit.collider.name);
            var building = Instantiate(buildingObj, hit.transform.position + new Vector3(0, -1, 0), Quaternion.identity);
            building.layer = 8;
            building.isStatic = true;
            building.transform.parent = GameObject.Find("Towers").transform;
            
            // hit.transform.position
            
        }
        
    }
}
