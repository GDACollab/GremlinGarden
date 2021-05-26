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

    /// <summary>
    /// The maximum amount of stamina a gremlin can have. Used by TrackManager.
    /// </summary>
    [HideInInspector]
    public float maxStamina;

    private GameObject player; 
    
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
        maxStamina = gremlin.getStat("Stamina");
        nameText.text = gremlinName;
    }

    void Update()
    {
        // Set the name text to face the player.
        if(player != null)
            nameText.transform.rotation = Quaternion.LookRotation(this.transform.position - player.transform.position, player.transform.up);
    }
}
