using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GremlinControl : MonoBehaviour
{
    public NavMeshAgent agent;

    //public float viewRadius;
    public float viewDistance = 50.0f;
    //public int time1 = 4;
    private GremlinInteraction gremInt;
    //private bool beingCarried = false;

    private RaycastHit hit;

    // Start is called before the first frame update
    void Start()
    {
        gremInt = this.gameObject.GetComponent<GremlinInteraction>();
        agent = this.gameObject.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        // experiment with base offset for navmesh agent
        // need to know if gremlin is being carried to disable navmesh agent
        // navmesh agent messing with gremlin carry
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        if (gremInt.beingCarried_)
        {
            Debug.Log("carried");
            agent.enabled = false;
        }
        else
        {
            agent.enabled = true;
        }

        // need to reset path after; physics is a bit wonky as gremlin can fly off
        // reset path after object destroyed
        if (Physics.Raycast(transform.position, forward, out hit, viewDistance))
        {
            if (hit.transform.tag == "Fruit")
            {
                Debug.Log("fruit");
                agent.SetDestination(hit.point);
            }
        }
    }
}
