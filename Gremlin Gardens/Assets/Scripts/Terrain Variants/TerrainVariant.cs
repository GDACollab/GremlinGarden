﻿using System.Collections;
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
    /// Calculate how fast the gremlin should be moving in terms of percentage, based on a Gremlin's stats. For a skill at the average level, it should return 1.
    /// </summary>
    /// <example>
    /// return gremlin.somestatistic/gremlin.somestatisticaverage;
    /// </example>
    /// <param name="gremlin">Track Module will pass the Gremlin object that it recieves from TrackManager.</param>
    /// <returns>A percentage of how fast that Gremlin should be moving.</returns>
    public virtual float relativeSpeed(Gremlin gremlin) {
        return 1 * speedModifier;
    }

    /// <summary>
    /// The function that TrackModule will use to update a Gremlin's position whilst following the Bezier curve.
    /// Given the time as input, return a Vector3 giving the offset of the gremlin. 
    /// </summary>
    /// <example>
    /// If we wanted the object to follow a wavy path on the x axis:
    /// return new Vector3(Mathf.Sin(time), 0, 0);
    /// </example>
    /// <param name="time">The time that's elapsed since starting the race.</param>
    /// <returns>A Vector 3 giving the offset of a Gremlin when moving on this TerrainVariant.</returns>
    public virtual Vector3 positionFunction(float time) {
        return Vector3.zero;
    }
}