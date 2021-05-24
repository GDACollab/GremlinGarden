﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PathToFood : MonoBehaviour
{
    // view distance of gremlin for raycast
    public float viewDistance = 50.0f;
    // determines how frequently the gremlin moves (not smooth)
    public float endtime = 0.3f;

    private float elapsed = 0.0f;
    // determines whether to move gremlin or not
    private bool move_to = false;

    private RaycastHit hit;
    private Transform target;
    private NavMeshPath path;
    private NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        path = new NavMeshPath();
        elapsed = 0.0f;
        // agent necessary? Make sure to re-examine
        agent = this.GetComponent<NavMeshAgent>();

        //StartCoroutine(timer());
    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 forward = transform.TransformDirection(Vector3.forward);
        /*if (Physics.Raycast(transform.position, forward, out hit, viewDistance))
        {
            if (hit.transform.tag == "Fruit")
            {
                //Debug.Log("fruit");
                target = hit.transform;
            }
        }*/

        // Debug test purposes
        //target = GameObject.FindGameObjectsWithTag("Fruit")[0].transform;
        // 2 problems:
        // Gremlin can go through objects
        // Gremlin can move to object even if it is being carried

        elapsed += Time.deltaTime;
        if (elapsed > endtime && target != null)
        {
            elapsed -= endtime;
            NavMesh.CalculatePath(transform.position, target.position, UnityEngine.AI.NavMesh.AllAreas, path);
            move_to = true;
        }
        else
        {
            move_to = false;
            agent.ResetPath();
        }

        // debugging path
        for (int i = 0; i < path.corners.Length - 1; i++)
        {
            //Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red);
            //Debug.Log(path.corners.Length - 1);
            //list3[i] = path.corners[i];
            //Debug.Log(list3[i]);
        }

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