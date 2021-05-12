using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PathToFood : MonoBehaviour
{
    private Transform target;
    private NavMeshPath path;
    private float elapsed = 0.0f;
    public float viewDistance = 50.0f;

    private RaycastHit hit;
    private bool move_to = false;
    private Vector3[] list3 = new Vector3[50];

    // Start is called before the first frame update
    void Start()
    {
        //RaycastHit hit;
        path = new NavMeshPath();
        elapsed = 0.0f;
        //Debug.Log(target);
        //Debug.Log(this.gameObject);

        //StartCoroutine(timer());
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        if (Physics.Raycast(transform.position, forward, out hit, viewDistance))
        {
            if (hit.transform.tag == "Fruit")
            {
                //Debug.Log("fruit");
                target = hit.transform;
            }
        }

        elapsed += Time.deltaTime;
        if (elapsed > 1.0f && target != null)
        {
            elapsed -= 1.0f;
            NavMesh.CalculatePath(transform.position, target.position, UnityEngine.AI.NavMesh.AllAreas, path);
            move_to = true;
            //UnityEngine.AI.NavMeshAgent.SetPath(path);
        }
        else
        {
            move_to = false;
        }

        // debugging path
        for (int i = 0; i < path.corners.Length - 1; i++)
        {
            Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red);
            //Debug.Log(path.corners[i]);
            Debug.Log(path.corners.Length - 1);
            list3[i] = path.corners[i];
            Debug.Log(list3[i]);
        }

        //transform.position = Vector3.MoveTowards(transform.position, list3[0], 2.0f * Time.deltaTime);
    }

    public NavMeshPath path_
    {
        get { return path; }
        set { path = value;  }
    }

    public bool move_to_
    {
        get { return move_to; }
    }

    /*IEnumerator timer()
    {
        while (true)
        {
            //Debug.Log("timer");
            yield return new WaitForSeconds(time1);
        }
    }*/
}
