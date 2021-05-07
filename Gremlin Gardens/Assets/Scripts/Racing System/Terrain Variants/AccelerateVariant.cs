using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AccelerateVariant", menuName = "Terrain Variants/AccelerateVariant")]
public class AccelerateVariant : TerrainVariant
{
    //Here's the equation in desmos: https://www.desmos.com/calculator/6rvjwnu92x
    public float exponent = 2.0f;
    public float scale = 2.0f;
    public override float relativeSpeed(GremlinObject gremlin, TrackModule activeModule) //relativeSpeed serves as dPos/dt, so I created a velocity curve.
    {
        return Mathf.Abs(-scale * Mathf.Pow(activeModule.timePassed - Mathf.Pow((speedModifier + (gremlin.gremlin.getStat("Running") + 1)), 1/exponent), exponent) + ((speedModifier + (gremlin.gremlin.getStat("Running") + 1)) * scale));
    }
}
