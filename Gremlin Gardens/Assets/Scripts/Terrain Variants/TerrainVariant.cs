using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A template for new terrain. Place this in an empty game object so that TrackModule can access that game object.
/// </summary>
public class TerrainVariant : MonoBehaviour //Have to extend monobehaviour so that you can add TerrainVariant to TrackModule.
{
    /// <summary>
    /// Calculate how fast the gremlin should be moving in terms of percentage, based on a Gremlin's stats.
    /// </summary>
    /// <param name="gremlin">Track Module will pass the Gremlin object that it recieves from TrackManager.</param>
    /// <returns>A percentage of how fast that gremlin should be moving.</returns>
    public virtual float relativeSpeed(Gremlin gremlin) {
        return 1;
    }

    /// <summary>
    /// The function that TrackModule will use to update a Gremlin's position whilst following the Bezier curve.
    /// Given the time as input, return a Vector3 with 1 representing 100% of
    /// the Gremlin's current position, and 0 representing 0% of the Gremlin's current position.
    /// So if we want the Gremlin to weave back and forth while moving, we'd return new Vector3(Mathf.sin(time), 1, 1);
    /// </summary>
    /// <param name="time">The time that's elapsed since starting the race.</param>
    /// <returns>A Vector 3 containing X% of the Gremlin's current position on that axis (with 1 being 100%, 0 being 0%).</returns>
    public virtual Vector3 positionFunction(float time) {
        return Vector3.zero;
    }
}