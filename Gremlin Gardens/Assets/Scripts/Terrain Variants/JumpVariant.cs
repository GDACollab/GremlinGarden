using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpVariant : TerrainVariant
{
    public float relativeSpeed(Gremlin gremlin)
    {
        return 2 * gremlin.speedThing;
    }
}
