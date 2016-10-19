using UnityEngine;
using UnityEditor;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace PixelCrushers.DialogueSystem {

	/// <summary>
	/// This is a base class for custom converter windows.
	/// </summary>
	public class AbstractConverterWindow : EditorWindow {

		/// <summary>
		/// Gets the source file extension.
		/// </summary>
		/// <value>The source file extension (e.g., 'xml' for XML files).</value>
		public virtual string SourceFileExtension { get { return string.Empty; } }

		/// <summary>
		/// Gets the EditorPrefs key to save the converter window's settings under.
		/// </summary>
		/// <value>The EditorPrefs key.</value>
		public virtual string PrefsKey { get { return UndefinedPrefsKey; } }

		/// <summary>
		/// An undefined prefs key.
		/// </summary>
		private const string UndefinedPrefsKey = "UndefinedConverterKey";


		/// <summary>
		/// This is the base prefs (converter window settings) class. The converter
		/// window uses an instance of this class, or a subclass if you need 
		/// additional info, to store the user's current settings.
		/// </summary>
		[System.Serializable]
		public class Prefs {

			/// <summary>
			/// The source filename. This file gets converted into a dialogue database.
			/// </summary>
			public string sourceFilename = string.Empty;

			/// <summary>
			/// The output folder in which to create the dialogue database.
			/// </summary>
			public string outputFolder = "Assets";

			/// <summary>
			/// The name of the dialogue database.
			/// </summary>
			public string databaseFilename = "Dialogue Database";

			/// <summary>
			/// If <c>true</c>, the converter may overwrite the dialogue database
			/// if it already exists.
			/// </summary>
			public bool overwrite = false;

			/// <summary>
			/// The encoding type to use when reading the source file.
			/// </summary>
			public EncodingType encodingType = EncodingType.Default;

			public Prefs() {}

			/// <summary>
			/// This method reads prefs from EditorPrefs or creates it if it doesn't exist.
			/// </summary>
			/// <param name="key">EditorPrefs key.</param>
			public static Prefs Load(string key) {
				WarnIfKeyUndefined(key);
				string xml = EditorPrefs.GetString(key);
				Prefs prefs = null;
				if (!string.IsNullOrEmpty(xml)) {
					XmlSerializer xmlSerializer = new XmlSerializer(typeof(Prefs));
					prefs = xmlSerializer.Deserialize(new StringReader(xml)) as Prefs;
				}
				return prefs ?? new Prefs();
			}
			
			/// <summary>
			/// This method saves prefs to EditorPrefs.
			/// </summary>
			/// <param name="key">Key.</param>
			/// <param name="prefs">EditorPrefs key.</param>
			public static void Save(string key, Prefs prefs) {
				WarnIfKeyUndefined(key);
				XmlSerializer xmlSerializer = new XmlSerializer(typeof(Prefs));
				StringWriter writer = new StringWriter();
				xmlSerializer.Serialize(writer, prefs);
				EditorPrefs.SetString(key, writer.ToString());
			}

			protected static void WarnIfKeyUndefined(string key) {
				if (string.Equals(key, UndefinedPrefsKey)) {
					Debug.LogWarning(string.Format("{0}: The converter preferences key hasn't been specified. " +
					                               "Check your converter script.", DialogueDebug.Prefix));
				}
			}
		}

		/// <summary>
		/// The prefs for the converter window.
		/// </summary>
		protected Prefs prefs = null;

		/// <summary>
		/// A reference to the Dialogue System template, used to create new dialogue database
		/// assets such as Actors, Items, and Conversations.
		/// </summary>
		protected Template template = null;

		/// <summary>
		/// The current scroll position of the converter window. If the contents of the window
		/// are larger than the current window size, the user can scroll up and down.
		/// </summary>
		protected Vector2 scrollPosition = Vector2.zero;

		public virtual void OnEnable() {
			LoadPrefs();
		}
		
		public virtual void OnDisable() {
			SavePrefs();
		}

		protected virtual void ClearPrefs() {
			prefs = new Prefs();
		}

		protected virtual void LoadPrefs() {
			prefs = Prefs.Load(PrefsKey);
		}
		
		protected virtual void SavePrefs() {
			Prefs.Save(PrefsKey, prefs);
		}

		public virtual void OnGUI() {
			if (prefs == null) LoadPrefs();
			scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
			try {
				DrawControls();
			} finally {
				EditorGUILayout.EndScrollView();
			}
		}

		/// <summary>
		/// Draws the contents of the converter window. You can override this if
		/// you want to draw more than the "source section" and "destination section".
		/// </summary>
		protected virtual void DrawControls() {
			DrawSourceSection();
			DrawDestinationSection();
		}

		/// <summary>
		/// Draws the source section. You can override this if you want to draw more
		/// than the Source File selection field.
		/// </summary>
		protected virtual void DrawSourceSection() {
			EditorGUILayout.BeginHorizontal();
			prefs.sourceFilename = EditorGUILayout.TextField("Source File", prefs.sourceFilename);
			if (GUILayout.Button("...", EditorStyles.miniButton, GUILayout.Width(22))) {
				prefs.sourceFilename = 
					EditorUtility.OpenFilePanel("Select Source File", 
					                            EditorWindowTools.GetDirectoryName(prefs.sourceFilename), 
					                            SourceFileExtension);
				GUIUtility.keyboardControl = 0;
			}
			EditorGUILayout.EndHorizontal();
		}

		/// <summary>
		/// Draws the destination section. You can override this if you want to draw
		/// more than the default controls.
		/// </summary>
		protected virtual void DrawDestinationSection() {
			EditorWindowTools.DrawHorizontalLine();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Save As", EditorStyles.boldLabel);
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();

			EditorWindowTools.StartIndentedSection();
			DrawOutputFolder();
			DrawDatabaseFilename();
			DrawEncodingPopup();
			DrawOverwriteCheckbox();
			DrawConversionButtons();
			EditorWindowTools.EndIndentedSection();
		}

		/// <summary>
		/// Draws the output folder selection field.
		/// </summary>
		protected virtual void DrawOutputFolder() {
			EditorGUILayout.BeginHorizontal();
			prefs.outputFolder = EditorGUILayout.TextField("Output Folder", prefs.outputFolder);
			if (GUILayout.Button("...", EditorStyles.miniButton, GUILayout.Width(22))) {
				prefs.outputFolder = EditorUtility.SaveFolderPanel("Folder to Save Database", prefs.outputFolder, "");
				prefs.outputFolder = "Assets" + prefs.outputFolder.Replace(Application.dataPath, string.Empty);
				GUIUtility.keyboardControl = 0;
			}
			EditorGUILayout.EndHorizontal();
		}

		/// <summary>
		/// Draws the dialogue database save-to filename.
		/// </summary>
		protected virtual void DrawDatabaseFilename() {
			prefs.databaseFilename = EditorGUILayout.TextField("Database Filename", prefs.databaseFilename);
		}

		/// <summary>
		/// Draws the encoding type popup.
		/// </summary>
		protected virtual void DrawEncodingPopup() {
			prefs.encodingType = (EncodingType) EditorGUILayout.EnumPopup("Encoding", prefs.encodingType, GUILayout.Width(300));
		}

		/// <summary>
		/// Draws the overwrite checkbox.
		/// </summary>
		protected virtual void DrawOverwriteCheckbox() {
			prefs.overwrite = EditorGUILayout.Toggle("Overwrite", prefs.overwrite);
		}

		/// <summary>
		/// Draws the conversion buttons. You can override this to change 
		/// what buttons are drawn.
		/// </summary>
		protected virtual void DrawConversionButtons() {
			EditorGUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			DrawClearButton();
			DrawConvertButton();
			EditorGUILayout.EndHorizontal();
		}

		/// <summary>
		/// Draws the Clear button that clears the prefs.
		/// </summary>
		protected virtual void DrawClearButton() {
			if (GUILayout.Button("Clear", GUILayout.Width(100))) {
				ClearPrefs();
			}
		}

		/// <summary>
		/// Draws the Convert button that calls Convert().
		/// </summary>
		protected virtual void DrawConvertButton() {
			EditorGUI.BeginDisabledGroup(!IsReadyToConvert());
			if (GUILayout.Button("Convert", GUILayout.Width(100))) Convert();
			EditorGUI.EndDisabledGroup();
		}

		/// <summary>
		/// Determines whether the window is ready to convert. Override this if you
		/// need to check more than the source, output folder, and database filename.
		/// </summary>
		/// <returns><c>true</c> if this instance is ready to convert; otherwise, <c>false</c>.</returns>
		protected virtual bool IsReadyToConvert() {
			return 
				!string.IsNullOrEmpty(prefs.databaseFilename) &&
					!string.IsNullOrEmpty(prefs.outputFolder) &&
					IsSourceAssigned();
		}

		/// <summary>
		/// Determines whether the source is assigned. Override this if, for
		/// example, your converter pulls from more than one source file.
		/// </summary>
		/// <returns><c>true</c> if this instance is source assigned; otherwise, <c>false</c>.</returns>
		protected virtual bool IsSourceAssigned() {
			return !string.IsNullOrEmpty(prefs.sourceFilename);
		}

		/// <summary>
		/// Converts the source into a dialogue database. This method does housekeeping such
		/// as creating the empty asset and saving it at the end, but the main work is
		/// done in the CopySourceToDialogueDatabase() method that you must implement.
		/// </summary>
		protected virtual void Convert() {
			DialogueDatabase database = LoadOrCreateDatabase();
			if (database == null) {
				Debug.LogError(string.Format("{0}: Couldn't create asset '{1}'.", DialogueDebug.Prefix, prefs.databaseFilename));
			} else {
				if (template == null) template = Template.FromEditorPrefs();
				CopySourceToDialogueDatabase(database);
				TouchUpDialogueDatabase(database);
				EditorUtility.SetDirty(database);
				AssetDatabase.SaveAssets();
				Debug.Log(string.Format("{0}: Created database '{1}' containing {2} actors, {3} conversations, {4} items/quests, {5} variables, and {6} locations.", 
				                        DialogueDebug.Prefix, prefs.databaseFilename, database.actors.Count, database.conversations.Count, database.items.Count, database.variables.Count, database.locations.Count));
			}
		}
		
		/// <summary>
		/// Loads the dialogue database if it already exists and overwrite is ticked; otherwise creates a new one.
		/// </summary>
		/// <returns>
		/// The database.
		/// </returns>
		/// <param name='filename'>
		/// Asset filename.
		/// </param>
		protected virtual DialogueDatabase LoadOrCreateDatabase() {
			string assetPath = string.Format("{0}/{1}.asset", prefs.outputFolder, prefs.databaseFilename);
			DialogueDatabase database = null;
			if (prefs.overwrite) {
				database = AssetDatabase.LoadAssetAtPath(assetPath, typeof(DialogueDatabase)) as DialogueDatabase;
				if (database != null) database.Clear();
			}
			if (database == null) {
				assetPath = AssetDatabase.GenerateUniqueAssetPath(string.Format("{0}/{1}.asset", prefs.outputFolder, prefs.databaseFilename));
				database = ScriptableObject.CreateInstance<DialogueDatabase>();
				database.name = prefs.databaseFilename;
				AssetDatabase.CreateAsset(database, assetPath);
			}
			return database;
		}

		/// <summary>
		/// Touches up dialogue database after conversion. This base version sets the START
		/// dialogue entries' Sequence fields to None(). Override this method if you want to
		/// do something different.
		/// </summary>
		/// <param name="database">Database.</param>
		protected virtual void TouchUpDialogueDatabase(DialogueDatabase database) {
			SetStartCutscenesToNone(database);
			//--- You could add this if you define portraitFolder: FindPortraitTextures(database, portraitFolder);
		}

		/// <summary>
		/// Sets the START dialogue entries' Sequences to None().
		/// </summary>
		/// <param name="database">Database.</param>
		protected virtual void SetStartCutscenesToNone(DialogueDatabase database) {
			foreach (var conversation in database.conversations) {
				SetConversationStartCutsceneToNone(conversation);
			}
		}

		/// <summary>
		/// Sets a conversation's START entry's Sequence to None().
		/// </summary>
		/// <param name="conversation">Conversation.</param>
		protected virtual void SetConversationStartCutsceneToNone(Conversation conversation) {
			DialogueEntry entry = conversation.GetFirstDialogueEntry();
			if (entry == null) {
				Debug.LogWarning(string.Format("{0}: Conversation '{1}' doesn't have a START dialogue entry.", DialogueDebug.Prefix, conversation.Title));
			} else {
				if (string.IsNullOrEmpty(entry.Sequence)) {
					if (Field.FieldExists(entry.fields, "Sequence")) {
						entry.Sequence = "None()";
					} else {
						entry.fields.Add(new Field("Sequence", "None()", FieldType.Text));
					}
				}
			}
		}

		/// <summary>
		/// Finds the actors' portrait textures, given a source portrait folder.
		/// </summary>
		/// <param name="database">Database.</param>
		/// <param name="portraitFolder">Portrait folder.</param>
		protected virtual void FindPortraitTextures(DialogueDatabase database, string portraitFolder) {
			foreach (var actor in database.actors) {
				FindPortraitTexture(actor, portraitFolder);
			}
		}

		/// <summary>
		/// Finds an actor's portrait texture.
		/// </summary>
		/// <param name="actor">Actor.</param>
		/// <param name="portraitFolder">Portrait folder.</param>
		protected virtual void FindPortraitTexture(Actor actor, string portraitFolder) {
			if (actor == null) return;
			string textureName = actor.TextureName;
			if (!string.IsNullOrEmpty(textureName)) {
				string filename = Path.GetFileName(textureName).Replace('\\', '/');
				string assetPath = string.Format("{0}/{1}", portraitFolder, filename);
				Texture2D texture = AssetDatabase.LoadAssetAtPath(assetPath, typeof(Texture2D)) as Texture2D;
				if (texture == null) {
					Debug.LogWarning(string.Format("{0}: Can't find portrait texture {1} for {2}.", DialogueDebug.Prefix, assetPath, actor.Name));
				}
				actor.portrait = texture;
			}
		}

		/// <summary>
		/// Copies the source to the dialogue database. You must implement this. You can
		/// use the helper methods LoadSourceFile(), IsSourceAtEnd(), PeekNextSourceLine(),
		/// and GetNextSourceLine().
		/// </summary>
		/// <param name="database">Database.</param>
		protected virtual void CopySourceToDialogueDatabase(DialogueDatabase database) {
		}


		/// <summary>
		/// The source lines.
		/// </summary>
		protected List<string> sourceLines = new List<string>();

		/// <summary>
		/// Loads the source file into memory.
		/// </summary>
		protected virtual void LoadSourceFile() {
			sourceLines.Clear();
			var file = new StreamReader(prefs.sourceFilename);
			string line;
			while ((line = file.ReadLine()) != null) {
				sourceLines.Add(line.TrimEnd());
			}
			file.Close();
		}

		/// <summary>
		/// Determines whether the source data's memory buffer is at the end.
		/// </summary>
		/// <returns><c>true</c> if source at end; otherwise, <c>false</c>.</returns>
		protected virtual bool IsSourceAtEnd() {
			return sourceLines.Count == 0;
		}

		/// <summary>
		/// Peeks the next source line without removing it from the buffer.
		/// </summary>
		/// <returns>The next source line.</returns>
		protected virtual string PeekNextSourceLine() {
			return (sourceLines.Count > 0) ? sourceLines[0] : string.Empty;
		}

		/// <summary>
		/// Gets the next source line and removes it from the buffer.
		/// </summary>
		/// <returns>The next source line.</returns>
		protected virtual string GetNextSourceLine() {
			if (sourceLines.Count > 0) {
				string s = sourceLines[0];
				sourceLines.RemoveAt(0);
				return s;
			} else {
				throw new EndOfStreamException("Unexpected end of file");
			}
		}

	}

}
