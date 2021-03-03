using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodObject : MonoBehaviour
{
    public Food food;

    public List<string> _stats;
    public List<float> _values;
    public Dictionary<string, float> stats;


    public string foodName;
    public float size;


    private Rigidbody rgbd;


    
    // Start is called before the first frame update
    // In start, the empty game object is turned into a Food Object
    public void Start()
    {
        stats = new Dictionary<string, float>();
        for (int i = 0; i < Mathf.Min(_stats.Count, _values.Count); i++)
        {
            stats.Add(_stats[i], _values[i]);
        }

        food = new Food(this.gameObject.GetComponent<MeshFilter>().sharedMesh, this.gameObject.GetComponent<MeshRenderer>().material,
            foodName, stats);
        //Adds Necessary Collision Components
        gameObject.AddComponent<SphereCollider>();
        rgbd = gameObject.AddComponent<Rigidbody>();
        //Scales Size Down
        scaleObjectSize(gameObject, size);

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
  
}
