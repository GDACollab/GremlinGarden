using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepVariant : TerrainVariant
{
    public override Vector3 rotationFunction(float time, TrackModule activeModule)
    {
        return new Vector3(0, Vector3.Angle(activeModuleactiveModule.GetComponent<PathCreation.PathCreator>().path.GetPointAtDistance(activeModule.totalDistance, PathCreation.EndOfPathInstruction.Stop)), 0);
    }
}
