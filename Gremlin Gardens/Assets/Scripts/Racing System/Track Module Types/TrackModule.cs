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
    // TODO: Reverse indexing option? I can't believe I've never realized this before.
    // Basically, the problem is that sometimes the PathCreator will sometimes put the gremlins in reverse of how the designer intends the path to be layed out.
    // If I ever have to fix a problem like this again, just make an option in TrackModule to reverse the index it has for the path, or something like that.

    /// <summary>
    /// How fast the Gremlin moves in units per frame. Does NOT affect animation speed (see TerrainVariant instead).
    /// </summary>
    [Tooltip("How fast the Gremlin moves in units per fixed frame count. Does NOT affect animation speed (see TerrainVariant instead).")]
    public float BaseSpeed = .5f;

    /// <summary>
    /// How much QTEs should be able to affect the gremlin's speed.
    /// </summary>
    [Tooltip("How much QTEs should be able to affect the gremlin's speed.")]
    public float QTEWeight = 0.1f;

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
    /// Not static because it may be changed by EquationTrackModule.
    /// </summary>
    [HideInInspector]
    public Vector3 pathStart;
    /// <summary>
    /// Position where the bezier path for this module ends. Intended to be used when constructing a track procedurally, to connect each point.
    /// Not static because it may be changed by EquationTrackModule.
    /// </summary>
    [HideInInspector]
    public Vector3 pathEnd;

    /// <summary>
    /// How much the gremlin has moved along the track module.
    /// </summary>
    [HideInInspector]
    public float totalDistance;

    [Header("Camera Settings")]


    ///<summary>
    /// Should this track module affect the camera while it's racing?
    /// </summary>
    public bool affectCamera = true;

    ///<summary>
    /// Used when the camera is watching a gremlin during a race. If zero, the camera will not use this. Any other value will cause the camera to change position to the new offset (based on the TrackModule's position).
    /// </summary>
    [Tooltip("Used when the camera is watching a gremlin during a race. If zero, the camera will not use this. Any other value will cause the camera to change position to the new offset (based on the TrackModule's position).")]
    public Vector3 optimalCameraOffset;

    /// <summary>
    /// Should the camera be fixed when the Gremlin is on this Track Module?
    /// </summary>
    [Tooltip("Should the camera be fixed when the Gremlin is on this Track Module?")]
    public bool optimalOffsetIsFixed = false;

    /// <summary>
    /// Should the camera immediately cut to the offset, or tween?
    /// </summary>
    [Tooltip("Should the camera immediately cut to the offset, or tween?")]
    public bool cameraImmediateCut = false;

    /// <summary>
    /// Should we offset the camera by the number of the tracks? Basically, if the Tracks are placed along the z-axis, do we want the camera to be stretched along the Z-axis based on however many tracks there are?
    /// </summary>
    [Tooltip("Should we offset the camera by the number of the tracks? Basically, if the Tracks are placed along the z-axis, do we want the camera to be stretched along the Z-axis based on however many tracks there are?")]
    public bool multiplyOffsetByNumTracks = false;


    [Header("Camera Flyover")]

    /// <summary>
    /// When doing a flyover, should the camera instead just go from the start to the end of the path?
    /// </summary>
    [Tooltip("When doing a flyover, should the camera instead just go from the start to the end of the path?")]
    public bool cameraIgnorePath = false;
    /// <summary>
    /// When doing a flyover, should the camera rotate with the path?
    /// </summary>
    [Tooltip("When doing a flyover, should the camera rotate with the path?")]
    public bool cameraShouldRotate = true;

    /// <summary>
    /// If a camera is doing a flyover, skip ahead how many modules when this is done? (Can be negative in case the camera is going over the track in reverse)
    /// </summary>
    [Tooltip("If a camera is doing a flyover, skip ahead how many modules when this is done? (Can be negative in case the camera is going over the track in reverse)")]
    public int cameraSkipAhead = 0;

    PathCreator internalCreator;
    
    void Awake()
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
    [HideInInspector]
    public GremlinObject activeGremlin; //Class name subject to change?
    [HideInInspector]
    public float timePassed;
    [HideInInspector]
    public Vector3 gOffset;
    /// <summary>
    /// The QTE object pulled from terrainVariant.
    /// </summary>
    GameObject qteObject;
    public delegate void Callback();
    /// <summary>
    /// A callback called at the end of the move.
    /// </summary>
    Callback toCallback;

    public SettingsMenu settings;
    /// <summary>
    /// Start moving the Gremlin across the track module.
    /// </summary>
    /// <param name="gremlin">The Gremlin that's being moved.</param>
    /// <param name="gremlinOffset">The offset of the gremlin (see: TrackManager.GremlinOffset).</param>
    /// <param name="callbackFunc">The function that TrackManager will pass to callback to later.</param>
    public void BeginMove(GremlinObject gremlin, Vector3 gremlinOffset, Callback callbackFunc, GameObject UI, SettingsMenu s_menu) {
        gremlinMoving = true;
        activeGremlin = gremlin;
        gOffset = gremlinOffset;
        modifiedSpeed = terrainVariant.relativeSpeed(activeGremlin, this);
        timePassed = 0.0f;
        totalDistance = 0;
        toCallback = callbackFunc;
        if (terrainVariant.QTEButton != null && this.GetComponentInParent<TrackManager>().isPlayerTrack) {
            qteObject = Instantiate(terrainVariant.QTEButton, UI.transform);
            qteObject.GetComponent<QTEScript>().SetActiveModule(this);
        }
        settings = s_menu;
    }

    public void EndMove() {
        gremlinMoving = false;
        if (qteObject != null)
        {
            Destroy(qteObject);
        }
        toCallback();
    }

    /// <summary>
    /// Set modified speed with the terrainVariant, but also set it with the QTE script, if you need that sort of thing.
    /// </summary>
    public void SetModifiedSpeed() {
        modifiedSpeed = terrainVariant.relativeSpeed(activeGremlin, this);
        if (qteObject != null) {
            modifiedSpeed += modifiedSpeed * QTEWeight * qteObject.GetComponent<QTEScript>().ModifySpeed();
        }
    }

    void Update()
    {
        if (gremlinMoving && !settings.paused) { //Move the Gremlin around.
            totalDistance += modifiedSpeed * BaseSpeed * Time.deltaTime; //Keeping track of how far along the Gremlin is in this module.
            if (totalDistance >= internalCreator.path.length)
            {
                EndMove();
            }
            else
            { //Move the Gremlin. We mutliply timePassed by modifiedSpeed to change the speed at which the offset changes (since the speed of the animation also affects the offset).
                SetModifiedSpeed(); //Set modifiedSpeed again in case it's somehow changed.
                activeGremlin.transform.position = internalCreator.path.GetPointAtDistance(totalDistance, EndOfPathInstruction.Stop) + terrainVariant.positionFunction(timePassed * modifiedSpeed, this) + gOffset; //EndOfPathInstruction.Stop just tells our Gremlin to stop when it reaches the end of the path.
                Vector3 nextPos = internalCreator.path.GetPointAtDistance(totalDistance + 0.01f, EndOfPathInstruction.Stop);
                activeGremlin.transform.rotation = Quaternion.LookRotation(new Vector3(nextPos.x, activeGremlin.transform.position.y, nextPos.z) - activeGremlin.transform.position, Vector3.up);
                timePassed += Time.deltaTime;
            }
        }
    }
}
