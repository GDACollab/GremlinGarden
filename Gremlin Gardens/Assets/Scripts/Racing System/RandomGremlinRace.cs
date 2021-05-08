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
            GameObject gremlin;
            if (i == playerGremlin)
            {
                GremlinObject gremlinToLoad;
                if (LoadingData.playerGremlins.Count != 0)
                {
                    gremlinToLoad = LoadingData.playerGremlins[LoadingData.gremlinToRace];
                }
                else {
                    // For testing and debugging, in case someone decides to load the race without going through the hub world:
                    gremlinToLoad = gameObject.AddComponent<GremlinObject>();
                    gremlinToLoad.gremlin = new Gremlin("My Spoon is Too Big.");
                    gremlinToLoad.gremlinName = "My Spoon is Too Big.";
                    gremlinToLoad.InitializeGremlin();
                }
                // Instead of making a random gremlin, load the player gremlin.
                gremlin = Instantiate(gremlinObject);
                gremlin.GetComponent<GremlinObject>().CopyGremlinData(gremlinToLoad);
                Instantiate(playerIndicator, gremlin.transform);
                gremlin.name = gremlinToLoad.gremlinName;
            }
            else {
                gremlin = Instantiate(gremlinObject);
                Gremlin gremlinClass = gremlin.GetComponent<GremlinObject>().gremlin;
                gremlinClass.setStat("Running", Random.Range(0.5f, 1f));
                gremlinClass.setStat("Flying", Random.Range(0.5f, 1f));
                gremlinClass.setStat("Stamina", Random.Range(0.5f, 1f));
                gremlinClass.setStat("Climbing", Random.Range(0.5f, 1f));
                gremlinClass.setStat("Happiness", Random.Range(0.5f, 1f));
                gremlinClass.setStat("Swimming", Random.Range(0.5f, 1f));
                gremlin.transform.GetChild(0).GetComponent<MeshRenderer>().material.SetColor("_Color", new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)));
                gremlin.name = GremlinNames[Random.Range(0, GremlinNames.Length)];
                gremlin.GetComponent<GremlinObject>().gremlinName = gremlin.name;
            }
            gremlinList.Add(gremlin);
        }
        raceManager.TrackSetup(gremlinList, playerGremlin);
    }
}
