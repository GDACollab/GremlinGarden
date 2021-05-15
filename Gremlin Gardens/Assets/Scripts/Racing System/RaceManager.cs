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
    /// The SceneLoader script in the scene to use to transition after the race has ended. Called in PostResults().
    /// </summary>
    [Tooltip("The SceneLoader script in the scene to use to transition after the race has ended. Called in PostResults().")]
    public SceneLoader sceneLoader;
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
    /// The actual object used to start the race.
    /// </summary>
    GameObject raceStart;

    /// <summary>
    /// Which way on the axes to offset tracks. So (1, 0, 0) for offsetting on the x-axis, (0, -1, 0) for offsetting down on the y-axis, that sort of thing.
    /// </summary>
    [Tooltip("Which way on the axes to offset tracks. So (1, 0, 0) for offsetting positively on the x-axis, (0, -1, 0) for offsetting down on the y-axis, that sort of thing.")]
    public Vector3 placementOffsetDimension = Vector3.left;

    /// <summary>
    /// The current list of racetracks being run. Set this ahead of time to use pre-made racetracks.
    /// </summary>
    [Header("The current list of racetracks being run. Set this ahead of time to use pre-made racetracks.")]
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

    /// <summary>
    /// How much time has elapsed since the start of the race.
    /// </summary>
    float timeElapsed = -1;
    /// <summary>
    /// The resulting times from the races.
    /// </summary>
    [HideInInspector]
    public float[] raceTimes;
    /// <summary>
    /// Keeps the indices of the tracks for sorting with raceTimes.
    /// </summary>
    int[] trackIndices;

    /// <summary>
    /// The amount of money a player can win by getting each place. Set by RandomGremlinRace.cs
    /// </summary>
    int[] winningAmounts;

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
    /// The settings menu to use.
    /// </summary>
    [Tooltip("The settings menu to use.")]
    public SettingsMenu settings;

    /// <summary>
    /// The index where the player's gremlin has been placed in the racetracks list (should be the same as playerIndex provided to TrackSetup, if you have it).
    /// </summary>
    [HideInInspector]
    public int gremlinPlayerIndex;

    private void OnGUI()
    {
        //Set an FPS cap because things get weird otherwise. We only set this for the editor, because we don't want to mess up the actual settings for the player.
        if (Application.isEditor) {
            Application.targetFrameRate = 60;
        }
    }

    /// <summary>
    /// Called by TrackSetup to create a new track, instead of using a pre-existing one.
    /// </summary>
    /// <param name="i">The current index in racingGremlins.</param>
    /// <param name="racingGremlins">The gremlins we're currently using to race.</param>
    /// <param name="playerIndex">The index of the player gremlin.</param>
    private void CreateTrack(int i, List<GameObject> racingGremlins, int playerIndex) {
        TrackManager track;
        GameObject trackObj;
        if (i == playerIndex)
        {
            trackObj = Instantiate(playerTrack, placementOffset * placementOffsetDimension, playerTrack.transform.rotation, this.transform); //We set the transform on instantiation, otherwise we get the wrong value for .pathStart.
            track = trackObj.GetComponent<TrackManager>();
            track.ActiveUI = ActiveUI;
            track.isPlayerTrack = true;
        }
        else
        {
            trackObj = Instantiate(aiTrack, placementOffset * placementOffsetDimension, aiTrack.transform.rotation, this.transform);
            track = trackObj.GetComponent<TrackManager>();
        }
        trackIndices[i] = i;
        track.trackID = i;
        track.transform.position = placementOffset * placementOffsetDimension;
        track.settings = settings;
        //Set the Gremlin's position to be the new track's earliest position so we can update the camera.
        racingGremlins[i].transform.position = track.transform.GetChild(0).GetComponent<TrackModule>().pathStart + track.GremlinOffset;
        placementOffset += track.GetComponent<TrackManager>().trackWidth;
        track.RacingGremlin = racingGremlins[i];
        racetracks.Add(trackObj);
    }

    private void PlaceGremlin(int i, List<GameObject> racingGremlins, int playerIndex) {
        TrackManager track = racetracks[i].GetComponent<TrackManager>();
        if (i == playerIndex) {
            track.ActiveUI = ActiveUI;
            track.isPlayerTrack = true;
        }
        trackIndices[i] = i;
        track.trackID = i;
        track.settings = settings;
        racingGremlins[i].transform.position = track.transform.GetChild(0).GetComponent<TrackModule>().pathStart + track.GremlinOffset;
        racingGremlins[i].transform.rotation = Quaternion.LookRotation(track.transform.GetChild(0).GetComponent<TrackModule>().pathEnd - racingGremlins[i].transform.position, Vector3.up);
        track.RacingGremlin = racingGremlins[i];
    }

    /// <summary>
    /// Gets the track ready for racing. StartTracks() is called by raceStartObject.
    /// </summary>
    /// <param name="racingGremlins">The list of gremlin objects to use in the race. Assigns them a track based on their order. Assumes that the Gremlins in the list already exist as objects in the game world.</param>
    /// <param name="playerIndex">The index that the player Gremlin is stored at (so we can make a different track)</param>
    /// <param name="wAmounts">The winning amounts that RandomGremlinRace.cs passes.</param>
    public void TrackSetup(List<GameObject> racingGremlins, int playerIndex, int[] wAmounts) {
        racingCamera.settings = settings;
        raceTimes = new float[racingGremlins.Count];
        trackIndices = new int[racingGremlins.Count];
        bool isPremade = (racetracks.Count > 0);
        if (playerIndex == 0)
        {
            placementOffset = playerTrack.GetComponent<TrackManager>().trackWidth;
        }
        else {
            placementOffset = aiTrack.GetComponent<TrackManager>().trackWidth;
        }
        for (int i = 0; i < racingGremlins.Count; i++) {
            //Disable Gremlin Rigidbodies so nothing weird happens.
            racingGremlins[i].GetComponent<Rigidbody>().isKinematic = true;
            if (isPremade) {
                PlaceGremlin(i, racingGremlins, playerIndex);
            } else {
                CreateTrack(i, racingGremlins, playerIndex);
            }
        }
        winningAmounts = wAmounts;
        gremlinPlayerIndex = playerIndex;
        placementOffset -= racetracks[racetracks.Count - 1].GetComponent<TrackManager>().trackWidth/2;
        raceStart = Instantiate(raceStartObject, this.transform);
        raceStart.GetComponent<RaceStart>().RaceStartSetup(this);
    }

    /// <summary>
    /// Meant to be called by FadeManager when the 
    /// </summary>
    public void SetFadeComplete() {
        raceStart.GetComponent<RaceStart>().OnFadeoutComplete();
    }

    /// <summary>
    /// Called after TrackSetup() by raceStartObject. Will begin each race.
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
        //activeManager.RacingGremlin.GetComponentInChildren<Animator>().Play("Victory"); //Play the Victory Dance!
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
    /// In UpdateCamera, set this to activeModule.optimalOffsetIsFixed.
    /// </summary>
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

    /// <summary>
    /// Called when the camera is done moving from an angle set up by a track piece.
    /// </summary>
    private void newVantageCallback() { //Disable camera movement if the camera is fixed, but still allow us to look at the Gremlin.
        racingCamera.SetGremlinFocus(racingCamera.gremlinFocus, racingCamera.gremlinTrack, false);
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
            text.GetComponent<UnityEngine.UI.Text>().text = racetracks[trackIndices[i]].GetComponent<TrackManager>().RacingGremlin.GetComponent<GremlinObject>().gremlinName;
            time.GetComponent<UnityEngine.UI.Text>().text = raceTimes[i].ToString() + " seconds";
            text.transform.position = new Vector3(-101.6f, heightOffset) + header.transform.position;
            time.transform.position = new Vector3(104.7f, heightOffset) + header.transform.position;
            heightOffset -= 30;
        }

        // Now we check what the player wins, if anything:
        for (int i = 0; i < winningAmounts.Length; i++) {
            if (trackIndices[i] == gremlinPlayerIndex) {
                LoadingData.money += winningAmounts[i];
                // Hack to tell the player what they win:
                var text = Instantiate(leaderboardText, header.transform);
                text.GetComponent<UnityEngine.UI.Text>().text = "Your gremlin placed in: " + (i + 1) + ", so you win: " + winningAmounts[i] + ".";
                text.transform.position = new Vector3(0, heightOffset) + header.transform.position;
            }
        }
        
        sceneLoader.FadeOutLoad("Hub World", 0.3f);
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
