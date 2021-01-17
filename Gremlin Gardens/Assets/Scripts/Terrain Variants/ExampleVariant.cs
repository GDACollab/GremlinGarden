using UnityEngine;

[CreateAssetMenu(fileName = "ExampleVariant", menuName = "Terrain Variants/ExampleVariant")]
public class ExampleVariant : TerrainVariant
{
    public override float relativeSpeed(Gremlin gremlin)
    {
        return gremlin.speedThing * speedModifier;
    }

    public override Vector3 positionFunction(float time, TrackModule activeModule)
    {
        return new Vector3(0, Mathf.Sin(time)/10, 0);
    }

}