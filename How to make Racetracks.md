Let's just start with the barebones components of a racetrack.

First up, we're going to need a TrackManager, to keep track of the Gremlin as it moves along the racetrack. For this, make an empty object, and attach a TrackManager component.

Next, we're going to make our Gremlin. You can either drag a Gremlin from the prefabs folder, or you can create a cube (or some other model) and attach the Animator component and the Gremlin class. The Gremlin needs both these components so the TrackModules and TerrainVariants (which we'll be covering shortly) can both animate the Gremlin and speed or slow the Gremlin based on specific statistics. Add your new Gremlin under the "Racing Gremlin" field in the Inspector view for the TrackManager.

![starting out](https://github.com/GDACollab/GremlinGarden/blob/track-system/TutorialImages/startingOutPrefab.PNG)

So we now have the overall hierarchy for our race. Next up, we're going to actually need things for the Gremlin to race on. Start by importing some models (or making some default cubes, what do I care) to make up your racetrack.

For each segment of your racetrack (that is, for each different running, swimming, sliding, or whatever else kind of race type), you're going to want to either: create an empty object with the PathCreator and TrackModule components, or attach PathCreator and TrackModule components to a model.

The TrackManager requires children with which to run a race. Starting with the first child, and iterating through all of its children, it will look at the PathCreator and TrackModule components of each child, saying "Hey, I need to race this Gremlin along this child's PathCreator. Better call up the TrackModule component to tell the Gremlin to race there." 

The TrackModule is paired with the PathCreator component because the PathCreator doesn't have a function to automatically have an object follow a path. So, we add a bunch of fields that you can tweak to the TrackModule to change how fast or slow a Gremlin will be going.

Now that everything's in place, you can then edit the Beziér curves to make what looks like a racetrack:

![making a racetrack](https://github.com/GDACollab/GremlinGarden/blob/track-system/TutorialImages/makingRacetrack.PNG)

If you hit run right now, nothing will happen, and that's because we're missing two things. Firstly, we need a script to actually start the race. If you just create an empty Game Object, attach the "Simple Start Race" script to it, and select the game object with your TrackManager script, you'll be fine. The second missing thing is a little more tricky. All TrackModules are missing something that's called a TerrainVariant.

What are TerrainVariants? Well, a Gremlin's statistics will ultimately affect how a race is going, we don't have anything to tell a TrackModule how fast or slow a Gremlin should be going based on its statistics, and the calculations for how fast or slow a Gremlin should be going will be different for each type of track (a Gremlin might be faster at swimming than running). So we make something called a TerrainVariant, which has two important jobs (as of right now, it may have more later): To calculate a Gremlin's statistics to tell the TrackModule how fast that Gremlin should be going, and to tell the Gremlin how much it should be offset while on the track (this is for any animations that require the Gremlin to be moved while they're following a path. So if you want the Gremlin to bob up and down while swimming, you'd do that here).

To import the TerrainVariants, either import the TerrainManager prefab or create an empty object and attach a TerrainVariant script for each TerrainVariant you want to be used in the race.

Then for each TrackModule script, attach the relevant TerrainVariant script (i.e., RunVariant for running, SwimVariant for swimming).

Now play the scene. Everything should be working.

If you want to know more about what you can modify you can either read below, or look at the tooltips for each field in the Inspector view.

If you have any questions, let me know (ambiguousname#1972 on discord).

![We're done, kinda](https://github.com/GDACollab/GremlinGarden/blob/track-system/TutorialImages/wereDone.PNG)

## What did I just do?
Let's go into a little more detail.

### Track Manager
First, we made the TrackManager. The TrackManager is in charge of well, managing the track. It will iterate through every child it has (it won't go through children of children, just children), and it will look at each PathCreator and TrackModule component of its children. It says "Hey, I have RacingGremlin assigned to be this Gremlin, and I know that I've just started the race, so this Gremlin should be here."

The Gremlin Offset variable of the TrackManager is used to change the Gremlin's offset during the race. So if a Gremlin is merging with the floor, you can adjust the Gremlin Offset to change... the Gremlin's offset on the track.

### The Gremlin

This is someone else's job, but the TrackManager will look for a Gremlin class (from which to pull stats from), and will also look for an Animator component. If you made your own Gremlin and attached the Gremlin and Animator components, it should theoretically work with the TrackManager.

We attach the Animator component because the TrackModules will play specific animations that the Animator has. Here's a tutorial on how to use the Animator: [https://learn.unity.com/tutorial/controlling-animation](https://learn.unity.com/tutorial/controlling-animation)

### Path Creator

[This is the Beziér Curve tool made by Sebastian Lague](https://assetstore.unity.com/packages/tools/utilities/b-zier-path-creator-136082#description).

The documentation (and instructions for using this tool) is in this branch, available [here](https://github.com/GDACollab/GremlinGarden/blob/track-system/Gremlin%20Gardens/Assets/PathCreator/Documentation/Documentation.pdf).

Just move the Beziér Curve around until it fits your liking.

### Track Module

This actually moves the Gremlin along its specific path. Needs to be a child of your TrackManager, and also needs a complementary Path Creator component.

Animation To Play will be the actual name of the animation that the Gremlin has in its Animator component to play while moving along. You can leave this blank.

Base speed is just the speed in units per fixed frame rate that the Gremlin will travel along the path. Base speed will not affect the speed of the animation, but the relativeSpeed function of TerrainVariants will.
Ideally, you'd make an animation, and then time the base speed with that animation.

Terrain Variants are something we'll cover... right now.

### Terrain Variants

A TerrainVariant tells a Track Module how fast a Gremlin should be moving, and if they should be doing any weird positional things.

This is something for people with **MATH** to manage. A Terrain Variant is a script that you have to attach to an empty object so that the TrackModules can access its data (it was either this or Scriptable Objects, I thought this would be easier). You can make other scripts that extend the Terrain Variant and create your own code for how stuff moves. So here's the default code for TerrainVariant:

```csharp

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A template for new terrain. Place this in an empty game object so that TrackModule can access that game object.
/// </summary>
public class TerrainVariant : MonoBehaviour //Have to extend monobehaviour so that you can add TerrainVariant to TrackModule.
{
    /// <summary>
    /// Calculate how fast the gremlin should be moving in terms of percentage, based on a Gremlin's stats. For a skill at the average level, it should return 1.
    /// </summary>
    /// <example>
    /// return gremlin.somestatistic/gremlin.somestatisticaverage;
    /// </example>
    /// <param name="gremlin">Track Module will pass the Gremlin object that it recieves from TrackManager.</param>
    /// <returns>A percentage of how fast that Gremlin should be moving.</returns>
    public virtual float relativeSpeed(Gremlin gremlin) {
        return 1;
    }

    /// <summary>
    /// The function that TrackModule will use to update a Gremlin's position whilst following the Bezier curve.
    /// Given the time as input, return a Vector3 giving the offset of the gremlin. 
    /// </summary>
    /// <example>
    /// If we wanted the object to follow a wavy path on the x axis:
    /// return new Vector3(Mathf.Sin(time), 0, 0);
    /// </example>
    /// <param name="time">The time that's elapsed since starting the race.</param>
    /// <returns>A Vector 3 giving the offset of a Gremlin when moving on this TerrainVariant.</returns>
    public virtual Vector3 positionFunction(float time) {
        return Vector3.zero;
    }
}
```

And here's the Example Terrain Variant:

```csharp
public class ExampleVariant : TerrainVariant
{
    public override float relativeSpeed(Gremlin gremlin)
    {
        return gremlin.speedThing / 1000;
    }

}
```

So we can override either the positionFunction (which tells us how to offset the Gremlin, so you could do a zig zagging/arcing/random motion or something while the Gremlin is walking/running/swimming) or (more importantly), the **relativeSpeed** function, which is used for telling a Gremlin how fast it can move by returning a percentage after we do some calculations on the Gremlin's stats.

Note that the relativeSpeed function will also affect how fast the animations play (just to make sure the timing of animations isn't off for different Goblins).
### Simple Start Race

Ideally, we'd have some sort of complex thing to start and time races. For that, you'd call TrackManager.StartRace(Callback), where Callback is a  function which takes a TrackManager object as a parameter:

```csharp
public void Callback(TrackManager manager){
  ...
}
```
From there, you'd be able to access the TrackManager's properties (like TrackManager.RacingGremlin, which tells you the Gremlin Racing that track) to determine things like the Gremlin's time and stuff.

But Simple Start Race will just run the race (and will enable looping, if you hit the checkbox in the inspector). Just create an empty object, attach Simple Start Race, select the TrackManager, and hit play.

Okay, that's a wrap on the tutorial. If you want to mess around with this yourself, load the TrackDemo or TrackTutorial scene to mess around with all the different components.

Thanks for taking the time to read this!
