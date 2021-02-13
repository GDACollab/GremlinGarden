using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Start a race populated by gremlins with random stats.
/// </summary>
public class RandomGremlinRace : MonoBehaviour
{
    /// <summary>
    /// Manager to start the race with.
    /// </summary>
    public RaceManager raceManager;

    /// <summary>
    /// The gremlins to populate the race with.
    /// </summary>
    public GameObject gremlinObject;

    /// <summary>
    /// Number of gremlins to populate the race with.
    /// </summary>
    public int gremlinCount = 4;

    // Start is called before the first frame update
    void Start()
    {
        List<GameObject> gremlinList = new List<GameObject>();
        for (int i = 0; i < gremlinCount; i++)
        {
            var gremlin = Instantiate(gremlinObject);
            gremlin.GetComponent<MeshRenderer>().material.color = new Color(Random.Range(0, 1), Random.Range(0, 1), Random.Range(0, 1));
            gremlinList.Add(gremlin);
        }
        raceManager.TrackSetup(gremlinList, Random.Range(0, gremlinCount));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
