using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenTrackModule : TrackModule
{
    private void FixedUpdate()
    {
        if (gremlinMoving)
        { //Move the Gremlin around.
            totalDistance += modifiedSpeed * BaseSpeed * Time.fixedDeltaTime; //Keeping track of how far along the Gremlin is in this module.
            //Hacky work-around for TweenTrackModule not given the previous and next children. The actual child is .currentChild - 1, so we account for that:
            var prevChild = transform.parent.GetChild(transform.GetComponentInParent<TrackManager>().currentChild - 2).GetComponent<TrackModule>().pathEnd;
            var nextChild = transform.parent.GetChild(transform.GetComponentInParent<TrackManager>().currentChild).GetComponent<TrackModule>().pathStart;
            if (Vector3.Distance(activeGremlin.transform.position, nextChild) <= 0.5f)
            {
                EndMove();
            }
            else
            { //Move the Gremlin. We mutliply timePassed by modifiedSpeed to change the speed at which the offset changes (since the speed of the animation also affects the offset).
                modifiedSpeed = terrainVariant.relativeSpeed(activeGremlin, this); //Get modifiedSpeed again in case it's somehow changed.
                activeGremlin.transform.position = Vector3.Lerp(prevChild, nextChild, totalDistance) + terrainVariant.positionFunction(timePassed * modifiedSpeed, this) + gOffset; //EndOfPathInstruction.Stop just tells our Gremlin to stop when it reaches the end of the path.
                timePassed += Time.fixedDeltaTime;
            }
        }
    }
}
