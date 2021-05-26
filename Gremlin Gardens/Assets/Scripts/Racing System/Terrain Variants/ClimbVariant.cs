using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ClimbVariant", menuName = "Terrain Variants/ClimbVariant")]
public class ClimbVariant : TerrainVariant
{
    public override float relativeSpeed(GremlinObject gremlin, TrackModule activeModule)
    {
        return Mathf.Log(2 + gremlin.gremlin.getStat("Climbing"), 100) * speedModifier;
    }
}
