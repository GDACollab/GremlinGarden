using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Handles the individual aspects of each track module. Should be attached to an object with the PathCreator component. Will handle movement and animation along the track, if the appropriate
/// attributes are provided.
/// </summary>
public class TrackModule : MonoBehaviour
{
    /// <summary>
    /// How fast the Gremlin moves in units per fixed frame count.
    /// </summary>
    public float BaseSpeed = 5.0f;

    /// <summary>
    /// The animation to use for this TrackModule.
    /// </summary>
    public AnimationState animationToPlay;

    /// <summary>
    /// The function that TrackModule will use to update a Gremlin's position whilst following the Bezier curve. Given the time as input, return a Vector3 with 1 representing 100% of
    /// the Gremlin's current position, and 0 representing 0% of the Gremlin's current position.
    /// So if we want the Gremlin to weave back and forth while moving, we'd return new Vector3(Mathf.sin(time), 1, 1);
    /// </summary>
    /// <param name="time">The time that's elapsed since starting the race.</param>
    /// <returns>A Vector 3 containing X% of the Gremlin's current position on that axis (with 1 being 100%, 0 being 0%).</returns>
    public delegate Vector3 PositionFunction(float time);


}
