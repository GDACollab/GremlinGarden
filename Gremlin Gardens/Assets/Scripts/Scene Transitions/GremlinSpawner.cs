using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GremlinSpawner : MonoBehaviour
{
    /// <summary>
    /// The gremlin prefab to use for spawning in save data. This system might need to change if more gremlin models/prefabs get added.
    /// </summary>
    [Tooltip("The gremlin prefab to use for spawning in save data. This system might need to change if more gremlin models/prefabs get added.")]
    public GameObject gremlinPrefab;

    /// <summary>
    /// The UI prefab to use for naming gremlins.
    /// </summary>
    [Tooltip("The UI prefab to use for naming gremlins.")]
    public GameObject gremlinNamingPrefab;

    /// <summary>
    /// The UI to attach stuff to.
    /// </summary>
    [Tooltip("The UI to attach stuff to.")]
    public GameObject UI;

    /// <summary>
    /// Used when creating gremlins to keep track of the gremlin being initialized.
    /// </summary>
    GameObject newGremlin;
    // Start is called before the first frame update
    void Start()
    {
        // If we've got no pre-existing player gremlins, create the ones the player needs to start with.
        if (LoadingData.playerGremlins.Count == 0)
        {
            // Hard coded value for now. Maybe we can get more creative later.
            CreateGremlin(new Vector3(22, 35, 37));
        }
        else {
            foreach (KeyValuePair<string, GremlinObject> savedGremlin in LoadingData.playerGremlins) {
                SpawnGremlin(savedGremlin.Value);
            }
        }
    }

    /// <summary>
    /// Used for actually spawning in a gremlin from pre-existing data. Intended to be used when the game loads from LoadingData.
    /// </summary>
    /// <param name="gremlinData">The gremlin data to spawn the gremlin with.</param>
    public void SpawnGremlin(GremlinObject gremlinData) {
        var gremlin = Instantiate(gremlinPrefab);
        gremlin.name = gremlinData.gremlinName;
        gremlin.transform.position = gremlinData.currentPosition;
        gremlin.GetComponent<GremlinObject>().CopyGremlinData(gremlinData);
        Debug.Log("Loaded data for: " + gremlinData.gremlinName);
    }

    /// <summary>
    /// For spawning a new Gremlin that will be permanently added to the player's list of gremlins.
    /// </summary>
    /// <param name="gremlinData">The gremlin data to initialize the Gremlin with. Optional.</param>
    public void CreateGremlin(Vector3 gremlinPosition, GremlinObject gremlinData = null) {
        newGremlin = Instantiate(gremlinPrefab);
        newGremlin.transform.position = gremlinPosition;
        newGremlin.GetComponent<GremlinObject>().InitializeGremlin();
        if (gremlinData != null)
        {
            newGremlin.GetComponent<GremlinObject>().CopyGremlinData(gremlinData);
        }
        var name = Instantiate(gremlinNamingPrefab, UI.transform);
        name.GetComponent<GremlinNamer>().BeginScanningInput(GetGremlinName, ValidateGremlinName);
    }

    bool ValidateGremlinName(string text) {
        // Only validate if there is no gremlin already with that name.
        return !LoadingData.playerGremlins.ContainsKey(text);
    }

    public void GetGremlinName(string name) {
        newGremlin.GetComponent<GremlinObject>().gremlinName = name;
        newGremlin.name = name;
        LoadingData.playerGremlins[name] = newGremlin.GetComponent<GremlinObject>();
        Debug.Log("Created: " + name);
    }
}
