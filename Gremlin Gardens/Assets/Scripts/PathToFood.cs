using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathToFood : MonoBehaviour
{
    public float viewRadius;
    public float viewDistance = 50.0f;
    public int time1 = 4;

    //private LineRenderer laserLine;
    private RaycastHit hit;
    //private beingCarried = false;

    // Start is called before the first frame update
    void Start()
    {
        //RaycastHit hit;

        // Get and store references to line renderer component
        // laserLine = GetComponent<LineRenderer>();
        StartCoroutine(timer());
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        //Debug.DrawRay(transform.position, forward, Color.green);
        // Can use an invisible cone instead to simulate sight
        if (Physics.Raycast(transform.position, forward, out hit, viewDistance))
        {
            if (hit.transform.tag == "Fruit")
            {
                //Debug.Log(hit.distance);
                //Debug.Log("fruit");
                //Debug.Log(hit.transform.position);
            }
        }
    }

    IEnumerator timer()
    {
        while (true)
        {
            //Debug.Log("timer");
            yield return new WaitForSeconds(time1);
        }
    }
}
