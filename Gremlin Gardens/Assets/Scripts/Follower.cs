using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{

    public GameObject gremlin;
    public Vector3 offset;

    // Update is called once per frame
    void Update()
    {
        transform.position = gremlin.transform.position + offset;
    }
}
