using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SlowWalkVariant", menuName = "Terrain Variants/Walk/SlowWalkVariant")]
public class SlowWalkVariant : WalkVariant
{
    public override Vector3 positionFunction(float time, TrackModule activeModule)
    {
        return new Vector3(0, 0, Mathf.Sin(time * 15)/20);
    }
}
