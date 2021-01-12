public class ExampleVariant : TerrainVariant
{
    public float relativeSpeed(Gremlin gremlin)
    {
        return gremlin.speedThing / 1000;
    }

}