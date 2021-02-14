using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// TrackModule that tweens between the previous and next siblings of this TrackModule in the hierarchy (requires TrackManager to be parent).
/// </summary>
public class TweenTrackModule : TrackModule
{
    private void Update()
    {
        if (gremlinMoving)
        { //Move the Gremlin around.
            totalDistance += modifiedSpeed * BaseSpeed * Time.deltaTime; //Keeping track of how far along the Gremlin is in this module.
            //Hacky work-around for TweenTrackModule not given the previous and next children:
            var prevChild = transform.parent.GetChild(transform.GetSiblingIndex() - 1).GetComponent<TrackModule>().pathEnd;
            var nextChild = transform.parent.GetChild(transform.GetSiblingIndex() + 1).GetComponent<TrackModule>().pathStart;
            if (Vector3.Distance(activeGremlin.transform.position, nextChild) <= 0.5f)
            {
                EndMove();
            }
            else
            { //Move the Gremlin. We mutliply timePassed by modifiedSpeed to change the speed at which the offset changes (since the speed of the animation also affects the offset).
                SetModifiedSpeed();
                activeGremlin.transform.position = Vector3.Lerp(prevChild, nextChild, totalDistance) + terrainVariant.positionFunction(timePassed * modifiedSpeed, this) + gOffset; //EndOfPathInstruction.Stop just tells our Gremlin to stop when it reaches the end of the path.
                
                timePassed += Time.deltaTime;
            }
        }
    }
}
