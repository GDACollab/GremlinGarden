using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A camera script designed to be used before, during, and after racing scenes.
/// </summary>
public class RacingCamera : MonoBehaviour
{
    [Header("Racing Mode Settings")]
    /// <summary>
    /// How far away a gremlin has to be before the camera can start moving. Only used in the camera's racing mode.
    /// </summary>
    [Tooltip("How far away a gremlin has to be before the camera can start moving. Only used in the camera's racing mode.")]
    public Vector3 gremlinBounds = new Vector3(50, 50, 50);

    /// <summary>
    /// How far away the camera is from the gremlin. Set this ahead of time, and then call the relevant functions to change how the camera offsets.
    /// </summary>
    [Tooltip("How far away the camera is from the gremlin.")]
    public Vector3 cameraOffset = new Vector3(0, 1, 1);


    /// <summary>
    /// Should the camera also look at the gremlin? Only used in the camera's racing mode.
    /// </summary>
    [Tooltip("Should the camera also look at the gremlin? Only used in the camera's racing mode.")]
    public bool lookAtFocus = true;

    /// <summary>
    /// Should the camera move to the gremlin? Only used in the camera's racing mode.
    /// </summary>
    [Tooltip("Should the camera move to the gremlin? Only used in the camera's racing mode.")]
    public bool enableMovement = true;

    public delegate void Callback();
    /// <summary>
    /// Used when a camera is done with a mode. Modes are intended to only be used one at a time, so don't set up multiple camera modes at once.
    /// </summary>
    Callback cameraStuffFinishedCallback;

    /// <summary>
    /// Used to switch between the various camera "modes", like previewing a track or following a gremlin.
    /// </summary>
    [HideInInspector]
    public string cameraMode = "none";

    /// <summary>
    /// The gremlin the camera is currently focusing on (if at all).
    /// </summary>
    [HideInInspector]
    public GameObject gremlinFocus = null;
    /// <summary>
    /// The track of the gremlinFocus.
    /// </summary>
    [HideInInspector]
    public TrackManager gremlinTrack = null;


    /// <summary>
    /// Sets the gremlin the camera is currently tracking. Please do not use multiple SetX() functions at once, it will break the camera.
    /// </summary>
    /// <param name="gremlin">The gremlin to track.</param>
    /// <param name="track">The track the gremlin is on.</param>
    /// <param name="updatePos">Should the camera immediately jump to this gremlin?</param>
    public void SetGremlinFocus(GameObject gremlin, TrackManager track, bool updatePos) {
        gremlinFocus = gremlin;
        gremlinTrack = track;
        cameraMode = "racing";
        if (updatePos == true) {
            this.transform.position = gremlinFocus.transform.position + cameraOffset;
            if (lookAtFocus == true)
            {
                this.transform.rotation = Quaternion.LookRotation(gremlinFocus.transform.position - this.transform.position, Vector3.up);
            }
        }
    }

    //Stuff for flyover:
    /// <summary>
    /// How fast the camera is gonna fly (used ONLY for flyovers)
    /// </summary>
    [Header("Flyover Settings"), Tooltip("How fast the camera is gonna fly (used ONLY for flyovers)")]
    public float cameraFlySpeed = 5.0f;
    /// <summary>
    /// The track the camera is focusing on (if at all).
    /// </summary>
    TrackManager trackFocus;
    /// <summary>
    /// The module the camera is currently on (used during flyover).
    /// </summary>
    int currentModule;
    /// <summary>
    /// How far along the camera is on any given track.
    /// </summary>
    float cameraTrackProgress = 0;
    /// <summary>
    /// What direction the camera is travelling during a flyover.
    /// </summary>
    int cameraTrackDirection = 0;
    /// <summary>
    /// If the camera is currently skipping over a module during a flyover.
    /// </summary>
    bool isSkipping = false;
    /// <summary>
    /// Used for skipping over modules in the flyover.
    /// </summary>
    Vector3 originalPos;

    /// <summary>
    /// Start the camera flying over the track. Please do not use multiple SetX() functions at once, it will break the camera.
    /// </summary>
    /// <param name="trackToFly">The track to fly over.</param>
    /// <param name="direction">-1 from starting at the end, 1 from starting at the beginning.</param>
    /// <param name="updatePos">Should the camera immediately jump to the track?</param>
    /// <param name="onFinishCallback">The callback to call when the flyover is done.</param>
    public void SetFlyover(TrackManager trackToFly, int direction, bool updatePos, Callback onFinishCallback) {
        trackFocus = trackToFly;
        cameraMode = "flyover";
        cameraTrackDirection = direction;
        cameraStuffFinishedCallback = onFinishCallback;
        if (direction == 1)
        {
            cameraTrackProgress = 0;
            currentModule = 0;
            if (updatePos == true)
            {
                this.transform.position = trackFocus.transform.GetChild(currentModule).GetComponent<TrackModule>().GetComponent<PathCreation.PathCreator>().path.GetPointAtDistance(0) + cameraOffset;
                Vector3 next = trackFocus.transform.GetChild(currentModule).GetComponent<TrackModule>().GetComponent<PathCreation.PathCreator>().path.GetPointAtDistance(cameraFlySpeed) + cameraOffset - this.transform.position;
                this.transform.rotation = Quaternion.LookRotation(next, Vector3.up);
            }
        } else if (direction == -1){
            currentModule = trackFocus.transform.childCount - 1;
            cameraTrackProgress = trackFocus.transform.GetChild(currentModule).GetComponent<TrackModule>().GetComponent<PathCreation.PathCreator>().path.length;
            if (updatePos == true)
            {
                this.transform.position = trackFocus.transform.GetChild(currentModule).GetComponent<TrackModule>().GetComponent<PathCreation.PathCreator>().path.GetPointAtDistance(trackFocus.transform.GetChild(currentModule).GetComponent<TrackModule>().GetComponent<PathCreation.PathCreator>().path.length, PathCreation.EndOfPathInstruction.Stop) + cameraOffset;
                Vector3 next = trackFocus.transform.GetChild(currentModule).GetComponent<TrackModule>().GetComponent<PathCreation.PathCreator>().path.GetPointAtDistance(trackFocus.transform.GetChild(currentModule).GetComponent<TrackModule>().GetComponent<PathCreation.PathCreator>().path.length - cameraFlySpeed, PathCreation.EndOfPathInstruction.Stop) + cameraOffset - this.transform.position;
                this.transform.rotation = Quaternion.LookRotation(next, Vector3.up);
            }
        }
    }

    ///<summary>
    /// Used in wipes. We assign a camera to have this RenderTexture, and the camera outputs what it sees to wipeTexture. We then display the texture on a plane.
    /// </summary>
    RenderTexture wipeTexture;
    /// <summary>
    /// A prefab of the plane to wipe with (will assign a RenderTexture as the main texture). Just search for TexturePlane in prefabs.
    /// </summary>
    [Header("Stuff for Camera Wipes"), Tooltip("A prefab of the plane to wipe with (will assign a RenderTexture as the main texture). Just search for TexturePlane in prefabs.")]
    public GameObject texturePlane;
    /// <summary>
    /// The actual plane we're going to wipe with.
    /// </summary>
    GameObject planeToWipeWith;
    /// <summary>
    /// Where we want the wipe to end.
    /// </summary>
    Vector3 wipeEndPos;
    /// <summary>
    /// This is the transform of the camera we're wiping to. It's an illusion basically. We just move this camera over to where the other camera is after we've overlayed the plane.
    /// </summary>
    Transform transferToCamera;
    /// <summary>
    /// How fast we're gonna wipe.
    /// </summary>
    float wipeSpeed;

    /// <summary>
    /// Start the camera with a wipe. To use this, make sure you have a suitable prefab for texturePlane. Please do not use multiple SetX() functions at once, it will break the camera.
    /// </summary>
    /// <param name="cameraToWipe">The camera we're going to use as a texture for where to wipe to.</param>
    /// <param name="ActiveUI">The currently active UI (to overlay the other camera's perspective as a "wipe").</param>
    /// <param name="startAt">Where the wipe texture is going to start on the canvas.</param>
    /// <param name="endAt">Where the wipe texture is going to move to on the canvas.</param>
    /// <param name="speed">How fast the wipe is going to move on the canvas.</param>
    /// <param name="callback">The function to call when the wipe is finished.</param>
    public void SetWipe(Camera cameraToWipe, GameObject ActiveUI, Vector3 startAt, Vector3 endAt, float speed, Callback callback = null) {
        //We create a new RenderTexture because the size of the camera might differ in different platforms. We have a RenderTexture already in place for the camera
        //because webGL freaks out if there isn't.
        wipeTexture = new RenderTexture(Screen.width, Screen.height, 24);
        planeToWipeWith = Instantiate(texturePlane, ActiveUI.transform);
        cameraToWipe.targetTexture = wipeTexture;
        planeToWipeWith.GetComponent<UnityEngine.UI.Image>().material.SetTexture("_MainTex", wipeTexture);
        Vector3 newScale = new Vector3(Screen.width, Screen.height);
        planeToWipeWith.transform.localScale = newScale;
        planeToWipeWith.transform.position = startAt;
        wipeEndPos = endAt;
        cameraMode = "wipe";
        cameraStuffFinishedCallback = callback;
        transferToCamera = cameraToWipe.transform;
        wipeSpeed = speed;
    }

    //Stuff for tweens (Sorry, people aged 14 and older, and kids aged 8 and younger):

    /// <summary>
    /// Where we're tweening to.
    /// </summary>
    Vector3 targetPos;
    Vector3 targetRot;
    /// <summary>
    /// Where we're tweening from.
    /// </summary>
    Vector3 oldPos;
    Vector3 oldRot;
    /// <summary>
    /// How much time has passed since the start of the tween.
    /// </summary>
    float tweenElapsed;
    /// <summary>
    /// How long the tween is supposed to last.
    /// </summary>
    float tweenDuration;
    /// <summary>
    /// The curve that determines the speed of the tween. Use this to all of its easing goodness. 0 means we're at 0% of what we're tweening towards. 1 is 100% of what we're tweening towards.
    /// Okay, I tried to get this to be input as a parameter for the SetTween function, but Unity was stubborn about this being provided as an Input.
    /// So instead, it's going to have to be something you set ahead of time, and then call SetTween for.
    /// </summary>
    [Header("Tweening Settings"), Tooltip("The curve that determines the speed of the tween. Use this to all of its easing goodness. 0 means we're at 0% of what we're tweening towards. 1 is 100% of what we're tweening towards.")]
    public AnimationCurve tweenCurve = AnimationCurve.Linear(0, 0, 1, 1);

    /// <summary>
    /// Sets the camera to tween to a new position. Uses tweenCurve as an easing function. Please do not use multiple SetX() functions at once, it will break the camera.
    /// </summary>
    /// <param name="newPos">The new position for the camera to tween to.</param>
    /// <param name="newRot">The new rotation for the camera to tween to.</param>
    /// <param name="time">How long the tween should be.</param>
    /// <param name="callback">The function to call when the tween is finished.</param>
    public void SetTween(Vector3 newPos, Quaternion newRot, float time, Callback callback = null) {
        cameraMode = "tween";
        targetPos = newPos;
        targetRot = newRot.eulerAngles;
        oldPos = this.transform.position;
        oldRot = this.transform.rotation.eulerAngles;
        tweenDuration = time;
        tweenElapsed = 0;
        cameraStuffFinishedCallback = callback;
    }

    void Update()
    {
        if (cameraMode == "racing")
        {
            if (enableMovement)
            {
                Vector3 newPos = this.transform.position;
                for (int i = 0; i < 3; i++)
                {
                    if (Mathf.Abs(gremlinFocus.transform.position[i] + cameraOffset[i] - this.transform.position[i]) > gremlinBounds[i])
                    {
                        var direction = Mathf.Sign(gremlinFocus.transform.position[i] + cameraOffset[i] - this.transform.position[i]);
                        // We subtract 1 from .currentChild because TrackManager increments currentChild after calling beginMove during the Race function. So basically .currentChild is the child in the future. Technically.
                        var track = gremlinTrack.transform.GetChild(gremlinTrack.currentChild - 1).GetComponent<TrackModule>();
                        //I can't use track.modifiedSpeed for... some reason.
                        newPos[i] += track.terrainVariant.relativeSpeed(gremlinFocus.GetComponent<GremlinObject>(), track) * track.BaseSpeed * direction;
                    }
                }
                this.transform.position = Vector3.Lerp(this.transform.position, newPos, Time.deltaTime);
            }
            if (lookAtFocus)
            {
                //We round so that subtle movements don't impact rotation, causing motion sickness.
                Vector3 newRotation = new Vector3(Mathf.Round(gremlinFocus.transform.position.x - this.transform.position.x), Mathf.Round(gremlinFocus.transform.position.y - this.transform.position.y), Mathf.Round(gremlinFocus.transform.position.z - this.transform.position.z));
                this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(newRotation, Vector3.up), Time.deltaTime);
            }
        }
        else if (cameraMode == "flyover")
        { //The logic for doing a flyover.
            PathCreation.PathCreator flyoverPath = trackFocus.transform.GetChild(currentModule).GetComponent<TrackModule>().GetComponent<PathCreation.PathCreator>();
            int skipAhead = trackFocus.transform.GetChild(currentModule).GetComponent<TrackModule>().cameraSkipAhead;
            if (cameraTrackDirection == 1 && cameraTrackProgress >= flyoverPath.path.length) //This if/else if is just to move along the path if we're doing one direction or the other.
            {
                if (skipAhead > 0)
                {
                    isSkipping = true;
                    originalPos = this.transform.position;
                    currentModule += skipAhead;
                }
                currentModule += 1;
                cameraTrackProgress = 0;
                if (currentModule >= trackFocus.transform.childCount)
                {
                    cameraMode = "none";
                    cameraStuffFinishedCallback();
                }
            }
            else if (cameraTrackDirection == -1 && cameraTrackProgress <= 0)
            {
                if (skipAhead < 0)
                {
                    isSkipping = true;
                    originalPos = this.transform.position;
                    currentModule += skipAhead;
                }
                currentModule -= 1;
                if (currentModule < 0)
                {
                    cameraMode = "none";
                    cameraStuffFinishedCallback();
                }
                cameraTrackProgress = trackFocus.transform.GetChild(currentModule).GetComponent<TrackModule>().GetComponent<PathCreation.PathCreator>().path.length;
            }
            else
            { //The else is for actually moving us on the path.
                Vector3 newPos = flyoverPath.path.GetPointAtDistance(cameraTrackProgress); //this.transform.position's soon to be new value.
                Vector3 nextPos = Vector3.zero; //nextPos is what's just after newPos (used for updating rotation).
                if (cameraTrackDirection == 1)
                { //Are we moving on the path regularly?
                    nextPos = flyoverPath.path.GetPointAtDistance(cameraTrackProgress + cameraFlySpeed);
                }
                else if (cameraTrackDirection == -1)
                {
                    nextPos = flyoverPath.path.GetPointAtDistance(cameraTrackProgress - cameraFlySpeed);
                }
                if (trackFocus.transform.GetChild(currentModule).GetComponent<TrackModule>().cameraIgnorePath)
                { //If not, we're moving from the start point to the end point, so that's reflected here.
                    Vector3 start = flyoverPath.path.GetPointAtDistance(0);
                    Vector3 end = flyoverPath.path.GetPointAtDistance(flyoverPath.path.length, PathCreation.EndOfPathInstruction.Stop);
                    Vector3 followLine = end - start;
                    newPos = flyoverPath.path.GetPointAtDistance(0) + (followLine * (cameraTrackProgress/Vector3.Distance(start, end)));
                    nextPos = flyoverPath.path.GetPointAtDistance(0) + (followLine * (cameraTrackProgress + cameraFlySpeed/flyoverPath.path.length));
                }
                if (isSkipping)
                { //Okay, but have we skipped over some modules? If so, start slowly moving over to the next available module.
                    Vector3 target = (newPos + cameraOffset - originalPos);
                    target.Normalize();
                    this.transform.position += target * (cameraFlySpeed);
                    if (Vector3.Distance(this.transform.position, newPos + cameraOffset) <= cameraFlySpeed)
                    {
                        isSkipping = false;
                    }
                }
                else
                { //Otherwise, do all that .Lerping goodness.
                    this.transform.position = Vector3.Lerp(cameraOffset + newPos, this.transform.position, Time.deltaTime);
                    if (cameraTrackDirection == -1)
                    {
                        cameraTrackProgress -= cameraFlySpeed;
                    }
                    else
                    {
                        cameraTrackProgress += cameraFlySpeed;
                    }
                }
                if (trackFocus.transform.GetChild(currentModule).GetComponent<TrackModule>().cameraShouldRotate)
                { //Just for updating rotation.
                    Vector3 newRotation = Vector3.zero;
                    if (cameraTrackDirection == 1)
                    {
                        newRotation = nextPos - newPos;
                    }
                    else if (cameraTrackDirection == -1)
                    {
                        newRotation = newPos - nextPos;
                    }
                    this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(newRotation, Vector3.up), Time.deltaTime);
                }
            }
        }
        else if (cameraMode == "wipe")
        {
            Vector3 target = (wipeEndPos - planeToWipeWith.transform.position);
            target.Normalize();
            target *= wipeSpeed; //This actually moves the camera during a wipe, so if you want to change the speed, do it here.
            target += planeToWipeWith.transform.position;
            planeToWipeWith.transform.position = Vector3.Lerp(target, planeToWipeWith.transform.position, Time.deltaTime);
            if (Vector3.Distance(planeToWipeWith.transform.position, wipeEndPos) <= wipeSpeed)
            {
                Destroy(planeToWipeWith);
                this.transform.position = transferToCamera.position;
                this.transform.rotation = transferToCamera.rotation;
                cameraMode = "none";
                cameraStuffFinishedCallback();
            }
        }
        else if (cameraMode == "tween") {
            var percent = tweenCurve.Evaluate(tweenElapsed/tweenDuration);
            this.transform.position = oldPos + ((targetPos - oldPos) * percent);
            this.transform.rotation = Quaternion.Euler(oldRot + ((targetRot - oldRot) * percent));
            if (tweenElapsed >= tweenDuration) {
                cameraMode = "none";
                cameraStuffFinishedCallback();
            } 
            tweenElapsed += Time.deltaTime;
        }
    }
}
