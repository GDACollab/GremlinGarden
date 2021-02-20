using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomFoodSpawner : MonoBehaviour
{
    public int amount;
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] allFruits = Resources.LoadAll<GameObject>("Food/Specific");
        for (int i = 0; i < amount; i++)
        {
            Vector3 position = new Vector3(Random.Range(-20f, 20f), 1, Random.Range(-20f, 20f));
            Instantiate(allFruits[Random.Range(0, allFruits.Length)], position, Quaternion.identity);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
