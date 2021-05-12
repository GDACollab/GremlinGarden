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
        var gremlin = activeModule.activeGremlin;
        var flyingStat = 0.0f;
        // This is for generating something EquationTrackModule tries to pull from positionFunction without an active gremlin:
        if (gremlin != null) {
            flyingStat = gremlin.gremlin.getStat("Flying");
        }
        return new Vector3(time * Mathf.Log(2 + flyingStat, 100), -Mathf.Pow(time, 2)/(100 * 1/Mathf.Log(flyingStat + 2, 100)) + 20, 0);
    }
}
