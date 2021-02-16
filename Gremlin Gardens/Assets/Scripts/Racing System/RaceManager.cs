using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    /// The side geometry of the track, to be used on both ends. Try making an empty object and making the geometry children of that object, and experiment to see what works.
    /// </summary>
    [Tooltip("The side geometry of the track, to be used on both ends. Try making an empty object and making the geometry children of that object, and experiment to see what works.")]
    public GameObject trackSides;

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
    /// Gets the track ready for racing.
    /// </summary>
    /// <param name="racingGremlins">The list of gremlin objects to use in the race. Assigns them a track based on their order. Assumes that the Gremlins in the list already exist as objects in the game world.</param>
    /// <param name="playerIndex">The index that the player Gremlin is stored at (so we can make a different track)</param>
    public void TrackSetup(List<GameObject> racingGremlins, int playerIndex) {
        var side = Instantiate(trackSides, this.transform);
        side.transform.position = Vector3.zero;
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
                track = Instantiate(playerTrack, this.transform);
                track.GetComponent<TrackManager>().ActiveUI = ActiveUI;
            }
            else {
                track = Instantiate(aiTrack, this.transform);
            }
            trackIndices[i] = i;
            track.GetComponent<TrackManager>().trackID = i;
            track.transform.position = placementOffset * placementOffsetDimension;
            //Set the Gremlin's position to be the new track's earliest position so we can update the camera.
            racingGremlins[i].transform.position = track.transform.GetChild(0).GetComponent<TrackModule>().pathStart + track.transform.GetChild(0).transform.position;
            placementOffset += track.GetComponent<TrackManager>().trackWidth;
            track.GetComponent<TrackManager>().RacingGremlin = racingGremlins[i];
            racetracks.Add(track);
        }
        //racingCamera.SetGremlinFocus(racingGremlins[playerIndex], true);
        racingCamera.SetFlyover(racetracks[0].GetComponent<TrackManager>(), 1, true);
        placementOffset -= racetracks[racetracks.Count - 1].GetComponent<TrackManager>().trackWidth/2;
        var otherSide = Instantiate(trackSides, this.transform);
        otherSide.transform.position += placementOffset * placementOffsetDimension;
    }

    /// <summary>
    /// Call after TrackSetup(). Will begin each race.
    /// </summary>
    public void StartTracks() {
        timeElapsed = 0;
        foreach (GameObject track in racetracks) { //Let's hope this won't create any unfair results.
            track.GetComponent<TrackManager>().StartRace(UpdateResults);
        }
    }

    /// <summary>
    /// When a race is finished, this is called, and this updates the leaderboards.
    /// </summary>
    /// <param name="activeManager">The TrackManager that called this function.</param>
    private void UpdateResults(TrackManager activeManager) { //A race has ended, so add it to the results.
        raceTimes[activeManager.trackID] = timeElapsed;
        activeManager.RacingGremlin.GetComponent<Rigidbody>().isKinematic = false; //Re-enable Rigidbody, just in case.
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
            text.GetComponent<Text>().text = racetracks[trackIndices[i]].GetComponent<TrackManager>().RacingGremlin.name;
            time.GetComponent<Text>().text = raceTimes[i].ToString() + " seconds";
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
