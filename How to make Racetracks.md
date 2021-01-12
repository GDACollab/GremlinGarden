Alright, you're gonna wanna start by making an empty game object. This will contain our TrackManager, to keep track of the Gremlin and move it along the path. Add the TrackManager component.

Next, make drag the Gremlin prefab from the prefabs folder into your scene. Drag the newly made gremlin from the hierarchy into the "Racing Gremlin" part of the inspector for the TrackManager. Don't worry about the other options, we'll cover those later.

![starting out](https://github.com/GDACollab/GremlinGarden/blob/track-system/TutorialImages/startingOutPrefab.PNG)

Okay, so you've got your Gremlin and your TrackManager. Next, you're going to want to add your racetrack model (or models) to the scene.

For each segment of your racetrack, you're going to either: create an empty object with the Path Creator and Track Module components, or attach Path Creator and  Track Module components to a model. You're then going to parent your model/empty object to your Track Manager. Make sure to order the children of the TrackManager in the order that you want the Gremlin to do the obstacle course, otherwise your Gremlin will teleport from place to place.

Now, edit the Bezier curves that appear from each Empty Object or Model to make a racetrack:
![making a racetrack](https://github.com/GDACollab/GremlinGarden/blob/track-system/TutorialImages/makingRacetrack.PNG)

Okay, so we've made the basics of a racetrack. To get it running, create an empty object. Attach the Simple Start Race script, and in the "Track Manager To Race" section, select your Track Manager. If you click run... nothing will happen, because we haven't added Terrain Variants to tell the Gremlins their speed. Drag the TerrainManager from the Prefabs folder into your scene.

For each racetrack segment with the TrackModule component, select a TerrainVariant from the TerrainVariant slot. "TerrainVariant" is fine.

Now play the scene. Everything should be working.

![We're done, kinda](https://github.com/GDACollab/GremlinGarden/blob/track-system/TutorialImages/wereDone.PNG)

## What did I just do?
Let's break down each process one by one.

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

Base speed is just the speed in units per fixed frame rate that the Gremlin will travel along the path. Ideally, you'd make an animation, and then time the base speed with that animation.

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

### Simple Start Race

Ideally, we'd have some sort of complex thing to start and time races. For that, you'd call TrackManager.StartRace(Callback), where Callback is a  function which takes a TrackManager object as a parameter:

```csharp
public void Callback(TrackManager manager){
  ...
}
```
From there, you'd be able to access the TrackManager's properties (like TrackManager.RacingGremlin, which tells you the Gremlin Racing that track) to determine things like the Gremlin's time and stuff.

But Simple Start Race will just run the race (and will enable looping, if you hit the checkbox in the inspector). Just create an empty object, attach Simple Start Race, select the TrackManager, and hit play.

Okay, that's a wrap on the tutorial. If you want to mess around with this yourself, load the TrackDemo or TrackTutorial scene to mess around with all the different components. If you have any questions, let me know (ambiguousname#1972 on discord).

Thanks for taking the time to read this!
