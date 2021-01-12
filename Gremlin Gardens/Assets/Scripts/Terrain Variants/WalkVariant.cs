using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkVariant : TerrainVariant
{
    public float relativeSpeed(Gremlin gremlin) {
        return gremlin.speedThing;
    }

    public Vector3 positionFunction(float time) {
        Debug.Log(time);
        return new Vector3(Mathf.Cos(time) * 100, 1, 1);
    }
}
