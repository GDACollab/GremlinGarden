using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleStartRace : MonoBehaviour
{
    public TrackManager trackManagerToRace;

    private void Start()
    {
        trackManagerToRace.StartRace(RaceIsEnded);
    }

    public void RaceIsEnded(TrackManager manager) {
        Debug.Log(manager.RacingGremlin.name);    
    }
}
