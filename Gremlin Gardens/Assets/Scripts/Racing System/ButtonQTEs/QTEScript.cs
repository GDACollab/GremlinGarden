using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A pretty hacky solution if I do say so myself. Have every QTE event extend QTE script so that the TrackModule script can attach itself to the relevant script.
/// Here's how you're supposed to use this. First, make something in the UI, and attach a QTEScript (or something that extends the QTEScript) to that something.
/// Then, make that thing a prefab, and delete it from the UI. Then, drag the prefab in to any TerrainVariant's "QTEButton" slot in the Inspector. Now every TrackModule with that TerrainVariant will use that QTE.
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
