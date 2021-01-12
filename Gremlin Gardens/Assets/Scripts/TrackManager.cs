using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Will handle transferring a gremlin along a set of track modules. A track manager must be attached to an object with a bunch of child objects. All of those child objects
/// must have the TrackManager and Path Creator component.
/// </summary>
public class TrackManager : MonoBehaviour
{

    public TrackModule activeModule;
    void Start()
    {
        
    }
}
