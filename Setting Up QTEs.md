1. Make something in the UI, and attach a QTEScript (or something that extends the QTEScript, like MashButton or AlternateKeys) to that something.
2. Modify the script to your liking (See the QTEScript class in the Scripts/Racing System/ButtonQTEs folder for how to do that)
3. Make your UI thing a prefab, and delete it from the UI.
4. Drag the prefab in to any TerrainVariant's "QTEButton" slot in the Inspector.
5. Now every TrackModule with that TerrainVariant will use that QTE.