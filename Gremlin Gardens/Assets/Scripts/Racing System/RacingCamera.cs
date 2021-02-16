﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The bare basics of what will soon be the ultimate racing camera the world has ever seen.
//Things to maybe implement in the future:
//Make modules able to tweak the camera's settings based on "optimal" settings?
public class RacingCamera : MonoBehaviour
{
    /// <summary>
    /// How far away a gremlin has to be before the camera can start moving.
    /// </summary>
    [Tooltip("How far away a gremlin has to be before the camera can start moving.")]
    public Vector3 gremlinBounds = new Vector3(50, 50, 50);

    /// <summary>
    /// How far away the camera is from the gremlin.
    /// </summary>
    [Tooltip("How far away the camera is from the gremlin.")]
    public Vector3 cameraOffset = new Vector3(0, 1, 1);


    /// <summary>
    /// Should the camera also look at the gremlin?
    /// </summary>
    [Tooltip("Should the camera also look at the gremlin?")]
    public bool lookAtFocus = true;

    public float cameraFlySpeed = 5.0f;


    /// <summary>
    /// The gremlin the camera is currently focusing on (if at all).
    /// </summary>
    GameObject gremlinFocus;
    /// <summary>
    /// The track the camera is focusing on (if at all).
    /// </summary>
    TrackManager trackFocus;
    /// <summary>
    /// The module the camera is currently on (used during flyover).
    /// </summary>
    int currentModule;
    /// <>
    /// How far along the camera is on any given track.
    /// </summary>
    float cameraTrackProgress = 0;
    int cameraTrackDirection = 0;
    bool isSkipping = false;
    Vector3 originalPos;

    /// <summary>
    /// Used to switch between the various camera "modes", like previewing a track or following a gremlin.
    /// </summary>
    [HideInInspector]
    public string cameraMode = "racing";


    /// <summary>
    /// Sets the gremlin the camera is currently tracking.
    /// </summary>
    /// <param name="gremlin">The gremlin to track.</param>
    /// <param name="updatePos">Should the camera immediately jump to this gremlin?</param>
    public void SetGremlinFocus(GameObject gremlin, bool updatePos) {
        gremlinFocus = gremlin;
        cameraMode = "racing";
        if (updatePos == true) {
            this.transform.position = gremlinFocus.transform.position + cameraOffset;
            if (lookAtFocus == true)
            {
                this.transform.rotation = Quaternion.LookRotation(gremlinFocus.transform.position - this.transform.position, Vector3.up);
            }
        }
    }

    /// <summary>
    /// Start the camera flying over the track.
    /// </summary>
    /// <param name="trackToFly">The track to fly over.</param>
    /// <param name="direction">-1 from starting at the end, 1 from starting at the beginning.</param>
    public void SetFlyover(TrackManager trackToFly, int direction) {
        trackFocus = trackToFly;
        cameraMode = "flyover";
        cameraTrackDirection = direction;
        if (direction == 1)
        {
            cameraTrackProgress = 0;
            currentModule = 0;
            this.transform.position = trackFocus.transform.GetChild(currentModule).GetComponent<TrackModule>().GetComponent<PathCreation.PathCreator>().path.GetPointAtDistance(0) + cameraOffset;
        }
        else {
            currentModule = trackFocus.transform.childCount - 1;
            cameraTrackProgress = trackFocus.transform.GetChild(currentModule).GetComponent<TrackModule>().GetComponent<PathCreation.PathCreator>().path.length;
            this.transform.position = trackFocus.transform.GetChild(currentModule).GetComponent<TrackModule>().GetComponent<PathCreation.PathCreator>().path.GetPointAtDistance(trackFocus.transform.GetChild(currentModule).GetComponent<TrackModule>().GetComponent<PathCreation.PathCreator>().path.length, PathCreation.EndOfPathInstruction.Stop) + cameraOffset;
        }
    }

    void Update()
    {
        if (cameraMode == "racing")
        {
            Vector3 newPos = this.transform.position;
            for (int i = 0; i < 3; i++)
            {
                if (Mathf.Abs(gremlinFocus.transform.position[i] - this.transform.position[i]) > gremlinBounds[i])
                {
                    newPos[i] = gremlinFocus.transform.position[i] + cameraOffset[i];
                }
            }
            if (lookAtFocus == true)
            {
                //We round so that subtle movements don't impact rotation, causing motion sickness.
                Vector3 newRotation = new Vector3(Mathf.Round(gremlinFocus.transform.position.x - this.transform.position.x), Mathf.Round(gremlinFocus.transform.position.y - this.transform.position.y), Mathf.Round(gremlinFocus.transform.position.z - this.transform.position.z));
                this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(newRotation, Vector3.up), Time.deltaTime);
            }
            this.transform.position = Vector3.Lerp(this.transform.position, newPos, Time.deltaTime);
        }
        else if (cameraMode == "flyover") { //TODO: Add "Skip" to TrackModule for camera flyovers.
            PathCreation.PathCreator flyoverPath = trackFocus.transform.GetChild(currentModule).GetComponent<TrackModule>().GetComponent<PathCreation.PathCreator>();
            if (cameraTrackDirection == 1)
            {
                if (cameraTrackProgress >= flyoverPath.path.length)
                {
                    if (trackFocus.transform.GetChild(currentModule).GetComponent<TrackModule>().cameraSkipAhead > 0)
                    {
                        isSkipping = true;
                        originalPos = this.transform.position;
                        currentModule += trackFocus.transform.GetChild(currentModule).GetComponent<TrackModule>().cameraSkipAhead;
                    }
                    currentModule += 1;
                    cameraTrackProgress = 0;
                }
                else
                {
                    Vector3 newPos = flyoverPath.path.GetPointAtDistance(cameraTrackProgress);
                    Vector3 nextPos = flyoverPath.path.GetPointAtDistance(cameraTrackProgress + cameraFlySpeed);
                    if (trackFocus.transform.GetChild(currentModule).GetComponent<TrackModule>().cameraIgnorePath) {
                        Vector3 followLine = flyoverPath.path.GetPointAtDistance(flyoverPath.path.length, PathCreation.EndOfPathInstruction.Stop) - flyoverPath.path.GetPointAtDistance(0);
                        followLine.Normalize();
                        newPos = flyoverPath.path.GetPointAtDistance(0) + (cameraTrackProgress * followLine);
                        nextPos = flyoverPath.path.GetPointAtDistance(0) + (followLine * (cameraTrackProgress + cameraFlySpeed));
                    }
                    if (isSkipping) {
                        Vector3 target = (newPos + cameraOffset - originalPos);
                        target.Normalize();
                        this.transform.position += target / (4000 * cameraFlySpeed); //Speed is arbitrary
                        if (Vector3.Distance(this.transform.position, newPos + cameraOffset) < 0.1f) {
                            isSkipping = false;
                        }
                    } else {
                        this.transform.position = Vector3.Lerp(cameraOffset + newPos, this.transform.position, Time.deltaTime);
                        cameraTrackProgress += cameraFlySpeed;
                    }
                    if (trackFocus.transform.GetChild(currentModule).GetComponent<TrackModule>().cameraShouldRotate) {
                        Vector3 newRotation = nextPos - newPos;
                        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(newRotation, Vector3.up), Time.deltaTime);
                    }
                }
            } else
            {
                if (cameraTrackProgress <= 0)
                {
                    if (trackFocus.transform.GetChild(currentModule).GetComponent<TrackModule>().cameraSkipAhead < 0)
                    {
                        isSkipping = true;
                        originalPos = this.transform.position;
                        currentModule += trackFocus.transform.GetChild(currentModule).GetComponent<TrackModule>().cameraSkipAhead;
                    }
                    currentModule -= 1;
                    cameraTrackProgress = trackFocus.transform.GetChild(currentModule).GetComponent<TrackModule>().GetComponent<PathCreation.PathCreator>().path.length;
                }
                else
                {
                    Vector3 newPos = flyoverPath.path.GetPointAtDistance(cameraTrackProgress);
                    Vector3 nextPos = flyoverPath.path.GetPointAtDistance(cameraTrackProgress - cameraFlySpeed);
                    if (trackFocus.transform.GetChild(currentModule).GetComponent<TrackModule>().cameraIgnorePath)
                    {
                        Vector3 followLine =  flyoverPath.path.GetPointAtDistance(flyoverPath.path.length, PathCreation.EndOfPathInstruction.Stop) - flyoverPath.path.GetPointAtDistance(0);
                        followLine.Normalize();
                        newPos = flyoverPath.path.GetPointAtDistance(0) + (cameraTrackProgress * followLine);
                        nextPos = flyoverPath.path.GetPointAtDistance(0) + (followLine * (cameraTrackProgress + cameraFlySpeed));
                    }
                    if (isSkipping)
                    {
                        Vector3 target = (newPos + cameraOffset - originalPos);
                        target.Normalize();
                        this.transform.position += target / (10000 * cameraFlySpeed);
                        if (Vector3.Distance(this.transform.position, newPos + cameraOffset) < 0.1f)
                        {
                            isSkipping = false;
                        }
                    }
                    else
                    {
                        this.transform.position = Vector3.Lerp(cameraOffset + newPos, this.transform.position, Time.deltaTime);
                        cameraTrackProgress -= cameraFlySpeed;
                    }
                    if (trackFocus.transform.GetChild(currentModule).GetComponent<TrackModule>().cameraShouldRotate)
                    {
                        Vector3 newRotation =   nextPos - newPos;
                        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(newRotation, Vector3.up), Time.deltaTime);
                    }
                }
            }
        }
    }
}
