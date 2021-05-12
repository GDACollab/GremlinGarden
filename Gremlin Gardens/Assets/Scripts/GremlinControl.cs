using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GremlinControl : MonoBehaviour
{
    public NavMeshAgent agent;
    private NavMeshPath path;
    //public float viewRadius;
    public float viewDistance = 50.0f;
    //public int time1 = 4;
    private GremlinInteraction gremInt;
    private NavMeshPath path1;
    private bool travel = false;
    //private bool beingCarried = false;
    private PathToFood path_script;
    private RaycastHit hit;

    // Start is called before the first frame update
    void Start()
    {
        //gremInt = this.gameObject.GetComponent<GremlinInteraction>();
        agent = this.gameObject.GetComponent<NavMeshAgent>();
        path_script = this.gameObject.GetComponent<PathToFood>();
        Debug.Log(this.gameObject.transform.position);

        // (23.3, 34.9, 40.9)
    }

    // Update is called once per frame
    void Update()
    {
        // experiment with base offset for navmesh agent
        // need to know if gremlin is being carried to disable navmesh agent
        // navmesh agent messing with gremlin carry
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        /*if (gremInt.beingCarried_)
        {
            Debug.Log("carried");
            agent.enabled = false;
        }
        else
        {
            agent.enabled = true;
        }*/

        path1 = path_script.path_;

        if (path_script.path_ != null)
        {
            travel = agent.SetPath(path_script.path_);
        }

        for (int i = 0; i < path1.corners.Length - 1; i++)
            Debug.Log(path1.corners[i]);

        // need to reset path after; physics is a bit wonky as gremlin can fly off
        // reset path after object destroyed
        /*if (Physics.Raycast(transform.position, forward, out hit, viewDistance))
        {
            if (hit.transform.tag == "Fruit")
            {
                Debug.Log("fruit");
                agent.SetDestination(hit.point);
            }
        }*/
    }
}
