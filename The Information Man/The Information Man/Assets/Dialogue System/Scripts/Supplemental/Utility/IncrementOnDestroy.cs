using UnityEngine;
using System.Collections;

namespace PixelCrushers.DialogueSystem {

	/// <summary>
	/// Increments an element of the Lua Variable[] table when the GameObject is destroyed,
	/// and then updates the quest tracker if it's attached to the Dialogue Manager object or
	/// its children. This script is useful for kill quests or gathering quests.
	/// 
	/// </summary>
	[AddComponentMenu("Dialogue System/Actor/Increment On Destroy")]
	public class IncrementOnDestroy : MonoBehaviour {

		/// <summary>
		/// The variable to increment.
		/// </summary>
		public string variable = string.Empty;

		/// <summary>
		/// The increment amount. To decrement, use a negative number.
		/// </summary>
		public int increment = 1;

		/// <summary>
		/// The minimum value.
		/// </summary>
		public int min = 0;

		/// <summary>
		/// The maximum value.
		/// </summary>
		public int max = 100;

		public string alertMessage = string.Empty;

		private bool listenForOnDestroy = false;

		protected string ActualVariableName { 
			get { return string.IsNullOrEmpty(variable) ? OverrideActorName.GetInternalName(transform) : variable; }
		}

		/// <summary>
		/// Only listen for OnDestroy if the script has been enabled.
		/// </summary>
		public void OnEnable() {
			listenForOnDestroy = true;
		}
		
		/// <summary>
		/// If the level is being unloaded, this GameObject will be destroyed.
		/// We don't want to count this in the variable, so disable the script.
		/// </summary>
		public void OnLevelWillBeUnloaded() {
			listenForOnDestroy = false;
		}
		
		/// <summary>
		/// When this object is destroyed, increment the counter and update the quest tracker.
		/// </summary>
		public void OnDestroy() {
			if (!listenForOnDestroy) return;
			int oldValue = DialogueLua.GetVariable(ActualVariableName).AsInt;
			int newValue = Mathf.Clamp(oldValue + increment, min, max);
			DialogueLua.SetVariable(ActualVariableName, newValue);
			DialogueManager.SendUpdateTracker();
			if (!string.IsNullOrEmpty(alertMessage)) DialogueManager.ShowAlert(alertMessage);
		}

	}

}