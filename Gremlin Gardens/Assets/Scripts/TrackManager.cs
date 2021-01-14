﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    /// <summary>
    /// How long in seconds for animations to CrossFade for.
    /// </summary>
    [Tooltip("How long in seconds for animations to CrossFade for.")]
    public float TransitionTime = 0.25f;

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
    /// Start racing with the selected Gremlin.
    /// </summary>
    /// <param name="endRaceCallback">A callback function (returns void) that takes TrackManager as a parameter, to be called at the end of the race. Optional.</param>
    /// <param name="moduleSwitchCallback">A callback function (returns void) that takes TrackManager and TrackModule as parameters, and is called when the Gremlin reaches a new TrackModule. Optional.</param>
    public void StartRace(Callback endRaceCallback = null, RacingCallback moduleSwitchCallback = null) { //The = null makes both of these optional parameters, in case you don't want to call them.
        currentChild = 0;
        toCallback = endRaceCallback;
        racingCallback = moduleSwitchCallback;
        Race();
    }

    /// <summary>
    /// Start the race, pick which Gremlin to race with.
    /// </summary>
    public void Race() {
        if (currentChild == transform.childCount) //Race is over, we've reached the end.
        {
            EndRace();
        }
        else
        {
            TrackModule module = transform.GetChild(currentChild).GetComponent<TrackModule>();
            module.BeginMove(RacingGremlin.GetComponent<Gremlin>(), GremlinOffset, Race); //Keep the Gremlin moving.
            RacingGremlin.GetComponent<Animator>().CrossFade(module.AnimationToPlay, TransitionTime); //CrossFade to next animation (Instead of playing. Might make things smoother. TODO: Test if this is a good idea).
            RacingGremlin.GetComponent<Animator>().speed = module.modifiedSpeed; //Speed or slow the animation based on how fast the Gremlin is going.
            racingCallback(this, transform.GetChild(currentChild).GetComponent<TrackModule>());
            currentChild += 1;
        }
    }

    public void EndRace() {
        toCallback(this);
    }
}
