using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Takes an equation instead of a PathCreator.
/// Uses a TerrainVariant's positionFunction as the equation (So positionFunction will actually be used for position in EquationTrackModule rather than offset).
/// BaseSpeed is how much to increment the function by per fixed frame rate.
/// The next child should be a TweenTrackModule to properly move the Gremlin to the next TrackModule.
/// </summary>
public class EquationTrackModule : TrackModule
{
    /// <summary>
    /// The global offset of the equation. Uses the position of the attached game object if worldPosOffset = 0,0,0
    /// </summary>
    [Tooltip("The global offset of the equation. Uses the position of the attached game object if worldPosOffset = 0,0,0")]
    public Vector3 worldPosOffset = Vector3.zero;

    /// <summary>
    /// What axes to use in the equation.
    /// </summary>
    [Tooltip("What axes to use in the equation.")]
    public Vector3 clampVector = new Vector3(1, 1, 0);

    void Awake()
    {
        for (int i = 0; i < 3; i++) {
            if (worldPosOffset[i] == 0)
            {
                worldPosOffset[i] = this.transform.position[i];
            }
        }
        totalDistance = 0;
        pathStart = terrainVariant.positionFunction(terrainVariant.domain.x, this) + worldPosOffset; //Position Function can now be used for actual positions!
        totalDistance = terrainVariant.domain.x;
        pathEnd = terrainVariant.positionFunction(terrainVariant.domain.y, this) + worldPosOffset;
    }


    private void Update()
    {
        if (gremlinMoving && !settings.paused)
        { //Move the Gremlin around.
            totalDistance += modifiedSpeed * BaseSpeed * Time.deltaTime; //Keeping track of how far along the Gremlin is in this module.
            var isStopping = true;
            for (int i = 0; i < 3; i++) { //Go through each axis on PositionClip, and check if we're within the threshold. If not, don't EndMove.
                float position = terrainVariant.positionClip[i];
                if (position != Mathf.Infinity && terrainVariant.positionFunction(totalDistance, this)[i] - position > terrainVariant.clipTolerance) {
                    isStopping = false;
                }
            }
            if (totalDistance >= terrainVariant.domain.y || isStopping)
            {
                pathEnd = activeGremlin.transform.position; //Set pathEnd so the next TrackModule will know what to do with it.
                EndMove();
            }
            else
            { //Move the Gremlin. We mutliply timePassed by modifiedSpeed to change the speed at which the offset changes (since the speed of the animation also affects the offset).
                SetModifiedSpeed();
                var position = terrainVariant.positionFunction(totalDistance, this);
                for (int i = 0; i < 3; i++) {
                    position[i] = position[i] * clampVector[i];
                }
                activeGremlin.transform.position = position + gOffset + worldPosOffset; //EndOfPathInstruction.Stop just tells our Gremlin to stop when it reaches the end of the path.
                Vector3 nextPos = terrainVariant.positionFunction(totalDistance + 0.01f, this);
                activeGremlin.transform.rotation = Quaternion.LookRotation(new Vector3(nextPos.x, activeGremlin.transform.position.y, nextPos.z) - activeGremlin.transform.position, Vector3.up);
                timePassed += Time.deltaTime;
            }
        }
    }
}
