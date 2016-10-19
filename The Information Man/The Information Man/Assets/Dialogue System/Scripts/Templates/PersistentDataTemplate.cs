/* [REMOVE THIS LINE]
 * [REMOVE THIS LINE] To use this template, make a copy and remove the lines that start
 * [REMOVE THIS LINE] with "[REMOVE THIS LINE]". Then add your code where the comments indicate.
 * [REMOVE THIS LINE]



using UnityEngine;
using PixelCrushers.DialogueSystem;

public class PersistentDataTemplate : MonoBehaviour {
	
	public void OnRecordPersistentData() {
		// Add your code here to record data into the Lua environment.
		// Typically, you'll record the data using a line similar to:
		// DialogueLua.SetActorField(name, "myFieldName", myData);
	}
	
	public void OnApplyPersistentData() {
		// Add your code here to get data from Lua and apply it (usually to the game object).
		// Typically, you'll use a line similar to:
		// myData = DialogueLua.GetActorField(name, "myFieldName").AsSomeType;
	}

	//--- Uncomment this method if you want to implement it:
	//public void OnLevelWillBeUnloaded() {
		// This will be called before loading a new level. You may want to add code here
		// to change the behavior of your persistent data script. For example, the
		// IncrementOnDestroy script disables itself because it should only increment
		// the variable when it's destroyed during play, not because it's being
		// destroyed while unloading the old level.
	//}
	
}



/**/