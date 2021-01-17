using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SlowMoVariant", menuName = "Terrain Variants/SlowMoVariant")]
public class SlowMoVariant : TerrainVariant
{
    public override float relativeSpeed(Gremlin gremlin)
    {
        return gremlin.speedThing / 10;
    }
}
