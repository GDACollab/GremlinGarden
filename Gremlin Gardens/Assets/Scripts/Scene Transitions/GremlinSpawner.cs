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
            foreach (KeyValuePair<string, Gremlin> savedGremlin in LoadingData.playerGremlins) {
                SpawnGremlin(savedGremlin.Value);
            }
        }
    }

    /// <summary>
    /// Used for actually spawning in a gremlin from pre-existing data. Intended to be used when the game loads from LoadingData.
    /// </summary>
    /// <param name="gremlinData">The gremlin data to spawn the gremlin with.</param>
    public void SpawnGremlin(Gremlin gremlinData) {
        // TODO: Get this to work somehow with updating gremlin positions.
        var gremlin = Instantiate(gremlinPrefab);
        gremlin.GetComponent<GremlinObject>().gremlin.CopyGremlinData(gremlinData);
        gremlin.GetComponent<GremlinObject>().nameText.text = gremlinData.getName();
        gremlin.GetComponent<GremlinInteraction>().gremlin = gremlin.GetComponent<GremlinObject>().gremlin;
        gremlin.transform.Find("gremlinModel").transform.Find("gremlin.mesh").GetComponent<SkinnedMeshRenderer>().material.SetColor("_Color", gremlin.GetComponent<GremlinObject>().gremlin.gremColor);
        gremlin.transform.position = gremlin.GetComponent<GremlinObject>().gremlin.currentPosition;
        gremlin.transform.rotation = gremlin.GetComponent<GremlinObject>().gremlin.currentRotation;
        gremlin.GetComponent<GremlinObject>().gremlin.transformReference = gremlin.transform;
        gremlin.name = gremlin.GetComponent<GremlinObject>().gremlin.getName();
        // I wonder if I could use a dictionary of pointers or something to make this easier. Oh well, we've reached the phase of the project where we resolve stuff with quick fixes and easy hacks.
        // Basically, we make sure that when a gremlin is created, the reference data for that gremlin is actually tied to the new data we've created.
        // This could probably be solved if I got rid of CopyGremlinData and just passed around .gremlin to LoadingData, but whatever.
        LoadingData.playerGremlins[gremlin.name] = gremlin.GetComponent<GremlinObject>().gremlin;
        Debug.Log("Loaded data for: " + gremlinData.getName());
    }

    /// <summary>
    /// For spawning a new Gremlin that will be permanently added to the player's list of gremlins.
    /// </summary>
    /// <param name="gremlinData">The gremlin data to initialize the Gremlin with. Optional.</param>
    public void CreateGremlin(Vector3 gremlinPosition, Gremlin gremlinData = null) {
        newGremlin = Instantiate(gremlinPrefab);
        newGremlin.transform.position = gremlinPosition;
        newGremlin.GetComponent<GremlinObject>().gremlin.InitializeGremlin();
        newGremlin.GetComponent<GremlinObject>().gremlin.transformReference = newGremlin.transform;
        if (gremlinData != null)
        {
            newGremlin.GetComponent<GremlinObject>().gremlin.CopyGremlinData(gremlinData);
        }
        var name = Instantiate(gremlinNamingPrefab, UI.transform);
        name.GetComponent<GremlinNamer>().BeginScanningInput(GetGremlinName, ValidateGremlinName);
        newGremlin.transform.Find("gremlinModel").transform.Find("gremlin.mesh").GetComponent<SkinnedMeshRenderer>().material.SetColor("_Color", Random.ColorHSV(0f, 1f, .6f, .8f, .5f, .7f));
        newGremlin.GetComponent<GremlinObject>().gremlin.gremColor = newGremlin.transform.Find("gremlinModel").transform.Find("gremlin.mesh").GetComponent<SkinnedMeshRenderer>().material.GetColor("_Color");
    }

    bool ValidateGremlinName(string text) {
        // Only validate if there is no gremlin already with that name.
        return !LoadingData.playerGremlins.ContainsKey(text);
    }

    public void GetGremlinName(string name) {
        newGremlin.GetComponent<GremlinObject>().gremlinName = name;
        newGremlin.GetComponent<GremlinObject>().gremlin.setName(name);
        newGremlin.name = name;
        newGremlin.GetComponent<GremlinObject>().nameText.text = name;
        LoadingData.playerGremlins[name] = newGremlin.GetComponent<GremlinObject>().gremlin;
        Debug.Log("Created: " + name);
    }
}
