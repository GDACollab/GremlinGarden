using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempGremlinMove : MonoBehaviour
{
    //Makes the gremlin look for food an move towards it
    // Start is called before the first frame update
    private Rigidbody rigidbody;
    public float strength;
    public float speed;
    void Start()
    {
        speed = 1000f;
        strength = 1;
        rigidbody = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        FoodObject[] foods = FindObjectsOfType<FoodObject>();

        if (foods.Length != 0)
        {
            gameObject.transform.LookAt(foods[0].transform.position);

        } else
        {
            gameObject.transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
        }
        //Debug.Log(transform.rotation.y);


        float x = transform.position.x;
        float z = transform.position.z;

        if (transform.position.x > 4)
        {
            x = 4;
            transform.Rotate(Vector3.up, 200);
            rigidbody.velocity = Vector3.zero;
        } else if (transform.position.x < -4)
        {
            x = -4;
            transform.Rotate(Vector3.up, 200);
            rigidbody.velocity = Vector3.zero;
        } else if (transform.position.z > 4)
        {
            z = 4;
            transform.Rotate(Vector3.up, 200);
            rigidbody.velocity = Vector3.zero;
        } else if (transform.position.z < -4)
        {
            z = -4;
            transform.Rotate(Vector3.up, 200);
            rigidbody.velocity = Vector3.zero;
        }
        transform.position = new Vector3(x, transform.position.y, z);
     }

    public void FixedUpdate()
    {
        rigidbody.AddForce(speed * transform.forward * Time.deltaTime);
    }

    public void EatFood(Food food)
    {
        strength += food.getStats()["strength"];
        speed += food.getStats()["speed"];
        Debug.Log(strength);
        scaleObjectSize(gameObject, strength);
    }

    private void scaleObjectSize(GameObject gameObject, float newSize)
    {
        gameObject.transform.localScale = Vector3.one * newSize;
    }
}
