using UnityEngine;
using PixelCrushers.DialogueSystem;

namespace PixelCrushers.DialogueSystem.SequencerCommands {

	/// <summary>
	/// This script implements the sequencer command LoadLevel(levelName).
	/// Before loading the level, it calls PersistentDataManager.Record() to
	/// tell objects in the current level to record their persistent data first,
	/// and then it calls LevelWillBeUnloaded() to tell the objects to ignore
	/// the upcoming OnDestroy() if they have OnDestroy() handlers.
	/// 
	/// If the Dialogue Manager has a LevelManager script, this command will use it.
	/// </summary>
	public class SequencerCommandLoadLevel : SequencerCommand {
		
		public void Start() {
			string levelName = GetParameter(0);
			if (string.IsNullOrEmpty(levelName)) {
				if (DialogueDebug.LogWarnings) Debug.LogWarning(string.Format("{0}: Sequencer: LoadLevel() level name is an empty string", DialogueDebug.Prefix));
			} else {
				if (DialogueDebug.LogInfo) Debug.Log(string.Format("{0}: Sequencer: LoadLevel({1})", DialogueDebug.Prefix, levelName));
				var levelManager = FindObjectOfType<LevelManager>();
				if (levelManager != null) {
					levelManager.LoadLevel(levelName);
				} else {
					PersistentDataManager.Record();
					PersistentDataManager.LevelWillBeUnloaded();
					Application.LoadLevel(levelName);
					PersistentDataManager.Apply();
				}
			}
			Stop();
		}
	}
}
