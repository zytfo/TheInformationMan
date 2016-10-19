using UnityEngine;
using System.Collections;

namespace PixelCrushers.DialogueSystem {

	/// <summary>
	/// This persistent data component keeps track of when the GameObject
	/// has been destroyed. The next time the level or game is loaded, if
	/// the GameObject has previously been destroyed, this script will
	/// destroy it again.
	/// </summary>
	[AddComponentMenu("Dialogue System/Save System/Persistent Destructible")]
	public class PersistentDestructible : MonoBehaviour {

		/// <summary>
		/// Assign a Lua variable name to keep track of whether the GameObject
		/// has been destroyed. If this is blank, it uses the name of the
		/// GameObject for the variable name. If the variable is <c>true</c>,
		/// the GameObject has been destroyed.
		/// </summary>
		public string variableName = string.Empty;

		protected string ActualVariableName { 
			get { return string.IsNullOrEmpty(variableName) ? OverrideActorName.GetInternalName(transform) : variableName; }
		}

		private bool listenForOnDestroy = false;

		/// <summary>
		/// When applying persistent data, check the variable. If it's <c>true</c>,
		/// the GameObject has been destroyed previously, so destroy it now.
		/// </summary>
		public void OnApplyPersistentData() {
			if (DialogueLua.GetVariable(ActualVariableName).AsBool) {
				// Before destroying the object, make it think that the level is
				// being unloaded. This will disable any persistent data scripts
				// that use OnDestroy, since it's not really being destroyed
				// during gameplay in this case.
				gameObject.BroadcastMessage("OnLevelWillBeUnloaded", SendMessageOptions.DontRequireReceiver);
				Destroy(gameObject);
			}
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
		/// If the GameObject is destroyed, set its Lua variable <c>true</c>.
		/// </summary>
		public void OnDestroy() {
			if (!listenForOnDestroy) return;
			DialogueLua.SetVariable(ActualVariableName, true);
		}

	}

}