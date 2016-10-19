using UnityEngine;

namespace PixelCrushers.DialogueSystem {

	/// <summary>
	/// When you attach this script to an actor, conversations involving that actor will be
	/// logged to the console.
	/// </summary>
	[AddComponentMenu("Dialogue System/Miscellaneous/Conversation Logger")]
	public class ConversationLogger : MonoBehaviour {

		public Color playerColor = Color.blue;
		public Color npcColor = Color.red;
		
		public void OnConversationStart(Transform actor) {
			Debug.Log(string.Format("{0}: Starting conversation with {1}", name, GetActorName(actor)));
		}
		
		public void OnConversationLine(Subtitle subtitle) {
			if (subtitle == null | subtitle.formattedText == null | string.IsNullOrEmpty(subtitle.formattedText.text)) return;
			string speakerName = (subtitle.speakerInfo != null && subtitle.speakerInfo.transform != null) ? subtitle.speakerInfo.transform.name : "(null speaker)";
			Debug.Log(string.Format("<color={0}>{1}: {2}</color>", GetActorColor(subtitle), speakerName, subtitle.formattedText.text));
		}
		
		public void OnConversationEnd(Transform actor) {
			Debug.Log(string.Format("{0}: Ending conversation with {1}", name, GetActorName(actor)));
		}

		private string GetActorName(Transform actor) {
			return (actor != null) ? actor.name : "(null transform)";
		}
		
		private string GetActorColor(Subtitle subtitle) {
			if (subtitle == null | subtitle.speakerInfo == null) return "white";
			return Tools.ToWebColor(subtitle.speakerInfo.IsPlayer ? playerColor : npcColor);
		}
		
	}

}
