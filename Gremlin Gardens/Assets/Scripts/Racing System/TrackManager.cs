﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

/// <summary>
/// Will handle transferring a gremlin along a set of track modules.
/// A track manager must be attached to an object with a bunch of child objects.
/// All of those child objects must have the TrackManager and Path Creator component.
/// Each child object should be in the order in which you want the goblin to race.
/// So if you want the Gremlin to run after they do a jump, put the running track module directly after the jumping track module.
/// </summary>
public class TrackManager : MonoBehaviour
{
    /// <summary>
    /// The Gremlin that we're going to race with.
    /// </summary>
    [Tooltip("The Gremlin that we're going to race with.")]
    public GameObject RacingGremlin;
    //public GameObject gremlinModel = RacingGremlin.Find("gremlinModel");


    /// <summary>
    /// The UI to attach QTEs to.
    /// </summary>
    [Tooltip("The UI to attach QTEs to.")]
    public GameObject ActiveUI;

    /// <summary>
    /// How far we should offset the Gremlin from its center of mass.
    /// </summary>
    [Tooltip("How far we should offset the Gremlin from its center of mass.")]
    public Vector3 GremlinOffset;

    /// <summary>
    /// The current module that the gremlin is on. Use TrackManager.GetChild(currentChild).GetComponent<TrackModule>(); to access the active TrackModule component.
    /// </summary>
    [HideInInspector]
    public int currentChild;

    /// <summary>
    /// If the current track is for the player's gremlin. Used for creating QTEs by TrackModule. Set by RaceManager.
    /// </summary>
    [HideInInspector]
    public bool isPlayerTrack = false;

    public delegate void Callback(TrackManager activeManager);
    /// <summary>
    /// A callback called at the end of the race. Will pass this current TrackManager in case you need to know anything from the TrackManager.
    /// </summary>
    Callback toCallback;
    public delegate void RacingCallback(TrackManager activeManager, TrackModule activeModule);
    /// <summary>
    /// A callback to be called whenever we switch to a new module.
    /// </summary>
    RacingCallback racingCallback;

    /// <summary>
    /// The width of the track, used in RaceManager for offsetting.
    /// </summary>
    [Tooltip("The width of the track, used in RaceManager for offsetting.")]
    public float trackWidth;

    /// <summary>
    /// Set by the RaceManager. This identifies the track for the RaceManager to use in calculations.
    /// </summary>
    [HideInInspector]
    public int trackID;

    public SettingsMenu settings;

    /// <summary>
    /// Start racing with the selected Gremlin.
    /// </summary>
    /// <param name="endRaceCallback">A callback function (returns void) that takes TrackManager as a parameter, to be called at the end of the race. Optional.</param>
    /// <param name="moduleSwitchCallback">A callback function (returns void) that takes TrackManager and TrackModule as parameters, and is called when the Gremlin reaches a new TrackModule. Optional.</param>
    public void StartRace(Callback endRaceCallback = null, RacingCallback moduleSwitchCallback = null) { //The = null makes both of these optional parameters, in case you don't want to call them.
        currentChild = 0;
        toCallback = endRaceCallback;
        racingCallback = moduleSwitchCallback;
        // Set maxStamina again, just in case.
        RacingGremlin.GetComponent<GremlinObject>().maxStamina = RacingGremlin.GetComponent<GremlinObject>().gremlin.getStat("Stamina");
        Race();
    }

    /// <summary>
    /// Iterate through children of this GameObject, moving the Gremlin across each TrackModule component until we reach the end of the list.
    /// </summary>
    private void Race() {
        if (currentChild == transform.childCount) //Race is over, we've reached the end.
        {
            EndRace();
        }
        else
        {
            TrackModule module = transform.GetChild(currentChild).GetComponent<TrackModule>();
            module.BeginMove(RacingGremlin.GetComponent<GremlinObject>(), GremlinOffset, Race, ActiveUI, settings); //Keep the Gremlin moving.
            RacingGremlin.transform.Find("gremlinModel").GetComponent<Animator>().SetTrigger(module.AnimationToPlay); //CrossFade to next animation (Instead of playing. Might make things smoother. TODO: Test if this is a good idea).
            RacingGremlin.transform.Find("gremlinModel").GetComponent<Animator>().speed = module.modifiedSpeed; //Speed or slow the animation based on how fast the Gremlin is going.
            if (racingCallback != null) {
                racingCallback(this, module);
            }
            currentChild += 1;
        }
    }

    private void EndRace() {
        RacingGremlin.GetComponentInChildren<VisualEffect>().enabled = false;
        RacingGremlin.GetComponent<GremlinObject>().gremlin.setStat("Stamina", RacingGremlin.GetComponent<GremlinObject>().maxStamina);
        if (toCallback != null)
        {
            toCallback(this);
        }
    }
}
