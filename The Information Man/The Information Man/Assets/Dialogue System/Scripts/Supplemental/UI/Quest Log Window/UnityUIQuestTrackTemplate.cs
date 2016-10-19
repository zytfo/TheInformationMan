#if !(UNITY_4_3 || UNITY_4_5)
using UnityEngine;
using PixelCrushers.DialogueSystem;

namespace PixelCrushers.DialogueSystem {

	/// <summary>
	/// This component hooks up the elements of a Unity UI quest track template,
	/// which is used by the Unity UI Quest Tracker.
	/// Add it to your quest track template and assign the properties.
	/// </summary>
	[AddComponentMenu("Dialogue System/UI/Unity UI/Quest/Quest Track Template")]
	public class UnityUIQuestTrackTemplate : MonoBehaviour	{

		public UnityEngine.UI.Text description;

		public UnityEngine.UI.Text entryDescription;

		public bool ArePropertiesAssigned {
			get {
				return (description != null) && (entryDescription != null);
			}
		}

	}

}
#endif