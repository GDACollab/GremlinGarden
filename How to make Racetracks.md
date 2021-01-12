Alright, you're gonna wanna start by making an empty game object. This will contain our TrackManager, to keep track of the Gremlin and move it along the path. Add the TrackManager component.

Next, make your Gremlin. Drag your Gremlin from the hierarchy into the "Racing Gremlin" part of the inspector for the TrackManager. Don't worry about the other options, we'll cover those later.

![starting out](https://github.com/GDACollab/GremlinGarden/blob/track-system/TutorialImages/startingOut.PNG)

Okay, so you've got your Gremlin and your TrackManager. Next, you're going to want to add your racetrack model (or models) to the scene.

For each segment of your racetrack, you're going to either: create an empty object with the Path Creator and Track Module components, or attach Path Creator and  Track Module components to a model. You're then going to parent your model/empty object to your Track Manager. Make sure to order the children of the TrackManager in the order that you want the Gremlin to do the obstacle course, otherwise your Gremlin will teleport from place to place.

Now, edit the Bezier curves that appear from each Empty Object or Model to make a racetrack:
![making a racetrack](https://github.com/GDACollab/GremlinGarden/blob/track-system/TutorialImages/makingRacetrack.PNG)

So for each segment of your racetrack, add 
