using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodObject : MonoBehaviour
{

    // Attach this script to an empty game object and it will become a food scaled to a size of 1
    public string foodID;
    private Food food;
    private Rigidbody rgbd;
    public float size;
    // Start is called before the first frame update

    void Start()
    {
        food = Food.allPossibleFood[foodID];
        //Sets the Model
        MeshRenderer renderer = gameObject.AddComponent<MeshRenderer>();
        MeshFilter objMesh = gameObject.AddComponent<MeshFilter>();
        objMesh.mesh = food.getModel().GetComponent<MeshFilter>().sharedMesh;
        //Sets the Material
        renderer.material = food.getModel().GetComponent<MeshRenderer>().sharedMaterial;
        //Adds Necessary Collision Components
        gameObject.AddComponent<SphereCollider>();
        rgbd = gameObject.AddComponent<Rigidbody>();
        //Scales Size Down
        scaleObjectSize(gameObject, size);
        

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void scaleObjectSize(GameObject gameObject, float newSize)
    {
        float currentSize = gameObject.GetComponent<Renderer>().bounds.size.y;
        Vector3 scale = gameObject.transform.localScale;
        scale = newSize * scale / currentSize;
        gameObject.transform.localScale = scale;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Gremlin"))
        {
            Debug.Log($"Gremlin ate {food.getName()}!");
            collision.gameObject.GetComponent<TempGremlinMove>().EatFood(food);
            Destroy(gameObject);
        }
    }
}
