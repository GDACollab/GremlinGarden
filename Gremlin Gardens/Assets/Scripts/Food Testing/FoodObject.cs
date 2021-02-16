using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodObject : MonoBehaviour
{

    public string foodID;
    private Food food;
    private Rigidbody rgbd;
    public float size;
    private bool eaten;
    // Start is called before the first frame update
    // In start, the empty game object is turned into a Food Object
    public void Start()
    {
        if (foodID != null && foodID != "")
        {
            eaten = false;
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
        

    }

    // Update is called once per frame
    void Update()
    {

    }


    /**
     * Scales the size of the fruit
     * 
     * @param gameObject: The game object that is to be scaled
     * @param newSize: The size the game object should become. The object's height will become equal to the new size and the rest of the dimensions will scale accordingly
     */
    private void scaleObjectSize(GameObject gameObject, float newSize)
    {
        float currentSize = gameObject.GetComponent<Renderer>().bounds.size.y;
        Vector3 scale = gameObject.transform.localScale;
        scale = newSize * scale / currentSize;
        gameObject.transform.localScale = scale;
    }

    //Detects Collision of Gremlin
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Gremlin"))
        {
            //Destroys the game object
            Destroy(gameObject);

            /*
             Was facing bug where the gremlin would not be destroyed before a another collision occured, resulting
             in the gremlin eating the food twice. To fix this, the food now tracks whether or not it was eaten and
             will only give the gremlin the stats if it hasn't been eaten.
             */
            if (!eaten)
            {
                Debug.Log($"{collision.gameObject.GetComponent<GremlinObject>().gremlinName} ate {food.getName()}!");
                collision.gameObject.GetComponent<GremlinObject>().EatFood(food);
                eaten = true;
            }
            
        }
    }
}
