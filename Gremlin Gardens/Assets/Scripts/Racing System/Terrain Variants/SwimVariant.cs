using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "SwimVariant", menuName = "Terrain Variants/SwimVariant")]
public class SwimVariant : TerrainVariant
{
    public override float relativeSpeed(Gremlin gremlin, TrackModule activeModule)
    {
        return base.relativeSpeed(gremlin, activeModule);
    }

    public override Vector3 positionFunction(float time, TrackModule activeModule)
    {
        return new Vector3(0, Mathf.Sin(time) * 0.15f, 0);
    }
}
