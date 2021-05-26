using System;
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
    public static string gremlinToRace = "Gremmy";

    public enum AllRaces { Starter, Beginner, Intermediate, Expert };
    /// <summary>
    /// Which race the player is going to & just played
    /// Only update upon entering a race from the race board
    /// </summary>
    public static AllRaces currentRace = AllRaces.Beginner;
    /// <summary>
    /// If the player won "currentRace"
    /// </summary>
    public static bool wonCurrentRace = false;
    /// <summary>
    /// Dictionary that stores true for races the player has won in, false if they haven't
    /// </summary>
    public static Dictionary<AllRaces, bool> RaceHistoryDictionary = new Dictionary<AllRaces, bool>();
    /// <summary>
    /// Dictionary that stores the scene name of AllRaces
    /// </summary>
    public static Dictionary<AllRaces, string> RaceSceneDictionary = new Dictionary<AllRaces, string>();

    public static int money = 0;

    public static Vector3 playerPosition;
    public static Quaternion playerRotation;

    /// <summary>
    /// Initializes the RaceHistory dictionary for all states
    /// of AllRaces and false if it is empty
    /// </summary>
    public static void ConstructRaceStatuses()
    {
        if(RaceHistoryDictionary.Count == 0)
        {
            var nameList = Enum.GetNames(typeof(AllRaces));

            foreach (int raceIndex in Enum.GetValues(typeof(AllRaces)))
            {
                RaceHistoryDictionary.Add((AllRaces)raceIndex, false);

                RaceSceneDictionary.Add((AllRaces)raceIndex, nameList[raceIndex] + "Race");
            }
        }
    }
}
