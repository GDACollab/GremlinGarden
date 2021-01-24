using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A pretty hacky solution if I do say so myself. Have every QTE event extend QTE script so that the TrackModule script can attach itself to the relevant script.
/// </summary>
public class QTEScript : MonoBehaviour
{
    [HideInInspector]
    public TrackModule activeModule;
}
