using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FoodSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < 20; i++)
        {
            GameObject food = new GameObject();
            food.AddComponent<FoodObject>();
            List<string> values = Food.allPossibleFood.Keys.ToList<string>();
            int size = Food.allPossibleFood.Count;
            food.GetComponent<FoodObject>().size = 1;
            food.GetComponent<FoodObject>().foodID = values[Random.Range(0, size)];
            food.transform.position = new Vector3(Random.Range(-30f, 30f), 2f, Random.Range(-30f, 30f));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
