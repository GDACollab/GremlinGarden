using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A template for new terrain. Place this in an empty game object so that TrackModule can access that game object.
/// </summary>
[CreateAssetMenu(fileName = "TerrainVariant", menuName = "Terrain Variants/TerrainVariant")]
public class TerrainVariant : ScriptableObject
{
    /// <summary>
    /// How much to multiply the TerrainVariant's relativeSpeed function. 
    /// </summary>
    [Tooltip("How much to multiply the TerrainVariant's modified speed by.")]
    public float speedModifier = 1.0f;

    /// <summary>
    /// Prefab button with associated script to use in this TerrainVariant's QTE. Make a prefab and attach something from the Scripts/Racing System/Button QTEs folder.
    /// </summary>
    [Tooltip("Prefab button with associated script to use in this TerrainVariant's QTE. Make a prefab and attach something from the Scripts/Racing System/Button QTEs folder.")]
    public GameObject QTEButton;

    /// <summary>
    /// Calculate how fast the gremlin should be moving in terms of percentage, based on a Gremlin's stats. For a skill at the average level, it should return 1.
    /// </summary>
    /// <example>
    /// return gremlin.somestatistic/gremlin.somestatisticaverage;
    /// </example>
    /// <param name="gremlin">Track Module will pass the Gremlin object that it recieves from TrackManager.</param>
    /// <param name="activeModule">The active module, if you need that to calculate other things.</param>
    /// <returns>A percentage of how fast that Gremlin should be moving.</returns>
    public virtual float relativeSpeed(GremlinObject gremlin, TrackModule activeModule) {
        return 1 * speedModifier;
    }

    /// <summary>
    /// Only to be used by EquationTrackModule.
    /// The domain of time for positionFunction. domain.x will determine what t is equal to when positionFunction is first called,
    /// and if domain.y == t, the Gremlin will stop moving. You may set domain.y = Math.Infinity to only rely on positionClip.
    /// </summary>
    [HideInInspector]
    public virtual Vector2 domain { get; }

    /// <summary>
    /// Only to be used by EquationTrackModule.
    /// A position on the function at which to stop the function. Set each axis equal to Mathf.Infinity if you want that to remain unused.
    /// </summary>
    [HideInInspector]
    public virtual Vector3 positionClip { get; }

    /// <summary>
    /// Only to be used by EquationTrackModule.
    /// The tolerance of distance between PositionClip and the position of the function to trigger a stop to Gremlin movement.
    /// </summary>
    [HideInInspector]
    public virtual float clipTolerance { get; }

    /// <summary>
    /// The function that TrackModule will use to update a Gremlin's position whilst following the Bezier curve.
    /// Given the time as input, return a Vector3 giving the offset of the gremlin. 
    /// </summary>
    /// <example>
    /// If we wanted the object to follow a wavy path on the x axis:
    /// return new Vector3(Mathf.Sin(time), 0, 0);
    /// </example>
    /// <param name="time">The time that's elapsed since starting the module.</param>
    /// <param name="activeModule">The module that called this function. Can be used if you need to look at other data.</param>
    /// <returns>A Vector 3 giving the offset of a Gremlin when moving on this TerrainVariant.</returns>
    public virtual Vector3 positionFunction(float time, TrackModule activeModule) {
        return Vector3.zero;
    }
}