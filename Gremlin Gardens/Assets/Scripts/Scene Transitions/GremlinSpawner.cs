using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GremlinSpawner : MonoBehaviour
{
    /// <summary>
    /// Any gremlin prefabs you want the player to start the game with.
    /// </summary>
    [Tooltip("Any gremlin prefabs you want the player to start the game with.")]
    public GameObject[] startingGremlins;
    // Start is called before the first frame update
    void Start()
    {
        // If we've got no pre-existing player gremlins, create the ones the player needs to start with.
        if (LoadingData.playerGremlins.Count == 0)
        {
            foreach (GameObject gremlin in startingGremlins)
            {
                var gremlinToSave = Instantiate(gremlin);
                LoadingData.playerGremlins[gremlin.GetComponent<GremlinObject>().gremlinName] = gremlinToSave;
                Debug.Log("Started game with: " + gremlin.GetComponent<GremlinObject>().gremlinName);
            }
        }
        else {
            foreach (KeyValuePair<string, GameObject> savedGremlin in LoadingData.playerGremlins) {
                Instantiate(savedGremlin.Value);
                Debug.Log("Loaded gremlin data for: " + savedGremlin.Value.GetComponent<GremlinObject>().gremlinName);
            }
        }
    }
}
