using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveGremlin : MonoBehaviour
{
    public float speed = 0.13f; // 0.1f - 0.15f

    private PathToFood path_script;
    private Vector3 dest;
    private int length;

    // Debugging variables
    //private Vector3 vect1 = new Vector3(23.3f, 34.9f, 40.9f);

    // Start is called before the first frame update
    void Start()
    {
        path_script = this.gameObject.GetComponentInChildren<PathToFood>();
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = Vector3.MoveTowards(transform.position, vect1, step);
        if (path_script.move_to_ == true)
        {
            length = path_script.path_.corners.Length - 1;
            dest = new Vector3(path_script.path_.corners[length].x, path_script.path_.corners[length].y, path_script.path_.corners[length].z);
            this.transform.position = Vector3.MoveTowards(transform.position, dest, speed);

            //Debug.LogWarning(pos);
            //this.transform.position = Vector3.Lerp(pos, this.transform.position, Time.deltaTime);
        }
    }
}
