using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This is the class that transfers data in between scenes. Right now, it just is for SceneLoader to pass over the next scene to LoadingScreenLoader. Feel free to add more stuff.
/// In whatever the next scene is, you should be able just to access LoadingData.whateverDataYouNeed to get the data that was passed from the previous scene.
/// Since this is static, it can also just act as like an in-game savegame or something.
/// </summary>
public static class LoadingData
{
    /// <summary>
    /// The scene to load in the loading scene.
    /// </summary>
    public static string sceneToLoad;
    /// <summary>
    /// The data of the gremlins the player owns to transfer between each scene.
    /// Each gremlin is stored by its name.
    /// </summary>
    public static Dictionary<string, Gremlin> playerGremlins = new Dictionary<string, Gremlin>();
    /// <summary>
    /// The name of the gremlin to race.
    /// </summary>
    public static string gremlinToRace;
}
