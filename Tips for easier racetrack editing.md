0. NEVER EVER SET AN GREMLIN'S POSITION USING THE ANIMATOR. It messes with the TrackManager code. If you want to set a Gremlin's position using the animator, make that Gremlin a child of an empty game object, and set that empty game object to be the new Gremlin. Then animate the child's position however you'd like.
1. You should read the tutorial first.
2. Make a separate object with the TrackManager called TestRacetrack for testing TrackModules individually.
3. Use the Simple Start Race script and set it to loop.
4. For timing and/or editing the path for faster animations, set the TerrainVariant of a TrackModule to SlowMo. Adjust the BaseSpeed and Animations as needed, then set the TrackModule's TerrainVariant to its previous setting. Everything should work as intended, just faster.
5. The Gremlin will always start (I think, you should test this) on the point closest to the farthest end of the negative x axis (opposite of the red arrow).
6. If you want multiple TrackModules to have the same TerrainVariant, you can ctrl+click all of them, and then set their TerrainVariant property collectively.
7. If you have components of the RaceTrack that you don't want to add paths to (meaning they can't be children of an object with the TrackManager component), create a separate object to store those components.