/*
Dialogue System Quest Example

This scene demonstrates three quests:

1. Sergeant Graves: A trackable, abandonable kill quest. The quest tracking HUD
   shows the current count.

2. Lieutenant West: A discovery quest using a Quest Trigger.

3. General Starr: A multi-part quest using quest entries and multiple conversations.

The Escape key opens the main menu, from which you can access the quest log window.

NOTE: This scene uses the Feature Demo's SaveGame/LoadGame buttons, which don't
actually reload the level to reset the GameObjects. If you kill an enemy, save
a game, and reload it, the enemy won't be reset. To correctly reset in your own
project, you should reload the level or use a persistent data component to restore
destroyed objects.
*/