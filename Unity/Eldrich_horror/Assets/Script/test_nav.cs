using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class test_nav : MonoBehaviour {

    public Transform target_pos;
    public NavMeshAgent agent;
    public RaycastHit hit;

    private int LocationMask;
    
    void Start () {
        agent = GetComponent<NavMeshAgent>();
        LocationMask = LayerMask.GetMask("Location");
    }
	
	// Update is called once per frame
	void Update() {       
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, LocationMask))
            {                
                agent.SetDestination(hit.point);
            }
        }
    }
}
