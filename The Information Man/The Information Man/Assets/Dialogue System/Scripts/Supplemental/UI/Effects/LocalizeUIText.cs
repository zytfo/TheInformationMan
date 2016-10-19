#if !(UNITY_4_3 || UNITY_4_5)
using UnityEngine;
using PixelCrushers.DialogueSystem;

namespace PixelCrushers.DialogueSystem {

	/// <summary>
	/// This script localizes the content of the Text element on this GameObject.
	/// You can assign the localized text table to this script or the Dialogue
	/// Manager. The Text element's starting text value serves as the field name
	/// to look up in the table.
	/// </summary>
	[AddComponentMenu("Dialogue System/UI/Unity UI/Effects/Localize UI Text")]
	public class LocalizeUIText : MonoBehaviour {

		[Tooltip("Optional; overrides the Dialogue Manager's table")]
		public LocalizedTextTable localizedTextTable;

		private UnityEngine.UI.Text text = null;
		private string fieldName = string.Empty;
		private bool started = false;
		
		void Start() {
			started = true;
			LocalizeText();
		}
		
		void OnEnable() {
			LocalizeText();
		}
		
		public void LocalizeText() {
			if (!started) return;
			if (string.IsNullOrEmpty(PixelCrushers.DialogueSystem.Localization.Language)) return;
			if (text == null) text = GetComponent<UnityEngine.UI.Text>();
			if (string.IsNullOrEmpty(fieldName)) fieldName = (text != null) ? text.text : string.Empty;
			if (localizedTextTable == null) {
				localizedTextTable = DialogueManager.DisplaySettings.localizationSettings.localizedText;
			}
			if (text == null) {
				if (DialogueDebug.LogWarnings) Debug.LogWarning(DialogueDebug.Prefix + ": LocalizeUILabel didn't find a Text component on " + name + ".", this);
			} else if (localizedTextTable == null) {
				if (DialogueDebug.LogWarnings) Debug.LogWarning(DialogueDebug.Prefix + ": No localized text table is assigned to " + name + " or the Dialogue Manager.", this);
			} else if (!localizedTextTable.ContainsField(fieldName)) {
				if (DialogueDebug.LogWarnings) Debug.LogWarning(DialogueDebug.Prefix + ": Localized text table '" + localizedTextTable.name + "' does not have a field: " + fieldName, this);
			} else if (!HasCurrentLanguage()) {
				if (DialogueDebug.LogWarnings) Debug.LogWarning(DialogueDebug.Prefix + "Localized text table '" + localizedTextTable + "' does not have a language '" + PixelCrushers.DialogueSystem.Localization.Language + "'", this);
			} else {
				text.text = localizedTextTable[fieldName];
			}
		}

		private bool HasCurrentLanguage() {
			if (localizedTextTable == null) return false;
			foreach (var language in localizedTextTable.languages) {
				if (string.Equals(language, PixelCrushers.DialogueSystem.Localization.Language)) {
					return true;
				}
			}
			return false;
		}
	}

}
#endif