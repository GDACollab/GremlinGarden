using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveGremlin : MonoBehaviour
{
    private Vector3 vect1 = new Vector3(23.3f, 34.9f, 40.9f);
    public float speed = 1.0f;
    private PathToFood path_script;
    private Vector3[] list3 = new Vector3[50];
    // Start is called before the first frame update
    void Start()
    {
        path_script = this.gameObject.GetComponentInChildren<PathToFood>();
    }

    // Update is called once per frame
    void Update()
    {
        float step = speed * Time.deltaTime;

        //transform.position = Vector3.MoveTowards(transform.position, vect1, step);
        if (path_script.move_to_ == true)
        {
            list3[0] = path_script.path_.corners[0];
            Vector3 pos = Vector3.MoveTowards(transform.position, list3[0], 10.0f * Time.deltaTime);
        }
    }
}
