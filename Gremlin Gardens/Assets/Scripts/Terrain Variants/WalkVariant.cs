using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WalkVariant", menuName = "Terrain Variants/WalkVariant")]
public class WalkVariant : TerrainVariant
{
    public override float relativeSpeed(Gremlin gremlin) {
        return gremlin.speedThing;
    }

    public override Vector3 positionFunction(float time) {
        return new Vector3(0, 0, Mathf.Sin(time * 15)/5);
    }
}
