using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwimVariant : TerrainVariant
{
    public override float relativeSpeed(Gremlin gremlin)
    {
        return base.relativeSpeed(gremlin);
    }

    public override Vector3 positionFunction(float time)
    {
        return new Vector3(0, Mathf.Sin(time) * 0.15f, 0);
    }
}
