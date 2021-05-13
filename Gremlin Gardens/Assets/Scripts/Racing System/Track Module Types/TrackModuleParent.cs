using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// If you have multiple TrackModule types as children, then this will allow you to race the gremlin along TrackModule, one by one.
/// </summary>
public class TrackModuleParent : TrackModule
{
    [Header("TrackModuleParent stuff (You can ignore above)")]
    [Tooltip("The children TrackModule to use.")]
    public List<TrackModule> trackModuleChildren;

    /// <summary>
    /// The callback to call once we're done here.
    /// </summary>
    private Callback toCallback;

    /// <summary>
    /// The ui object.
    /// </summary>
    private GameObject ui;

    /// <summary>
    /// The current child we're using.
    /// </summary>
    private int trackChild;
    void Awake()
    {
        totalDistance = 0;
        pathStart = trackModuleChildren[0].pathStart;
        pathEnd = trackModuleChildren[trackModuleChildren.Count - 1].pathEnd;
    }

    public new void BeginMove(GremlinObject gremlin, Vector3 gremlinOffset, Callback callbackFunc, GameObject UI) {
        activeGremlin = gremlin;
        gOffset = gremlinOffset;
        trackChild = 0;
        toCallback = callbackFunc;
        ui = UI;
        trackModuleChildren[trackChild].BeginMove(gremlin, gremlinOffset, NextTrackModule, UI);
    }

    void NextTrackModule()
    {
        trackChild += 1;
        if (trackChild < trackModuleChildren.Count)
        {
            trackModuleChildren[trackChild].BeginMove(activeGremlin, gOffset, NextTrackModule, ui);
        } else {
            toCallback();
        }
    }
}
