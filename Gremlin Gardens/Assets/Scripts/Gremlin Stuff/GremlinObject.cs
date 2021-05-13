using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Class attatched to each Gremlin gameObject
public class GremlinObject : MonoBehaviour
{
    // Variables for the in-game gremlins
    [HideInInspector]
    public Gremlin gremlin;
    Rigidbody body;
    public string gremlinName;

    private GameObject player; 

    /// <summary>
    /// We store this so the GremlinSpawner can place the gremlin back where it was. Used by RaceSelection before loading a new scene (see MouseDown in RaceSelection.cs)
    /// </summary>
    [HideInInspector]
    public Vector3 currentPosition;
    
    [Header("Aesthetic Stuff")]
    /// <summary>
    /// The text the gremlin is displaying.
    /// </summary>
    [Tooltip("The text object for the gremlin to display.")]
    public TMP_Text nameText;

    //Use Awake instead of Start so that we get the stats ASAP. Useful for preventing bugs where the Gremlin is spawned in immediately but does not have stats yet.
    void Awake()
    {
        // Defines the Gremlin
        gremlin = new Gremlin(gremlinName);
        player = GameObject.Find("Player");
        nameText.text = gremlinName;
    }

    void Update()
    {
        // Set the name text to face the player.
        if(player != null)
            nameText.transform.rotation = Quaternion.LookRotation(this.transform.position - player.transform.position, player.transform.up);
    }

    /// <summary>
    /// Initialize the gremlin's relevant stats. Should be called by GremlinSpawner.
    /// </summary>
    public void InitializeGremlin()
    {
        gremlin.setStat("Running", 0);
        gremlin.setStat("Flying", 0);
        gremlin.setStat("Stamina", 0);
        gremlin.setStat("Climbing", 0);
        gremlin.setStat("Happiness", 0);
        gremlin.setStat("Swimming", 0);
    }

    /// <summary>
    /// Used for transferring gremlins across scenes.
    /// </summary>
    /// <param name="srcGremlin">The source gremlin from which to copy.</param>
    public void CopyGremlinData(GremlinObject srcGremlin)
    {
        gremlinName = srcGremlin.gremlinName;
        foreach (KeyValuePair<string, float> statistic in gremlin.getStats())
        {
            gremlin.setStat(statistic.Key, srcGremlin.gremlin.getStat(statistic.Key));
        }
    }
}
