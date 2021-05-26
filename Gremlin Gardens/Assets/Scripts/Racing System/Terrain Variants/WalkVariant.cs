using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WalkVariant", menuName = "Terrain Variants/Walk/WalkVariant")]
public class WalkVariant : TerrainVariant
{
    public override float relativeSpeed(GremlinObject gremlin, TrackModule activeModule) {
        return Mathf.Log(2 + gremlin.gremlin.getStat("Running"), 100) * speedModifier;
    }
}
