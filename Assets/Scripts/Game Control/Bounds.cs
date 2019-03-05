using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounds : MonoBehaviour {

    private Collider plane_collider;

	// Use this for initialization
	void Start () {
        plane_collider = this.GetComponent<BoxCollider>();
    }
	
	// Update is called once per frame
	void Update () {
    }

    void OnTriggerExit(Collider col)
    {
        if (col.transform.position.x > plane_collider.bounds.size.x / 2)
        {
            col.transform.position = new Vector3(-plane_collider.bounds.size.x / 2 - col.bounds.size.x / 2, col.transform.position.y, col.transform.position.z);
        }
        else if (col.transform.position.x < -plane_collider.bounds.size.x / 2)
        {
            col.transform.position = new Vector3(plane_collider.bounds.size.x / 2 + col.bounds.size.x / 2, col.transform.position.y, col.transform.position.z);
        }

        if (col.transform.position.z > plane_collider.bounds.size.z / 2)
        {
            col.transform.position = new Vector3(col.transform.position.x, col.transform.position.y, -plane_collider.bounds.size.z / 2);
        }
        else if (col.transform.position.z < -plane_collider.bounds.size.z / 2)
        {
            col.transform.position = new Vector3(col.transform.position.x, col.transform.position.y, plane_collider.bounds.size.z / 2);
        }
    }
}
