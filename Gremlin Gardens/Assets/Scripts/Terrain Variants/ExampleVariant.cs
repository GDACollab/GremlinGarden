public class ExampleVariant : TerrainVariant
{
    public override float relativeSpeed(Gremlin gremlin)
    {
        return gremlin.speedThing / 1000;
    }

}