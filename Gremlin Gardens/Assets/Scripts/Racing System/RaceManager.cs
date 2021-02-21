using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is in charge of making multiple tracks populated with gremlins. See RandomGremlinRace.cs for an example of how to use this.
/// </summary>
public class RaceManager : MonoBehaviour
{

    /// <summary>
    /// The active canvas object to be used by QTE prompts.
    /// </summary>
    [Tooltip("The active canvas object to be used by QTE prompts.")]
    public GameObject ActiveUI;
    [Header("Racetrack Generation")]
    /// <summary>
    /// The player's version of the track (for QTE stuff). The track object requires a TrackManager component.
    /// </summary>
    [Tooltip("The player's version of the track (for QTE stuff). The track object requires a TrackManager component.")]
    public GameObject playerTrack;
    /// <summary>
    /// The AI's version of the track (for stuff without QTEs). The track object requires a TrackManager component.
    /// </summary>
    [Tooltip("The AI's version of the track (for stuff without QTEs). The track object requires a TrackManager component.")]
    public GameObject aiTrack;

    /// <summary>
    /// This is a prefab meant to set up all the fancy camera angles and intro to a race before it actually starts. Prefab requires a RaceStart component.
    /// Check the prefabs folder (search for BasicStart) for examples.
    /// </summary>
    [Tooltip("This is a prefab meant to set up all the fancy camera angles and intro to a race before it actually starts. Prefab requires a RaceStart component.")]
    public GameObject raceStartObject;

    /// <summary>
    /// Which way on the axes to offset tracks. So (1, 0, 0) for offsetting on the x-axis, (0, -1, 0) for offsetting down on the y-axis, that sort of thing.
    /// </summary>
    [Tooltip("Which way on the axes to offset tracks. So (1, 0, 0) for offsetting positively on the x-axis, (0, -1, 0) for offsetting down on the y-axis, that sort of thing.")]
    public Vector3 placementOffsetDimension = Vector3.left;

    /// <summary>
    /// The current list of racetracks being run.
    /// </summary>
    [HideInInspector]
    public List<GameObject> racetracks;

    /// <summary>
    /// The current camera to use for racing.
    /// </summary>
    [Tooltip("The current camera to use for racing.")]
    public RacingCamera racingCamera;

    /// <summary>
    /// How much we've actually offset by in constructing the track.
    /// </summary>
    float placementOffset;

    float timeElapsed = -1;
    /// <summary>
    /// The resulting times from the races.
    /// </summary>
    [HideInInspector]
    public float[] raceTimes;
    int[] trackIndices; //Keeps the indices of the tracks for sorting with raceTimes.

    [Header("Results Stuff")]

    //Temporary way to render leaderboards. Awaiting a more graphically fancy version. TODO: Make this look better and scalable.
    /// <summary>
    /// A UI Prefab to use as the header for the leaderboards.
    /// </summary>
    [Tooltip("A UI Prefab to use as the header for the leaderboards.")]
    public GameObject leaderboardHeader;
    /// <summary>
    /// A UI Prefab to populate the leaderboards with (requires a Text component).
    /// </summary>
    [Tooltip("A UI Prefab to populate the leaderboards with (requires a Text component).")]
    public GameObject leaderboardText;

    /// <summary>
    /// The index where the player's gremlin has been placed in the racetracks list (should be the same as playerIndex provided to TrackSetup, if you have it).
    /// </summary>
    [HideInInspector]
    public int gremlinPlayerIndex;

    /// <summary>
    /// Gets the track ready for racing. StartTracks() is called by raceStartObject.
    /// </summary>
    /// <param name="racingGremlins">The list of gremlin objects to use in the race. Assigns them a track based on their order. Assumes that the Gremlins in the list already exist as objects in the game world.</param>
    /// <param name="playerIndex">The index that the player Gremlin is stored at (so we can make a different track)</param>
    public void TrackSetup(List<GameObject> racingGremlins, int playerIndex) {
        raceTimes = new float[racingGremlins.Count];
        trackIndices = new int[racingGremlins.Count];
        if (playerIndex == 0)
        {
            placementOffset = playerTrack.GetComponent<TrackManager>().trackWidth;
        }
        else {
            placementOffset = aiTrack.GetComponent<TrackManager>().trackWidth;
        }
        for (int i = 0; i < racingGremlins.Count; i++) {
            GameObject track;
            //Disable Gremlin Rigidbodies so nothing weird happens.
            racingGremlins[i].GetComponent<Rigidbody>().isKinematic = true;
            if (i == playerIndex)
            {
                track = Instantiate(playerTrack, placementOffset * placementOffsetDimension, playerTrack.transform.rotation, this.transform); //We set the transform on instantiation, otherwise we get the wrong value for .pathStart.
                track.GetComponent<TrackManager>().ActiveUI = ActiveUI;
            }
            else {
                track = Instantiate(aiTrack, placementOffset * placementOffsetDimension, aiTrack.transform.rotation, this.transform);
            }
            trackIndices[i] = i;
            track.GetComponent<TrackManager>().trackID = i;
            track.transform.position = placementOffset * placementOffsetDimension;
            //Set the Gremlin's position to be the new track's earliest position so we can update the camera.
            racingGremlins[i].transform.position = track.transform.GetChild(0).GetComponent<TrackModule>().pathStart + track.GetComponent<TrackManager>().GremlinOffset;
            placementOffset += track.GetComponent<TrackManager>().trackWidth;
            track.GetComponent<TrackManager>().RacingGremlin = racingGremlins[i];
            racetracks.Add(track);
        }
        gremlinPlayerIndex = playerIndex;
        placementOffset -= racetracks[racetracks.Count - 1].GetComponent<TrackManager>().trackWidth/2;
        var starting = Instantiate(raceStartObject, this.transform);
        starting.GetComponent<RaceStart>().RaceStartSetup(this);
    }

    /// <summary>
    /// Call after TrackSetup(). Will begin each race.
    /// </summary>
    public void StartTracks() {
        timeElapsed = 0;
        foreach (GameObject track in racetracks) { //Let's hope this won't create any unfair results.
            track.GetComponent<TrackManager>().StartRace(UpdateResults, UpdateCamera);
        }
    }

    /// <summary>
    /// When a race is finished, this is called, and this updates the leaderboards.
    /// </summary>
    /// <param name="activeManager">The TrackManager that called this function.</param>
    private void UpdateResults(TrackManager activeManager) { //A race has ended, so add it to the results.
        raceTimes[activeManager.trackID] = timeElapsed;
        activeManager.RacingGremlin.GetComponent<Animator>().Play("Victory"); //Play the Victory Dance!
        var raceIsDone = true;
        for (int i = 0; i < raceTimes.Length; i++) {
            if (!(raceTimes[i] > 0)) {
                raceIsDone = false;
            }
        }
        if (raceIsDone) {
            PostResults();
        }
    }

    [HideInInspector]
    public bool cameraIsFixed;

    /// <summary>
    /// If a gremlin has switched onto a new module, call this, and if that gremlin is being tracked by the camera, update the camera if we need to.
    /// </summary>
    private void UpdateCamera(TrackManager activeManager, TrackModule activeModule) {
        if (racingCamera.gremlinFocus == activeManager.RacingGremlin && activeModule.affectCamera)
        {
            if (activeModule.optimalCameraOffset != Vector3.zero)
            {
                Vector3 newPos = activeModule.optimalCameraOffset + activeModule.transform.position;
                if (activeModule.multiplyOffsetByNumTracks)
                {
                    int trackNumber = racetracks.Count - (activeManager.trackID + 1);
                    newPos += placementOffsetDimension * trackNumber * activeManager.trackWidth;
                }
                cameraIsFixed = activeModule.optimalOffsetIsFixed;
                if (activeModule.cameraImmediateCut)
                {
                    racingCamera.transform.position = newPos;
                    racingCamera.transform.rotation = Quaternion.LookRotation(activeManager.RacingGremlin.transform.position - newPos);
                }
                else
                {
                    racingCamera.SetTween(newPos, Quaternion.LookRotation(activeManager.RacingGremlin.transform.position - newPos), activeModule.modifiedSpeed + 1.0f, newVantageCallback);
                }
            }
            else if (cameraIsFixed) { //Re-enable camera movement once we go to the next module.
                racingCamera.enableMovement = true;
                cameraIsFixed = false;
            }
        }
    }

    private void newVantageCallback() { //Disable camera movement if the camera is fixed, but still allow us to look at the Gremlin.
        racingCamera.SetGremlinFocus(racingCamera.gremlinFocus, false);
        racingCamera.enableMovement = !cameraIsFixed;
    }

    /// <summary>
    /// Puts the results on the active UI. Needs to be redesigned to look better.
    /// </summary>
    private void PostResults() {
        Array.Sort(raceTimes, trackIndices); //Probably a better solution, but whatever. I don't know Quicksort.
        float heightOffset = -30;
        var header = Instantiate(leaderboardHeader, ActiveUI.transform);
        for (int i = 0; i < raceTimes.Length; i++) {
            var text = Instantiate(leaderboardText, header.transform);
            var time = Instantiate(leaderboardText, header.transform);
            text.GetComponent<UnityEngine.UI.Text>().text = racetracks[trackIndices[i]].GetComponent<TrackManager>().RacingGremlin.name;
            time.GetComponent<UnityEngine.UI.Text>().text = raceTimes[i].ToString() + " seconds";
            text.transform.position = new Vector3(-101.6f, heightOffset) + header.transform.position;
            time.transform.position = new Vector3(104.7f, heightOffset) + header.transform.position;
            heightOffset -= 30;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (timeElapsed >= 0)
        {
            timeElapsed += Time.deltaTime;
        }
    }
}
