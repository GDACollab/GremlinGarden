﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WalkVariant", menuName = "Terrain Variants/Walk/WalkVariant")]
public class WalkVariant : TerrainVariant
{
    public override float relativeSpeed(Gremlin gremlin, TrackModule activeModule) {
        return gremlin.speedThing * speedModifier;
    }

    public override Vector3 positionFunction(float time, TrackModule activeModule) {
        return new Vector3(0, 0, Mathf.Sin(time * 15)/5);
    }
}
