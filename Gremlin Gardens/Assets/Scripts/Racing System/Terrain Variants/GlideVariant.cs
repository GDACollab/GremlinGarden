using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GlideVariant", menuName = "Terrain Variants/Equation Module Variants/GlideVariant")]
public class GlideVariant : TerrainVariant
{
    public override Vector2 domain { get { return new Vector2(0, Mathf.Infinity); } } //Use only position clip for the end.
    public override Vector3 positionClip { get { return new Vector3(Mathf.Infinity, 0, Mathf.Infinity); } } //Clip when y = 0.
    public override float clipTolerance { get { return 0.1f; } }
    
    public override Vector3 positionFunction(float time, TrackModule activeModule)
    {
        //Use speed things to affect the shape of the curve.
        return new Vector3(time * activeModule.activeGremlin.speedThing, -Mathf.Pow(time, 2)/(100 * 1/activeModule.activeGremlin.speedThing) + 20, 0);
    }
}
