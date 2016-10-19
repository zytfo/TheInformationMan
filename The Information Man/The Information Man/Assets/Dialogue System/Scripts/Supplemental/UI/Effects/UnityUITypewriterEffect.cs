#if !(UNITY_4_3 || UNITY_4_5)
using UnityEngine;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace PixelCrushers.DialogueSystem {
	
	/// <summary>
	/// This is a typewriter effect for Unity UI. It handles bold, italic, and 
	/// color rich text tags. It also works with any text alignment.
	/// </summary>
	[AddComponentMenu("Dialogue System/UI/Unity UI/Effects/Unity UI Typewriter Effect")]
	public class UnityUITypewriterEffect : MonoBehaviour {
		
		/// <summary>
		/// How fast to "type."
		/// </summary>
		[Tooltip("How fast to type. This is separate from Dialogue Manager > Subtitle Settings > Chars Per Second")]
		public float charactersPerSecond = 50;
		
		/// <summary>
		/// The audio clip to play with each character.
		/// </summary>
		[Tooltip("Optional audio clip to play with each character")]
		public AudioClip audioClip = null;
		
		/// <summary>
		/// Indicates whether the effect is playing.
		/// </summary>
		/// <value><c>true</c> if this instance is playing; otherwise, <c>false</c>.</value>
		public bool IsPlaying { get; private set; }
		
		private UnityEngine.UI.Text control;
		private AudioSource audioSource;
		private string original = string.Empty;
		private List<string> closureTags = new List<string>();
		private List<string> openingTags = new List<string>();
		public bool paused = false;
		
		public void Awake() {
			control = GetComponent<UnityEngine.UI.Text>();
			audioSource = GetComponent<AudioSource>();
		}
		
		public void OnEnable() {
			if (control != null) StartCoroutine(Play());
		}
		
		public void OnDisable() {
			Stop();
		}
		
		public void Pause() {
			paused = true;
		}
		
		public void Unpause() {
			paused = false;
		}
		
		/// <summary>
		/// Plays the typewriter effect.
		/// </summary>
		public IEnumerator Play() {
			if (charactersPerSecond <= 0) {
				control.text = original;
				IsPlaying = false;
				yield break;
			}
			IsPlaying = true;
			paused = false;
			closureTags.Clear();
			openingTags.Clear();
			original = control.text;
			int originalLength = original.Length;
			int length = 0;
			float delay = 1 / charactersPerSecond;
			float startTime = Time.time;
			float totalTime = Mathf.Max(0.1f, originalLength / charactersPerSecond);
			float originalLengthFloat = originalLength;
			while (length < originalLength) {
				if (!( paused || DialogueTime.IsPaused )) {
					if (audioClip != null && audioSource != null) audioSource.PlayOneShot(audioClip);
					float elapsedTime = Time.time - startTime;
					int currentLength = Mathf.Clamp ((int) ((elapsedTime / totalTime) * originalLengthFloat), 0, originalLength);
					int safeguard = 0;
					while (length < currentLength && safeguard < 10240) {
						length = AdvanceOneCharacter(length);
						safeguard++;
					}
					//---Was: length = AdvanceOneCharacter(length);
					var text = original.Substring(0, length) + GetRichTextClosures() + 
						"<color=#00000000>" + GetRichTextStubs() + StripColorCodes(original.Substring(length)) + "</color>";
					control.text = text;
					//--- Uncomment this line to debug how rich text codes are handled:
					//Debug.Log (text.Replace ("<", "{").Replace (">", "}"));
				}
				yield return new WaitForSeconds(delay);
			}
			control.text = original;
			IsPlaying = false;
		}
		
		private const string RichTextBoldOpen = "<b>";
		private const string RichTextBoldClose = "</b>";
		private const string RichTextItalicOpen = "<i>";
		private const string RichTextItalicClose = "</i>";
		private const string RichTextColorOpenPrefix = "<color=";
		private const string RichTextColorClose = "</color>";
		
		/// <summary>
		/// Advances the label one character or rich text code.
		/// </summary>
		/// <param name="control">GUI Label to advance.</param>
		private int AdvanceOneCharacter(int length) {
			if (original[length] == '<') {
				if (string.Compare(original, length, RichTextBoldOpen, 0, RichTextBoldOpen.Length) == 0) {
					closureTags.Insert(0, RichTextBoldClose);
					openingTags.Insert(0, RichTextBoldOpen);
					return length + RichTextBoldOpen.Length;
				} else if (string.Compare(original, length, RichTextBoldClose, 0, RichTextBoldClose.Length) == 0) {
					closureTags.RemoveAt(0);
					openingTags.RemoveAt(0);
					return length + RichTextBoldClose.Length;
				} else if (string.Compare(original, length, RichTextItalicOpen, 0, RichTextItalicOpen.Length) == 0) {
					closureTags.Insert(0, RichTextItalicClose);
					openingTags.Insert(0, RichTextItalicOpen);
					return length + RichTextItalicOpen.Length;
				} else if (string.Compare(original, length, RichTextItalicClose, 0, RichTextItalicClose.Length) == 0) {
					closureTags.RemoveAt(0);
					openingTags.RemoveAt(0);
					return length + RichTextItalicClose.Length;
				}if (string.Compare(original, length, RichTextColorOpenPrefix, 0, RichTextColorOpenPrefix.Length) == 0) {
					closureTags.Insert(0, RichTextColorClose);
					//--- Don't fake color codes; strip instead: openingTags.Insert(0, "<color=#00000000>");
					var pos = original.Substring(length).IndexOf(">");
					return length + RichTextColorOpenPrefix.Length + pos;
				} else if (string.Compare(original, length, RichTextColorClose, 0, RichTextColorClose.Length) == 0) {
					closureTags.RemoveAt(0);
					//--- Don't fake color codes; strip instead: openingTags.RemoveAt(0);
					return length + RichTextColorClose.Length;
				}
			}
			return length + 1;
		}
		
		/// <summary>
		/// Gets the rich text closures. These are the rich text closing codes that we need to
		/// insert to close off the rich text codes that were opened in first part of 
		/// the string (the visible part).
		/// </summary>
		/// <returns>The rich text closures.</returns>
		private string GetRichTextClosures() {
			if (closureTags.Count == 0) return string.Empty;
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < closureTags.Count; i++) {
				sb.Append(closureTags[i]);
			}
			return sb.ToString();
		}
		
		/// <summary>
		/// Gets the rich text stubs. These are the rich text opening codes that we need to 
		/// insert to at the beginning of the invisible part to make the remaining rich text
		/// closing codes valid.
		/// </summary>
		/// <returns>The rich text stubs.</returns>
		private string GetRichTextStubs() {
			if (openingTags.Count == 0) return string.Empty;
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < openingTags.Count; i++) {
				sb.Append(openingTags[i]);
			}
			return sb.ToString();
		}
		
		private string StripColorCodes(string s) {
			if (!s.Contains("color")) return s;
			return Regex.Replace(s, @"<color=[#]\w+>|</color>", string.Empty);
		}
		
		/// <summary>
		/// Stops the effect.
		/// </summary>
		public void Stop() {
			StopAllCoroutines();
			IsPlaying = false;
			if (control != null) control.text = original;
			original = string.Empty;
		}
		
	}
	
}
#endif