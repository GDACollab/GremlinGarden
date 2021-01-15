using UnityEngine;

public class ExampleVariant : TerrainVariant
{
    public override float relativeSpeed(Gremlin gremlin)
    {
        return gremlin.speedThing / 1000;
    }

    public override Vector3 positionFunction(float time)
    {
        return new Vector3(0, Mathf.Sin(time), 0);
    }

}