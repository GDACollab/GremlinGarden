using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
/// <summary>
/// Handles the individual aspects of each track module.
/// Should be attached to an object with the PathCreator component.
/// Will handle movement and animation along the track, if the appropriate attributes are provided.
/// </summary>
public class TrackModule : MonoBehaviour
{
    /// <summary>
    /// How fast the Gremlin moves in units per fixed frame count.
    /// </summary>
    public float BaseSpeed = 5.0f;

    /// <summary>
    /// The animation name to play for this TrackModule.
    /// </summary>
    public string AnimationToPlay;

    /// <summary>
    /// Get a GameObject's TerrainVariant data. Be sure to give an empty object a TerrainVariant script (or a script that extends TerrainVariant).
    /// </summary>
    public TerrainVariant TerrainVariant;

    /// <summary>
    /// Is the gremlin moving across this track module?
    /// </summary>
    [HideInInspector]
    public bool gremlinMoving = false;

    /// <summary>
    /// How much the gremlin has moved along the track module.
    /// </summary>
    float totalDistance;
    PathCreator internalCreator;
    
    public void Start()
    {
        totalDistance = 0;
        internalCreator = GetComponent<PathCreator>();
    }

    /// <summary>
    /// Percentage of the base speed that the gremlin's going to move at.
    /// </summary>
    float modifiedSpeed;
    Gremlin activeGremlin; //Class name subject to change?
    float timePassed;
    public delegate void Callback();
    /// <summary>
    /// A callback called at the end of the move.
    /// </summary>
    Callback toCallback;
    /// <summary>
    /// When we're done moving, where are we headed next?
    /// </summary>
    Vector3 nextPos;
    /// <summary>
    /// Start moving the Gremlin across the track module.
    /// </summary>
    /// <param name="gremlin">The Gremlin that's being moved.</param>
    /// <param name="callbackFunc">The function that TrackManager will pass to callback to later.</param>
    public void BeginMove(Gremlin gremlin, Vector3 nextPos, Callback callbackFunc) {
        gremlinMoving = true;
        activeGremlin = gremlin;
        modifiedSpeed = TerrainVariant.relativeSpeed(activeGremlin);
        activeGremlin.GetComponent<Animator>().Play(AnimationToPlay);
        timePassed = 0.0f;
        totalDistance = 0;
        toCallback = callbackFunc;
    }

    public void EndMove() {
        activeGremlin.GetComponent<Animator>().Play("Neutral"); //Reset the Gremlin. If the Gremlin doesn't have a "Neutral" animation state, artifacts from the previous animation will remain.

        gremlinMoving = false;
        toCallback();
    }

    void FixedUpdate()
    {
        if (gremlinMoving) { //Move the little Gremlin around.
            totalDistance += modifiedSpeed * BaseSpeed * Time.fixedDeltaTime;
            if (totalDistance >= internalCreator.path.length)
            {
                EndMove();
            }
            else
            {
                activeGremlin.transform.position = internalCreator.path.GetPointAtDistance(totalDistance, EndOfPathInstruction.Stop); //EndOfPathInstruction.Stop just tells our Gremlin to stop when it reaches the end of the path.
                activeGremlin.transform.position.Scale(TerrainVariant.positionFunction(timePassed));
                timePassed += Time.fixedDeltaTime;
            }
        }
    }
}
