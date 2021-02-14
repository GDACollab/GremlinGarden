using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The bare basics of what will soon be the ultimate racing camera the world has ever seen.
//Things to maybe implement in the future:
//Make modules able to tweak the camera's settings based on "optimal" settings?
public class RacingCamera : MonoBehaviour
{
    /// <summary>
    /// How far away a gremlin has to be before the camera can start moving.
    /// </summary>
    [Tooltip("How far away a gremlin has to be before the camera can start moving.")]
    public Vector3 gremlinBounds = new Vector3(50, 50, 50);

    /// <summary>
    /// How far away the camera is from the gremlin.
    /// </summary>
    [Tooltip("How far away the camera is from the gremlin.")]
    public Vector3 cameraOffset = new Vector3(0, 1, 1);


    /// <summary>
    /// Should the camera also look at the gremlin?
    /// </summary>
    [Tooltip("Should the camera also look at the gremlin?")]
    public bool lookAtFocus = true;


    /// <summary>
    /// The gremlin the camera is currently focusing on.
    /// </summary>
    [Tooltip("The gremlin the camera is currently focusing on.")]
    GameObject gremlinFocus;


    /// <summary>
    /// Sets the gremlin the camera is currently tracking.
    /// </summary>
    /// <param name="gremlin">The gremlin to track.</param>
    /// <param name="updatePos">Should the camera immediately jump to this gremlin?</param>
    public void SetGremlinFocus(GameObject gremlin, bool updatePos) {
        gremlinFocus = gremlin;
        if (updatePos == true) {
            this.transform.position = gremlinFocus.transform.position + cameraOffset;
            if (lookAtFocus == true)
            {
                this.transform.rotation = Quaternion.LookRotation(gremlinFocus.transform.position - this.transform.position, Vector3.up);
            }
        }
    }

    void Update()
    {
        Vector3 newPos = this.transform.position;
        for (int i = 0; i < 3; i++)
        {
            if (Mathf.Abs(gremlinFocus.transform.position[i] - this.transform.position[i]) > gremlinBounds[i]) {
                newPos[i] = gremlinFocus.transform.position[i] + cameraOffset[i];
            }
        }
        if (lookAtFocus == true) {
            //We round so that subtle movements don't impact rotation, causing motion sickness.
            Vector3 newRotation = new Vector3(Mathf.Round(gremlinFocus.transform.position.x - this.transform.position.x), Mathf.Round(gremlinFocus.transform.position.y - this.transform.position.y), Mathf.Round(gremlinFocus.transform.position.z - this.transform.position.z));
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(newRotation, Vector3.up), Time.deltaTime);
        }
        this.transform.position = Vector3.Lerp(this.transform.position, newPos, Time.deltaTime);
    }
}
