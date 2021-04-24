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

    /// <summary>
    /// The gremlin prefab to use for spawning in save data. This system might need to change if more gremlin models/prefabs get added.
    /// </summary>
    [Tooltip("The gremlin prefab to use for spawning in save data. This system might need to change if more gremlin models/prefabs get added.")]
    public GameObject gremlinPrefab;
    // Start is called before the first frame update
    void Start()
    {
        // If we've got no pre-existing player gremlins, create the ones the player needs to start with.
        if (LoadingData.playerGremlins.Count == 0)
        {
            foreach (GameObject gremlin in startingGremlins)
            {
                var gremlinToSave = Instantiate(gremlin);
                LoadingData.playerGremlins[gremlin.GetComponent<GremlinObject>().gremlinName] = gremlinToSave.GetComponent<GremlinObject>();
                Debug.Log("Started game with: " + gremlin.name);
            }
        }
        else {
            foreach (KeyValuePair<string, GremlinObject> savedGremlin in LoadingData.playerGremlins) {
                var newGremlin = Instantiate(gremlinPrefab);
                newGremlin.GetComponent<GremlinObject>().CopyGremlinData(savedGremlin.Value);
                Debug.Log("Loaded gremlin data for: " + savedGremlin.Value.gremlinName);
            }
        }
    }
}
