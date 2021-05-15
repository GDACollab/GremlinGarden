using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Start a race populated by gremlins with random stats.
/// </summary>
public class RandomGremlinRace : MonoBehaviour
{
    /// <summary>
    /// Manager to start the race with.
    /// </summary>
    [Tooltip("Manager to start the race with.")]
    public RaceManager raceManager;

    /// <summary>
    /// The gremlin prefab to populate the race with.
    /// </summary>
    [Tooltip("Gremlins to populate the race with.")]
    public GameObject gremlinObject;

    /// <summary>
    /// What to attach to the player's gremlin to differentiate them from the others.
    /// </summary>
    [Tooltip("What to attach to the player's gremlin to differentiate them.")]
    public GameObject playerIndicator;

    /// <summary>
    /// What to name the rival gremlin during the race. Temporary fix.
    /// </summary>
    public string rivalName = "Jellybeans";

    string[] GremlinNames = {"Happy", "Grumpy", "Doc", "Sleepy", "Bashful", "Sneezy", "Dopey", "Shrek", "Donkey", "Gremstork", "Dave", "Jellybean 2.0", "Uhhhhh Gremlin",
    "Jeven", "Jazzercise", "Trapped in a-", "Bingus", "Bingo", "Gus", "Amon", "Chun", "Sportz", "Bigwig", "Blueberry", "Butterbean", "Chaos", "Chipmuck", "Crimson",
    "Miskit", "Wet Puddle", "Lakestar", "Domino", "Dreambell", "Einstein", "Euclid", "Keter", "Ferrari", "Flip", "Flizzard", "Gigabyte", "Goosebump", "Harmonica",
    "Bugle", "Hurricane", "Jammin", "Jellybean", "Licorice", "Lunar", "Megabyte", "Noodle", "Nacho", "Shadowfang", "Penguin", "Qwerty", "Sega", "Terabyte", "Tofu",
    "Tinnitus", "Turbo", "Vegas", "Ziggy", "Juliette", "Bean", "Sanic", "Sephora", "Emberpaw"};

    /// <summary>
    /// Number of gremlins to populate the race with.
    /// </summary>
    [Tooltip("Number of gremlins to populate the race with.")]
    public int gremlinCount = 4;

    /// <summary>
    /// The minimum stat value a gremlin needs to win the race. Any maximum possible randomly generated stat will be below this value.
    /// </summary>
    [Tooltip("The minimum stat value a gremlin needs to win the race. The maximum possible randomly generated will be below this value.")]
    public float winningStat = 25.0f;

    /// <summary>
    /// The lowest value a randomly generated statistic can possibly have.
    /// </summary>
    [Tooltip("The lowest value a randomly generated statistic can possibly have.")]
    public float minStatValue = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        List<GameObject> gremlinList = new List<GameObject>();
        int playerGremlin = Random.Range(0, gremlinCount);
        // Hacky solution for inserting rivalGremlin:
        int rivalGremlin = Random.Range(0, playerGremlin);
        if (rivalGremlin == playerGremlin) {
            rivalGremlin = gremlinCount;
        }
        for (int i = 0; i < gremlinCount; i++)
        {
            GameObject gremlin;
            if (i == playerGremlin)
            {
                GremlinObject gremlinToLoad;
                if (LoadingData.playerGremlins.Count != 0)
                {
                    gremlinToLoad = LoadingData.playerGremlins[LoadingData.gremlinToRace];
                }
                else {
                    // For testing and debugging, in case someone decides to load the race without going through the hub world:
                    gremlinToLoad = gameObject.AddComponent<GremlinObject>();
                    gremlinToLoad.gremlin = new Gremlin("My Spoon is Too Big.");
                    gremlinToLoad.gremlinName = "My Spoon is Too Big.";
                    gremlinToLoad.InitializeGremlin();
                    GenerateStats(gremlinToLoad.gremlin);
                }
                // Instead of making a random gremlin, load the player gremlin.
                gremlin = Instantiate(gremlinObject);
                gremlin.GetComponent<GremlinObject>().CopyGremlinData(gremlinToLoad);
                Instantiate(playerIndicator, gremlin.transform);
                gremlin.name = gremlinToLoad.gremlinName;
            }
            else {
                gremlin = Instantiate(gremlinObject);
                Gremlin gremlinClass = gremlin.GetComponent<GremlinObject>().gremlin;
                GenerateStats(gremlinClass);
                gremlin.transform.Find("gremlinModel").transform.Find("gremlin.mesh").GetComponent<SkinnedMeshRenderer>().material.SetColor("_Color", Random.ColorHSV(0f, 1f, .6f, .8f, .5f, .7f));
                gremlin.name = GremlinNames[Random.Range(0, GremlinNames.Length)];
                if (i == rivalGremlin) {
                    gremlin.name = rivalName;
                }
                gremlin.GetComponent<GremlinObject>().gremlinName = gremlin.name;
                
            }
            gremlin.GetComponent<GremlinObject>().nameText.text = gremlin.name;
            gremlinList.Add(gremlin);
        }
        raceManager.TrackSetup(gremlinList, playerGremlin);
    }

    /// <summary>
    /// Generates random stats for a gremlin based on what stats the game thinks the player needs to win.
    /// </summary>
    /// <param name="gremlin">The gremlin to assign random stats to.</param>
    public void GenerateStats(Gremlin gremlin) {
        // TODO: Yeah, I need to double check my math at a point where I'm not very tired.
        // There are a lot of errors here.

        // Make sure no gremlin could possibly beat the winning stat number:
        float maxValue = winningStat - 2.0f;

        // Now we need to generate gremlin stats. Normally I'd use a random number generator with no extra steps,
        // but the competition should feel as close as the designers want it to be. So that's why I spend some time
        // calculating how much dice the game should roll (it creates a normal distribution to ensure that the competition is actually close):

        // Sum of lowest possible values for each die = numDice = minimum value. (For instance, the lowest possible roll for 3d6 is 3).
        int numDice = Mathf.FloorToInt(minStatValue);

        // dice faces = maxValue / number of dice.
        // https://www.redblobgames.com/articles/probability/damage-rolls.html
        // We keep diceFaces as a float because Random.Range can return a random float in between 1.0f and dieFaces, meaning our
        // "dice rolls" can be a little more accurate.
        float diceFaces = maxValue / numDice;

        // Go through each possible gremlin stat (except for Happiness, that doesn't matter in the races)
        foreach (KeyValuePair<string, float> stat in gremlin.getStats()) {
            if (stat.Key != "Happiness") {
                float diceSum = 0;
                float lowestValue = diceFaces;
                for (int i = 0; i < numDice + 1; i++) {
                    float roll = Random.Range(1.0f, diceFaces);
                    diceSum += roll;
                    if (roll < lowestValue) {
                        lowestValue = roll;
                    }
                }

                // While having Gremlins with low stats might be fun, we want to reduce the chances of that happening, so we drop the lowest value from the dice rolls,
                // increasing the odds of having gremlins who are better at racing. It's a trick used in D&D:
                diceSum -= lowestValue;

                gremlin.setStat(stat.Key, diceSum);
            }
        }
    }
}
