using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowMoVariant : TerrainVariant
{
    public override float relativeSpeed(Gremlin gremlin)
    {
        return gremlin.speedThing / 10;
    }
}
