using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodObject : MonoBehaviour
{
    public Food food;

    public List<string> _stats;
    public List<float> _values;
    public Dictionary<string, float> stats;
    public bool multipleMeshes;


    public string foodName;
    public float size;


    private Rigidbody rgbd;


    
    // Start is called before the first frame update
    // In start, the empty game object is turned into a Food Object
    public void Start()
    {
        // If there are multiple Meshes, combines them
        if (multipleMeshes)
        {
            Vector3 oldPos = transform.position;
            transform.position = Vector3.zero;
            MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
            CombineInstance[] combine = new CombineInstance[meshFilters.Length];

            int i = 0;
            while (i < meshFilters.Length)
            {
                combine[i].mesh = meshFilters[i].sharedMesh;
                combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
                meshFilters[i].gameObject.SetActive(false);

                i++;
            }
            transform.GetComponent<MeshFilter>().mesh = new Mesh();
            transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
            transform.gameObject.SetActive(true);
            transform.position = oldPos;
            Outline outline = GetComponent<Outline>();
            if (outline != null)
            {
                outline.enabled = true;
            }
        }

        stats = new Dictionary<string, float>();
        for (int i = 0; i < Mathf.Min(_stats.Count, _values.Count); i++)
        {
            stats.Add(_stats[i], _values[i]);
        }

        food = new Food(this.gameObject.transform.GetChild(1).GetComponent<MeshFilter>().sharedMesh, this.gameObject.transform.GetChild(1).GetComponent<MeshRenderer>().material,
            foodName, stats);
        
        /*
        //Adds Necessary Collision Components
        gameObject.AddComponent<BoxCollider>();
        rgbd = gameObject.AddComponent<Rigidbody>();
        //Scales Size Down
        scaleObjectSize(gameObject, size);
        */

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
