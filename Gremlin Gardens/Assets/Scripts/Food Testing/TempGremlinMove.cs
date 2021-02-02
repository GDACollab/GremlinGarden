using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempGremlinMove : MonoBehaviour
{
    //Makes the gremlin look for food an move towards it
    // Start is called before the first frame update
    private Rigidbody rigidbody;
    public float speed;
    void Start()
    {
        rigidbody = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        FoodObject[] foods = FindObjectsOfType<FoodObject>();
        if (foods.Length != 0)
        {
            gameObject.transform.LookAt(foods[0].transform.position);
            rigidbody.velocity = transform.forward * speed;
        } else
        {
            rigidbody.velocity = Vector3.zero;
        }
    }
}
