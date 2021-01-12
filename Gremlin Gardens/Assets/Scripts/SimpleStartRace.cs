using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleStartRace : MonoBehaviour
{
    public TrackManager trackManagerToRace;
    public bool ShouldLoop = true;

    private void Start()
    {
        trackManagerToRace.StartRace(RaceIsEnded);
    }

    public void RaceIsEnded(TrackManager manager) {
        Debug.Log(manager.RacingGremlin.name + " finished track.");
        if (ShouldLoop) {
            manager.StartRace(RaceIsEnded);
        }
    }
}
