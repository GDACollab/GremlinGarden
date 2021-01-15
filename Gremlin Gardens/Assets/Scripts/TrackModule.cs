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
    /// How fast the Gremlin moves in units per fixed frame count. Does NOT affect animation speed (see TerrainVariant instead).
    /// </summary>
    [Tooltip("How fast the Gremlin moves in units per fixed frame count. Does NOT affect animation speed (see TerrainVariant instead).")]
    public float BaseSpeed = .5f;

    /// <summary>
    /// The animation name to play from the Animator for this TrackModule.
    /// </summary>
    [Tooltip("The animation name to play from the Animator for this TrackModule.")]
    public string AnimationToPlay;

    /// <summary>
    /// Get a GameObject's TerrainVariant data (how to move, how fast to move). Be sure to give an empty object a TerrainVariant script (or a script that extends TerrainVariant).
    /// </summary>
    [Tooltip("Get a GameObject's TerrainVariant data (how to move, how fast to move). Be sure to give an empty object a TerrainVariant script (or a script that extends TerrainVariant).")]
    public TerrainVariant terrainVariant;

    /// <summary>
    /// Is the gremlin moving across this track module?
    /// </summary>
    [HideInInspector]
    public bool gremlinMoving = false;

    /// <summary>
    /// Position where the bezier path for this module starts. Intended to be used when constructing a track procedurally, to connect each point.
    /// </summary>
    public static Vector3 pathStart;
    /// <summary>
    /// Position where the bezier path for this module ends. Intended to be used when constructing a track procedurally, to connect each point.
    /// </summary>
    public static Vector3 pathEnd;

    /// <summary>
    /// How much the gremlin has moved along the track module.
    /// </summary>
    float totalDistance;
    PathCreator internalCreator;
    
    void Start()
    {
        totalDistance = 0;
        internalCreator = GetComponent<PathCreator>();
        pathStart = internalCreator.path.GetPoint(0);
        pathEnd = internalCreator.path.GetPoint(internalCreator.path.NumPoints - 1);
    }

    /// <summary>
    /// Percentage of the base speed that the gremlin's going to move at.
    /// </summary>
    [HideInInspector]
    public float modifiedSpeed;
    Gremlin activeGremlin; //Class name subject to change?
    float timePassed;
    Vector3 gOffset;
    public delegate void Callback();
    /// <summary>
    /// A callback called at the end of the move.
    /// </summary>
    Callback toCallback;
    /// <summary>
    /// Start moving the Gremlin across the track module.
    /// </summary>
    /// <param name="gremlin">The Gremlin that's being moved.</param>
    /// <param name="gremlinOffset">The offset of the gremlin (see: TrackManager.GremlinOffset).</param>
    /// <param name="callbackFunc">The function that TrackManager will pass to callback to later.</param>
    public void BeginMove(Gremlin gremlin, Vector3 gremlinOffset, Callback callbackFunc) {
        gremlinMoving = true;
        activeGremlin = gremlin;
        gOffset = gremlinOffset;
        modifiedSpeed = terrainVariant.relativeSpeed(activeGremlin);
        timePassed = 0.0f;
        totalDistance = 0;
        toCallback = callbackFunc;
    }

    private void EndMove() {
        gremlinMoving = false;
        toCallback();
    }

    void FixedUpdate()
    {
        if (gremlinMoving) { //Move the Gremlin around.
            totalDistance += modifiedSpeed * BaseSpeed * Time.fixedDeltaTime; //Keeping track of how far along the Gremlin is in this module.
            if (totalDistance >= internalCreator.path.length)
            {
                EndMove();
            }
            else
            { //Move the Gremlin. We mutliply timePassed by modifiedSpeed to change the speed at which the offset changes (since the speed of the animation also affects the offset).
                activeGremlin.transform.position = internalCreator.path.GetPointAtDistance(totalDistance, EndOfPathInstruction.Stop) + terrainVariant.positionFunction(timePassed * modifiedSpeed) + gOffset; //EndOfPathInstruction.Stop just tells our Gremlin to stop when it reaches the end of the path.
                timePassed += Time.fixedDeltaTime;
            }
        }
    }
}
