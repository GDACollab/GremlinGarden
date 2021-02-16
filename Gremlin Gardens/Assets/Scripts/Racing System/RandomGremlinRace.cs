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
    /// What to attach to the player's gremlin to differentiate them from the others.
    /// </summary>
    public GameObject playerIndicator;

    string[] GremlinNames = {"Happy", "Grumpy", "Doc", "Sleepy", "Bashful", "Sneezy", "Dopey", "Shrek", "Donkey", "Gremstork", "Dave"};

    /// <summary>
    /// Number of gremlins to populate the race with.
    /// </summary>
    public int gremlinCount = 4;

    // Start is called before the first frame update
    void Start()
    {
        List<GameObject> gremlinList = new List<GameObject>();
        int playerGremlin = Random.Range(0, gremlinCount);
        for (int i = 0; i < gremlinCount; i++)
        {
            var gremlin = Instantiate(gremlinObject);
            gremlin.GetComponent<GremlinObject>().gremlin.setStat("Running", Random.Range(0.5f, 1f));
            gremlin.GetComponent<GremlinObject>().gremlin.setStat("Flying", Random.Range(0.5f, 1f));
            gremlin.GetComponent<GremlinObject>().gremlin.setStat("Stamina", Random.Range(0.5f, 1f));
            gremlin.GetComponent<GremlinObject>().gremlin.setStat("Climbing", Random.Range(0.5f, 1f));
            gremlin.GetComponent<GremlinObject>().gremlin.setStat("Happiness", Random.Range(0.5f, 1f));
            gremlin.GetComponent<GremlinObject>().gremlin.setStat("Swimming", Random.Range(0.5f, 1f));
            
            if (i == playerGremlin)
            {
                gremlin.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.red);
                Instantiate(playerIndicator, gremlin.transform);
                gremlin.name = "Player";
            }
            else {
                gremlin.GetComponent<MeshRenderer>().material.SetColor("_Color", new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)));
                gremlin.name = GremlinNames[Random.Range(0, GremlinNames.Length)];
            }
            gremlinList.Add(gremlin);
        }
        raceManager.TrackSetup(gremlinList, playerGremlin);
        //raceManager.StartTracks();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
