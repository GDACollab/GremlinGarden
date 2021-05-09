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
    public RaceManager raceManager;

    /// <summary>
    /// The gremlins to populate the race with.
    /// </summary>
    public GameObject gremlinObject;

    /// <summary>
    /// What to attach to the player's gremlin to differentiate them from the others.
    /// </summary>
    public GameObject playerIndicator;

    string[] GremlinNames = {"Happy", "Grumpy", "Doc", "Sleepy", "Bashful", "Sneezy", "Dopey", "Shrek", "Donkey", "Gremstork", "Dave"};

    /// <summary>
    /// Number of gremlins to populate the race with.
    /// </summary>
    public int gremlinCount = 4;

    /// <summary>
    /// The minimum stat value a gremlin needs to win the race. Used when randomly generating other gremlins' stat values.
    /// </summary>
    [Tooltip("The minimum stat value a gremlin needs to win the race. Used when randomly generating other gremlins' stat values.")]
    public float winningStat = 25.0f;

    /// <summary>
    /// What the stats for an average gremlin racer should generally be. Used when randomly generating other gremlins' stat values.
    /// </summary>
    [Tooltip("What the stats for an average gremlin racer should generally be. Used when randomly generating other gremlins' stat values.")]
    public float meanStatValue = 20.0f;

    // Start is called before the first frame update
    void Start()
    {
        List<GameObject> gremlinList = new List<GameObject>();
        int playerGremlin = Random.Range(0, gremlinCount);
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
                gremlin.transform.GetChild(0).GetComponent<MeshRenderer>().material.SetColor("_Color", new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)));
                gremlin.name = GremlinNames[Random.Range(0, GremlinNames.Length)];
                gremlin.GetComponent<GremlinObject>().gremlinName = gremlin.name;
            }
            gremlinList.Add(gremlin);
        }
        raceManager.TrackSetup(gremlinList, playerGremlin);
    }

    /// <summary>
    /// Generates random stats for a gremlin based on what stats the game thinks the player needs to win.
    /// </summary>
    /// <param name="gremlin">The gremlin to assign random stats to.</param>
    public void GenerateStats(Gremlin gremlin) {
        // Make sure no gremlin could possibly beat the winning stat number:
        float maxValue = winningStat - 2.0f;

        // Now we need to generate gremlin stats. Normally I'd use a random number generator with no extra steps,
        // but the competition should feel as close as the designers want it to be. So that's why I spend some time
        // calculating how much dice the game should roll (it creates a normal distribution to ensure that the competition is actually close):

        // Sum of lowest possible values for each die = numDice = minimum value. (For instance, the lowest possible roll for 3d6 is 3).
        //average value = (maximum - minimum)/2 + minimum
        // average value - maximum/2 = minimum/2
        // 2 * average value - maximum = minimum
        // We get the absolute value in case we get a negative value, which we don't want.
        // A mean value of 10 and a max value of 25 will result in a min value of -5. While that is correct, it's not actually
        // helpful in telling us the number of dice we need.
        // TODO: This of course, means that some calculations for the number of dice will get weird. Need a fix.
        int numDice = Mathf.FloorToInt(Mathf.Abs((2 * meanStatValue) - maxValue));

        // average value = (diceFaces / 2) * number of dice.
        // dieFaces = 2 * (average value / number of dice).
        // Try this out! If you roll 4d8, the most frequent value will be 16 (multiple dice rolls use a normal distribution curve).
        // For 5d10s, the most frequent value will be 25.
        // https://www.redblobgames.com/articles/probability/damage-rolls.html
        int diceFaces = Mathf.FloorToInt(2 * (meanStatValue / numDice));

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
