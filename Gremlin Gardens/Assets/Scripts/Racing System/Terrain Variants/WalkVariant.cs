using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WalkVariant", menuName = "Terrain Variants/Walk/WalkVariant")]
public class WalkVariant : TerrainVariant
{
    public override float relativeSpeed(GremlinObject gremlin, TrackModule activeModule) {
        return gremlin.gremlin.getStat("Running") * speedModifier;
    }

    public override Vector3 positionFunction(float time, TrackModule activeModule) {
        return new Vector3(0, 0, Mathf.Sin(time * 15)/5);
    }
}
