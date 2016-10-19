using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace PixelCrushers.DialogueSystem {

	/// <summary>
	/// This is an example converter that demonstrates how to make a subclass of 
	/// AbstractConverterWindow to make your own converter for the Dialogue System.
	/// It converts CSV (comma-separated value) files in a specific format into a 
	/// dialogue database.
	/// 
	/// In the CSV file, each section is optional. The sections are:
	/// 
	/// - Database
	/// - Actors
	/// - Items (also used for quests)
	/// - Locations
	/// - Variables
	/// - Conversations (high level information about each conversation)
	/// - DialogueEntries (individual dialogue entries in the conversations)
	/// - OutgoingLinks (links between dialogue entries)
	/// 
	/// The Database section must contain:
	/// <pre>
	/// Database
	/// Name,Version,Author,Description,Emphasis1,Emphasis2,Emphasis3,Emphasis4
	/// (name),(version),(author),(description),(emphasis setting 1 in format #rrggbbaa biu),(emphasis2),(emphasis3),(emphasis4)
	/// Global User Script
	/// (luacode)
	/// </pre>
	/// 
	/// The Actors section must contain:
	/// <pre>
	/// Actors
	/// ID,Portrait,AltPortraits,Name,Pictures,Description,IsPlayer
	/// Number,Special,Special,Text,Files,Text,Boolean
	/// (id),(texturename),[(texturenames)],(name),[(picturenames)],(description),(isplayer)
	/// ...
	/// </pre>
	/// 
	/// The Items, Locations, Variables, and Conversations section must contain:
	/// <pre>
	/// (Heading) -- where this is Items, Locations, Variables, or Conversations
	/// ID,(field),(field),(field)...
	/// Number,(fieldtype),(fieldtype),(fieldtype)...
	/// (id),(fieldvalue),(fieldvalue),(fieldvalue)...
	/// </pre>
	/// 
	/// The DialogueEntries section must contain:
	/// <pre>
	/// DialogueEntries
	/// entrytag,ConvID,ID,Actor,Conversant,MenuText,DialogueText,IsGroup,FalseConditionAction,ConditionPriority,Conditions,Script,Sequence,(field),(field)...
	/// Text,Number,Number,Number,Number,Text,Text,Boolean,Special,Special,Text,Text,Text,(fieldtype),(fieldtype),...
	/// (entrytag),(ConvID),(ID),(ActorID),(ConversantID),(MenuText),(DialogueText),(IsGroup),(FalseConditionAction),(ConditionPriority),(Conditions),(Script),(Sequence),(fieldvalue),(fieldvalue)...
	/// </pre>
	/// 
	/// The OutgoingLinks section must contain:
	/// <pre>
	/// OutgoingLinks
	/// OriginConvID,OriginID,DestConvID,DestID,ConditionPriority
	/// Number,Number,Number,Number,Special
	/// #,#,#,#,(priority)
	/// </pre>
	/// 
	/// Omitted values in any particular asset should be tagged with "{{omit}}".
	/// </summary>
	public class CSVConverterWindow : AbstractConverterWindow {

		/// <summary>
		/// Gets the source file extension (CSV).
		/// </summary>
		/// <value>The source file extension (e.g., 'xml' for XML files).</value>
		public override string SourceFileExtension { get { return "csv"; } }

		/// <summary>
		/// Gets the EditorPrefs key to save the converter window's settings under.
		/// </summary>
		/// <value>The EditorPrefs key.</value>
		public override string PrefsKey { get { return "PixelCrushers.DialogueSystem.CSVConverterSettings"; } }

		/// <summary>
		/// Menu item code to create a CSVConverterWindow.
		/// </summary>
		[MenuItem("Window/Dialogue System/Converters/CSV Converter", false, 1)]
		public static void Init() {
			EditorWindow.GetWindow(typeof(CSVConverterWindow), false, "CSV Converter");
		}

		/// <summary>
		/// A list of all asset type headings.
		/// </summary>
		private static List<string> AssetTypeHeadings = new List<string>()  
		{ "Database", "Actors", "Items", "Locations", "Variables", "Conversations", "DialogueEntries", "OutgoingLinks" };

		private static List<string> DefaultSpecialValues = new List<string>()
		{ "ID" };

		private static List<string> ActorSpecialValues = new List<string>()
		{ "ID", "Portrait", "AltPortraits" };
		
		private static List<string> DialogueEntrySpecialValues = new List<string>()
		{ "ID", "entrytag", "ConvID", "IsGroup", "FalseConditionAction", "ConditionPriority", "Conditions", "Script" };

		/// <summary>
		/// Copies the source CSV file to a dialogue database. This method demonstrates 
		/// the helper methods LoadSourceFile(), IsSourceAtEnd(), PeekNextSourceLine(), 
		/// and GetNextSourceLine().
		/// </summary>
		/// <param name="database">Dialogue database to copy into.</param>
		protected override void CopySourceToDialogueDatabase(DialogueDatabase database) {
			Debug.Log("Copying source to dialogue database");
			try {
				LoadSourceFile();
				while (!IsSourceAtEnd()) {
					string line = GetNextSourceLine();
					if (string.Equals(GetFirstField(line), "Database")) {
						ReadDatabaseProperties(database);
					} else if (string.Equals(GetFirstField(line), "Actors")) {
						ReadAssets<Actor>(database.actors);
					} else if (string.Equals(GetFirstField(line), "Items")) {
						ReadAssets<Item>(database.items);
					} else if (string.Equals(GetFirstField(line), "Locations")) {
						ReadAssets<Location>(database.locations);
					} else if (string.Equals(GetFirstField(line), "Variables")) {
						ReadAssets<Variable>(database.variables);
					} else if (string.Equals(GetFirstField(line), "Conversations")) {
						ReadAssets<Conversation>(database.conversations);
					} else if (string.Equals(GetFirstField(line), "DialogueEntries")) {
						ReadDialogueEntries(database);
					} else if (string.Equals(GetFirstField(line), "OutgoingLinks")) {
						ReadOutgoingLinks(database);
					} else {
						throw new InvalidDataException("Line not recognized: " + line);
					}
				}
			} catch (Exception e) {
				Debug.LogError(string.Format("{0}: CSV conversion failed: {1}", DialogueDebug.Prefix, e.Message));
			}
		}

		/// <summary>
		/// Reads the database properties section.
		/// </summary>
		/// <param name="database">Dialogue database.</param>
		private void ReadDatabaseProperties(DialogueDatabase database) {
			Debug.Log("Reading database properties");
			GetNextSourceLine(); // Field headings
			string[] values = GetValues(GetNextSourceLine());
			if (values.Length < 8) throw new IndexOutOfRangeException("Incorrect number of values in database properties line");
			database.name = values[0];
			database.version = values[1];
			database.author = values[2];
			database.description = values[3];
			database.emphasisSettings[0] = UnwrapEmField(values[4]);
			database.emphasisSettings[1] = UnwrapEmField(values[5]);
			database.emphasisSettings[2] = UnwrapEmField(values[6]);
			database.emphasisSettings[3] = UnwrapEmField(values[7]);
			GetNextSourceLine(); // Global User Script heading
			database.globalUserScript = UnwrapValue(GetNextSourceLine());
		}

		/// <summary>
		/// Reads a section of assets such as Actors, Items, etc.
		/// </summary>
		/// <param name="assets">List of assets to populate.</param>
		/// <typeparam name="T">The type of asset.</typeparam>
		private void ReadAssets<T>(List<T> assets) where T : Asset, new() {
			string typeName = typeof(T).Name;
			bool isActorSection = (typeof(T) == typeof(Actor));
			Debug.Log(string.Format("Reading {0} section", typeName));

			// Read field names and types:
			string[] fieldNames = GetValues(GetNextSourceLine());
			string[] fieldTypes = GetValues(GetNextSourceLine());

			// Set up ignore list for values that aren't actual fields:
			List<string> ignore = isActorSection ? ActorSpecialValues : DefaultSpecialValues;

			// Keep reading until we reach another asset type heading or end of file:
			while (!(IsSourceAtEnd() || AssetTypeHeadings.Contains(GetFirstField(PeekNextSourceLine())))) {
				string[] values = GetValues(GetNextSourceLine());

				// Create the asset:
				T asset = new T();
				asset.id = Tools.StringToInt(values[0]);
				asset.fields = new List<Field>();

				// Preprocess a couple extra values for actors:
				if (isActorSection) FindActorPortraits(asset as Actor, values[1], values[2]);

				// Read the remaining values and assign them to the asset's fields:
				ReadAssetFields(fieldNames, fieldTypes, ignore, values, asset.fields);

				// Finally, add the asset:
				assets.Add(asset);
			}
		}

		/// <summary>
		/// Reads the asset fields.
		/// </summary>
		/// <param name="fieldNames">Field names.</param>
		/// <param name="fieldTypes">Field types.</param>
		/// <param name="ignore">List of field names to not add.</param>
		/// <param name="values">CSV values.</param>
		/// <param name="fields">Fields list of populate.</param>
		private void ReadAssetFields(string[] fieldNames, string[] fieldTypes, List<string> ignore, 
		                             string[] values, List<Field> fields) {
			for (int i = 0; i < fieldNames.Length; i++) {
				if ((ignore != null) && ignore.Contains(fieldNames[i])) continue;
				if (string.Equals(values[i], "{{omit}}")) continue;
				if (string.IsNullOrEmpty(fieldNames[i])) continue;
				string title = fieldNames[i];
				string value = values[i];
				FieldType type = GuessFieldType(value, fieldTypes[i]);
				fields.Add(new Field(title, value, type));
			}
		}

		private void FindActorPortraits(Actor actor, string portraitName, string alternatePortraitNames) {
			// To keep this example relatively simple, we don't import the portrait images.
			// To do this, we might create a subclass of AbstractConverterWindow.Prefs containing
			// an extra field called portraitFolder. We'd also have to override ClearPrefs(), 
			// LoadPrefs(), and SavePrefs(), and override DrawSourceSection() to include a
			// GUI control for portraitFolder.
			//
			// Then, in this method, we'd search the contents of portraitFolder for textures
			// matching portraitName and the names in alternatePortraitNames and assign them
			// to the actor.
		}

		/// <summary>
		/// Reads the DialogueEntries section. DialogueEntry is not a subclass of Asset,
		/// so we can't reuse the ReadAssets() code above.
		/// </summary>
		/// <param name="database">Dialogue database.</param>
		private void ReadDialogueEntries(DialogueDatabase database) {
			Debug.Log("Reading DialogueEntries section");

			// Read field names and types:
			string[] fieldNames = GetValues(GetNextSourceLine());
			string[] fieldTypes = GetValues(GetNextSourceLine());

			// Keep reading until we reach another asset type heading or end of file:
			while (!(IsSourceAtEnd() || AssetTypeHeadings.Contains(GetFirstField(PeekNextSourceLine())))) {
				string[] values = GetValues(GetNextSourceLine());

				// Create the dialogue entry:
				DialogueEntry entry = new DialogueEntry();
				entry.fields = new List<Field>();
				// We can ignore value[0] (entrytag).
				entry.conversationID = Tools.StringToInt(values[1]);
				entry.id = Tools.StringToInt(values[2]);
				entry.ActorID = Tools.StringToInt(values[3]);
				entry.ConversantID = Tools.StringToInt(values[4]);
				entry.Title = values[5];
				entry.DefaultMenuText = values[6];
				entry.DefaultDialogueText = values[7];
				entry.isGroup = Tools.StringToBool(values[8]);
				entry.falseConditionAction = values[9];
				entry.conditionPriority = ConditionPriorityTools.StringToConditionPriority(values[10]);
				entry.conditionsString = values[11];
				entry.userScript = values[12];

				// Read the remaining values and assign them to the asset's fields:
				ReadAssetFields(fieldNames, fieldTypes, DialogueEntrySpecialValues, values, entry.fields);
				
				// Finally, add the asset:
				var conversation = database.GetConversation(entry.conversationID);
				if (conversation == null) throw new InvalidDataException(string.Format("Conversation {0} referenced in entry {1} not found", entry.conversationID, entry.id));
				conversation.dialogueEntries.Add(entry);
			}
		}
		
		/// <summary>
		/// Reads the OutgoingLinks section. Again, Link is not a subclass of Asset,
		/// so we can't reuse the ReadAssets() method.
		/// </summary>
		/// <param name="database">Dialogue database.</param>
		private void ReadOutgoingLinks(DialogueDatabase database) {
			Debug.Log("Reading OutgoingLinks section");
			GetNextSourceLine(); // Headings
			GetNextSourceLine(); // Types

			// Keep reading until we reach another asset type heading or end of file:
			while (!(IsSourceAtEnd() || AssetTypeHeadings.Contains(GetFirstField(PeekNextSourceLine())))) {
				string[] values = GetValues(GetNextSourceLine());
				var link = new Link(Tools.StringToInt(values[0]), Tools.StringToInt(values[1]),
				                    Tools.StringToInt(values[2]), Tools.StringToInt(values[3]));
				var entry = database.GetDialogueEntry(link.originConversationID, link.originDialogueID);
				if (entry == null) throw new InvalidDataException(string.Format("Dialogue entry {0}.{1} referenced in link not found", link.originConversationID, link.originDialogueID));
				entry.outgoingLinks.Add(link);
			}
		}
		
		/// <summary>
		/// Returns the individual comma-separated values in a line.
		/// </summary>
		/// <returns>The values.</returns>
		/// <param name="line">Line.</param>
		private string[] GetValues(string line) {
			Regex csvSplit = new Regex("(?:^|,)(\"(?:[^\"]+|\"\")*\"|[^,]*)", RegexOptions.Compiled);
			List<string> values = new List<string>();
			foreach (Match match in csvSplit.Matches(line)) {
				values.Add(UnwrapValue(match.Value.TrimStart(',')));
			}
			return values.ToArray();
		}

		private string GetFirstField(string line) {
			if (line.Contains(",")) {
				var values = line.Split(new char[] { ',' });
				return values[0];
			} else {
				return line;
			}
		}

		/// <summary>
		/// Returns a "fixed" version of a comma-separated value where escaped newlines
		/// have been converted back into real newlines, and optional surrounding quotes 
		/// have been removed.
		/// </summary>
		/// <returns>The value.</returns>
		/// <param name="value">Value.</param>
		private string UnwrapValue(string value) {
			string s = value.Replace("\\n", "\n");
			if (s.StartsWith("\"") && s.EndsWith("\"")) {
				s = s.Substring(1, s.Length - 2).Replace("\"\"", "\"");
			}
			return s;
		}

		/// <summary>
		/// Converts an emphasis field in the format "#rrggbbaa biu" into an EmphasisSetting object.
		/// </summary>
		/// <returns>An EmphasisSetting object.</returns>
		/// <param name="emField">Em field.</param>
		private EmphasisSetting UnwrapEmField(string emField) {
			return new EmphasisSetting(emField.Substring(0, 9), emField.Substring(10, 3));
		}
		
		/// <summary>
		/// The CSV format isn't robust enough to describe if different assets define different
		/// types for the same field name. This method checks if a "Text" field has a Boolean
		/// or Number value and returns that type instead of Text.
		/// </summary>
		/// <returns>The field type.</returns>
		/// <param name="value">Value.</param>
		/// <param name="typeSpecifier">Type specifier.</param>
		private FieldType GuessFieldType(string value, string typeSpecifier) {
			if (string.Equals(typeSpecifier, "Text") && !string.IsNullOrEmpty(value)) {
				if (IsBoolean(value)) {
					return FieldType.Boolean;
				} else if (IsNumber(value)) {
					return FieldType.Number;
				}
			}
			return Field.StringToFieldType(typeSpecifier);
		}
		
		/// <summary>
		/// Determines whether a string represents a Boolean value.
		/// </summary>
		/// <returns><c>true</c> if this is a Boolean value; otherwise, <c>false</c>.</returns>
		/// <param name="value">String value.</param>
		private bool IsBoolean(string value) {
			return ((string.Compare(value, "True", System.StringComparison.OrdinalIgnoreCase) == 0) ||
			        (string.Compare(value, "False", System.StringComparison.OrdinalIgnoreCase) == 0));
		}
		
		/// <summary>
		/// Determines whether a string represents a Number value.
		/// </summary>
		/// <returns><c>true</c> if this is a number; otherwise, <c>false</c>.</returns>
		/// <param name="value">String value.</param>
		private bool IsNumber(string value) {
			float n;
			return float.TryParse(value, System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out n);
		}
		
	}

}
