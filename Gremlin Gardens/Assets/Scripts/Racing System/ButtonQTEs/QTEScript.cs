using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A pretty hacky solution if I do say so myself. Have every QTE event extend QTE script so that the TrackModule script can attach itself to the relevant script.
/// </summary>
public class QTEScript : MonoBehaviour
{
    /// <summary>
    /// The active module that will be set when SetActiveModule is called.
    /// </summary>
    [HideInInspector]
    public TrackModule activeModule;

    /// <summary>
    /// Called by TrackModule, which sets the active module.
    /// Meant to be overridden if you ever need to set other variables that are dependend on the active module.
    /// </summary>
    /// <param name="module">The module that's calling this function.</param>
    public virtual void SetActiveModule(TrackModule module) {
        activeModule = module;
    }

    /// <summary>
    /// Used in case the QTE needs to modify the speed of the gremlin and the animations.
    /// </summary>
    /// <returns>A float of how much to increase activeModule.modifiedSpeed by.</returns>
    public virtual float ModifySpeed() {
        return 0;
    }
}
