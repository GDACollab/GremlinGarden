using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleStartRace : MonoBehaviour
{
    public TrackManager trackManagerToRace;
    public bool ShouldLoop = true;

    private void Start()
    {
        trackManagerToRace.StartRace(RaceIsEnded, ModuleSwitch);
    }

    public void ModuleSwitch(TrackManager manager, TrackModule module) {
        Debug.Log(manager.RacingGremlin + " on " + module.name + " module.");
    }

    public void RaceIsEnded(TrackManager manager) {
        Debug.Log(manager.RacingGremlin.name + " finished track.");
        if (ShouldLoop) {
            manager.StartRace(RaceIsEnded, ModuleSwitch);
        }
    }
}
