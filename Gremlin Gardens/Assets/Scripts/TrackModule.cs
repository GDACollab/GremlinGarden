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
    /// Percentage of how fast the gremlin should be going in comparison to the BaseSpeed.
    /// </summary>
    public float Speed = 1.0f;
    /// <summary>
    /// 
    /// </summary>
    public string RelevantStat = "";
    /// <summary>
    /// How fast the Gremlin moves in units per fixed frame count.
    /// </summary>
    public float BaseSpeed = 5.0f;

    public AnimationState

    private void FixedUpdate()
    {
        
    }
}
