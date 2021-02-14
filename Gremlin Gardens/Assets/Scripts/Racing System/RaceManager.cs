﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceManager : MonoBehaviour
{

    /// <summary>
    /// The active canvas object to be used by QTE prompts.
    /// </summary>
    [Tooltip("The active canvas object to be used by QTE prompts.")]
    public GameObject ActiveUI;
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
    /// The current list of racetracks being run.
    /// </summary>
    [Tooltip("The current list of racetracks being run.")]
    public List<GameObject> racetracks;

    /// <summary>
    /// The current camera to use for racing.
    /// </summary>
    [Tooltip("The current camera to use for racing.")]
    public RacingCamera racingCamera;

    /// <summary>
    /// Which way on the axes to offset tracks. So (1, 0, 0) for offsetting on the x-axis, (0, -1, 0) for offsetting down on the y-axis, that sort of thing.
    /// </summary>
    [Tooltip("Which way on the axes to offset tracks. So (1, 0, 0) for offsetting positively on the x-axis, (0, -1, 0) for offsetting down on the y-axis, that sort of thing.")]
    public Vector3 placementOffsetDimension = Vector3.left;
    float placementOffset;

    /// <summary>
    /// Gets the track ready for racing.
    /// </summary>
    /// <param name="racingGremlins">The list of gremlin objects to use in the race. Assigns them a track based on their order. Assumes that the Gremlins in the list already exist as objects in the game world.</param>
    /// <param name="playerIndex">The index that the player Gremlin is stored at (so we can make a different track)</param>
    public void TrackSetup(List<GameObject> racingGremlins, int playerIndex) {
        var side = Instantiate(trackSides, this.transform);
        side.transform.position = Vector3.zero;
        if (playerIndex == 0)
        {
            placementOffset = playerTrack.GetComponent<TrackManager>().trackWidth;
        }
        else {
            placementOffset = aiTrack.GetComponent<TrackManager>().trackWidth;
        }
        for (int i = 0; i < racingGremlins.Count; i++) {
            GameObject track;
            if (i == playerIndex)
            {
                track = Instantiate(playerTrack, this.transform);
                track.GetComponent<TrackManager>().ActiveUI = ActiveUI;
                racingCamera.SetGremlinFocus(racingGremlins[playerIndex], true);
            }
            else {
                track = Instantiate(aiTrack, this.transform);
            }
            track.transform.position = placementOffset * placementOffsetDimension;
            placementOffset += track.GetComponent<TrackManager>().trackWidth;
            track.GetComponent<TrackManager>().RacingGremlin = racingGremlins[i];
            racetracks.Add(track);
        }
        placementOffset -= racetracks[racetracks.Count - 1].GetComponent<TrackManager>().trackWidth/2;
        var otherSide = Instantiate(trackSides, this.transform);
        otherSide.transform.position += placementOffset * placementOffsetDimension;
    }

    public void StartTracks() {
        foreach (GameObject track in racetracks) { //Let's hope this won't create any unfair results.
            track.GetComponent<TrackManager>().StartRace();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
