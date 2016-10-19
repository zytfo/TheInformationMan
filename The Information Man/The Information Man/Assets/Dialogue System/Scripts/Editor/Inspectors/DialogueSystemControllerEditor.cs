using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace PixelCrushers.DialogueSystem {

	/// <summary>
	/// Custom inspector editor for DialogueSystemController (e.g., Dialogue Manager).
	/// </summary>
	[CustomEditor (typeof(DialogueSystemController))]
	public class DialogueSystemControllerEditor : Editor {

		private const string LightSkinIconFilename = "Assets/Dialogue System/DLLs/DialogueManager Inspector Light.png";
		private const string DarkSkinIconFilename  = "Assets/Dialogue System/DLLs/DialogueManager Inspector Dark.png";

		private static Texture2D icon = null;
		private static GUIStyle iconButtonStyle = null;
		private static GUIContent iconButtonContent = null;

		private DialogueSystemController dialogueSystemController = null;

		private void OnEnable() {
			dialogueSystemController = target as DialogueSystemController;
		}

		/// <summary>
		/// Draws the inspector GUI. Adds a Dialogue System banner.
		/// </summary>
		public override void OnInspectorGUI() {
			DrawExtraFeatures();
			var originalDialogueUI = GetCurrentDialogueUI();
			DrawDefaultInspector();
			var newDialogueUI = GetCurrentDialogueUI();
			if (newDialogueUI != originalDialogueUI) CheckDialogueUI(newDialogueUI);
		}

		private GameObject GetCurrentDialogueUI() {
			if (dialogueSystemController == null || dialogueSystemController.displaySettings == null) return null;
			return dialogueSystemController.displaySettings.dialogueUI;
		}

		private void CheckDialogueUI(GameObject newDialogueUIObject) {
			var newUIs = dialogueSystemController.displaySettings.dialogueUI.GetComponentsInChildren<UnityUIDialogueUI>(true);
			if (newUIs.Length > 0) {
				DialogueManagerWizard.HandleUnityUIDialogueUI(newUIs[0], DialogueManager.Instance);
			}
		}

		private void DrawExtraFeatures() {
			if (icon == null) {
				string iconFilename = EditorGUIUtility.isProSkin ? DarkSkinIconFilename : LightSkinIconFilename;
				icon = AssetDatabase.LoadAssetAtPath(iconFilename, typeof(Texture2D)) as Texture2D;
			}
			if (dialogueSystemController == null || icon == null) return;
			if (iconButtonStyle == null) {
				iconButtonStyle = new GUIStyle(EditorStyles.label);
				iconButtonStyle.normal.background = icon;
				iconButtonStyle.active.background = icon;
			}
			if (iconButtonContent == null) {
				iconButtonContent = new GUIContent(string.Empty, "Click to open Dialogue Editor.");
			}
			GUILayout.BeginHorizontal();
			if (GUILayout.Button(iconButtonContent, iconButtonStyle, GUILayout.Width(icon.width), GUILayout.Height(icon.height))) {
				Selection.activeObject = dialogueSystemController.initialDatabase;
				PixelCrushers.DialogueSystem.DialogueEditor.DialogueEditorWindow.OpenDialogueEditorWindow();
			}
			GUILayout.FlexibleSpace();
			if (GUILayout.Button("Wizard...", GUILayout.Width(64))) {
				DialogueManagerWizard.Init();
			}
			GUILayout.EndHorizontal();
			EditorWindowTools.DrawHorizontalLine();
		}

	}

}
