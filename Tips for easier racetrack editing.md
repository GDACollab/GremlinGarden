-1. NEVER EVER SET AN GREMLIN'S POSITION USING THE ANIMATOR. It messes with the TrackManager code.
0. You should read the tutorial first.
1. Make a separate object with the TrackManager called TestRacetrack for testing TrackModules individually.
2. Use the Simple Start Race script and set it to loop.
3. For timing and/or editing the path for faster animations, set the TerrainVariant of a TrackModule to SlowMoVariant. Adjust the BaseSpeed and Animations as needed, then set the TrackModule's TerrainVariant to its previous setting. Everything should work as intended, just faster.