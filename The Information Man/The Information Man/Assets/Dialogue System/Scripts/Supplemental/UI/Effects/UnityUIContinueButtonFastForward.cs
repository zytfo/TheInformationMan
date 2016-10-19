#if !(UNITY_4_3 || UNITY_4_5)
using UnityEngine;
using System.Collections;

namespace PixelCrushers.DialogueSystem {

	/// <summary>
	/// This script replaces the normal continue button functionality with
	/// a two-stage process. If the typewriter effect is still playing, it
	/// simply stops the effect. Otherwise it sends OnContinue to the UI.
	/// </summary>
	[AddComponentMenu("Dialogue System/UI/Unity UI/Effects/Unity UI Continue Button Fast Forward")]
	public class UnityUIContinueButtonFastForward : MonoBehaviour {

		public UnityUIDialogueUI dialogueUI;
		
		public UnityUITypewriterEffect typewriterEffect;

		public void Awake() {
			if (dialogueUI == null) {
				dialogueUI = Tools.GetComponentAnywhere<UnityUIDialogueUI>(gameObject);
			}
			if (typewriterEffect == null) {
				typewriterEffect = GetComponentInChildren<UnityUITypewriterEffect>();
			}
		}

		public void OnFastForward() {
			if ((typewriterEffect != null) && typewriterEffect.IsPlaying) {
				typewriterEffect.Stop();
			} else {
				if (dialogueUI != null) dialogueUI.OnContinue();
			}
		}
		
	}

}
#endif