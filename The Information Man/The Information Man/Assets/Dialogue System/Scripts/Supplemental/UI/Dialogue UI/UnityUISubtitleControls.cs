#if !(UNITY_4_3 || UNITY_4_5)
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace PixelCrushers.DialogueSystem {
	
	/// <summary>
	/// Subtitle Unity UI controls for UnityUIDialogueUI.
	/// </summary>
	[System.Serializable]
	public class UnityUISubtitleControls : AbstractUISubtitleControls {
		
		/// <summary>
		/// The panel containing the response menu controls. A panel is optional, but you may want one
		/// so you can include a background image, panel-wide effects, etc.
		/// </summary>
		[Tooltip("Optional panel for the subtitle elements")]
		public UnityEngine.UI.Graphic panel;
		
		/// <summary>
		/// The label that will show the text of the subtitle.
		/// </summary>
		[Tooltip("Subtitle text")]
		public UnityEngine.UI.Text line;
		
		/// <summary>
		/// The label that will show the portrait image.
		/// </summary>
		[Tooltip("Optional image for speaker's portrait")]
		public UnityEngine.UI.Image portraitImage;
		
		/// <summary>
		/// The label that will show the name of the speaker.
		/// </summary>
		[Tooltip("Optional label for speaker's name")]
		public UnityEngine.UI.Text portraitName;
		
		/// <summary>
		/// The continue button. This is only required if DisplaySettings.waitForContinueButton 
		/// is <c>true</c> -- in which case this button should send "OnContinue" to the UI when clicked.
		/// </summary>
		[Tooltip("Optional continue button; configure OnClick to invoke dialogue UI's OnContinue method")]
		public UnityEngine.UI.Button continueButton;

		[Serializable]
		public class AnimationTransitions {
			public string showTrigger = string.Empty;
			public string hideTrigger = string.Empty;
		}

		[Tooltip("Optional animation transitions; panel should have an Animator")]
		public AnimationTransitions animationTransitions = new AnimationTransitions();

		[Tooltip("Never hide this subtitle panel")]
		public bool alwaysVisible = false;
		
		private bool haveSavedOriginalColor = false;
		private Color originalColor = Color.white;
		private Animator animator = null;
		private bool lookedForAnimator = false;
		private bool isVisible = false;
		private bool isHiding = false;
		
		/// <summary>
		/// Indicates whether this subtitle is currently assigned text.
		/// </summary>
		/// <value>
		/// <c>true</c> if it has text; otherwise, <c>false</c>.
		/// </value>
		public override bool HasText {
			get { return (line != null) && !string.IsNullOrEmpty(line.text); }
		}
		
		public override void SetActive(bool value) {
			try {
				Tools.SetGameObjectActive(line, value || alwaysVisible);
				Tools.SetGameObjectActive(portraitImage, value || alwaysVisible);
				Tools.SetGameObjectActive(portraitName, value || alwaysVisible);
				Tools.SetGameObjectActive(continueButton, value || alwaysVisible);
				if (value == true) {
					Tools.SetGameObjectActive(panel, true);
					if (!isVisible && CanTriggerAnimation(animationTransitions.showTrigger)) {
						animator.SetTrigger(animationTransitions.showTrigger);
					}
				} else {
					if (isVisible && !alwaysVisible && CanTriggerAnimation(animationTransitions.hideTrigger)) {
						animator.SetTrigger(animationTransitions.hideTrigger);
						DialogueManager.Instance.StartCoroutine(DisableAfterAnimation(panel));
					} else if (!isHiding) {
						if (panel != null) Tools.SetGameObjectActive(panel, false || alwaysVisible);
					}
				}
			} finally {
				isVisible = value || alwaysVisible;
			}
		}

		public override void HideContinueButton() {
			Tools.SetGameObjectActive(continueButton, false);
		}
		
		/// <summary>
		/// Sets the subtitle.
		/// </summary>
		/// <param name='subtitle'>
		/// Subtitle.
		/// </param>
		public override void SetSubtitle(Subtitle subtitle) {
			if ((subtitle != null) && !string.IsNullOrEmpty(subtitle.formattedText.text)) {
				if (portraitImage != null) portraitImage.sprite = UITools.CreateSprite(subtitle.GetSpeakerPortrait());
				if (portraitName != null) portraitName.text = subtitle.speakerInfo.Name;
				if (line != null) SetFormattedText(line, subtitle.formattedText);
				Show();
				if (alwaysVisible && line != null) {
					var typewriter = line.GetComponent<UnityUITypewriterEffect>();
					if (typewriter != null) typewriter.OnEnable();
				}
			} else {
				if ((line != null) && (subtitle != null)) SetFormattedText(line, subtitle.formattedText);
				Hide();
			}
		}

		/// <summary>
		/// Clears the subtitle.
		/// </summary>
		public override void ClearSubtitle() {
			SetFormattedText(line, null);
		}
		
		/// <summary>
		/// Sets a label with formatted text.
		/// </summary>
		/// <param name='label'>
		/// Label to set.
		/// </param>
		/// <param name='formattedText'>
		/// Formatted text.
		/// </param>
		private void SetFormattedText(UnityEngine.UI.Text label, FormattedText formattedText) {
			if (label != null) {
				if (formattedText != null) {
					label.text = UITools.GetUIFormattedText(formattedText);
					if (!haveSavedOriginalColor) {
						originalColor = label.color;
						haveSavedOriginalColor = true;
					}
					label.color = (formattedText.emphases.Length > 0) ? formattedText.emphases[0].color : originalColor;
				} else {
					label.text = string.Empty;
				}
			}
		}
		
		/// <summary>
		/// Sets the portrait texture to use in the subtitle if the named actor is the speaker.
		/// This is used to immediately update the GUI control if the SetPortrait() sequencer 
		/// command changes the portrait texture.
		/// </summary>
		/// <param name="actorName">Actor name in database.</param>
		/// <param name="portraitTexture">Portrait texture.</param>
		public override void SetActorPortraitTexture(string actorName, Texture2D portraitTexture) {
			if ((currentSubtitle != null) && string.Equals(currentSubtitle.speakerInfo.nameInDatabase, actorName)) {
				if (portraitImage != null) portraitImage.sprite = UITools.CreateSprite(AbstractDialogueUI.GetValidPortraitTexture(actorName, portraitTexture));  //---Was: .texture = AbstractDialogueUI.GetValidPortraitTexture(actorName, portraitTexture);
			}
		}

		/// <summary>
		/// Auto-focuses the continue button. Useful for gamepads.
		/// </summary>
		public void AutoFocus() {
			if (continueButton != null && UnityEngine.EventSystems.EventSystem.current != null) {
				UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(continueButton.gameObject, null);
			}
		}
		
		private bool CanTriggerAnimation(string triggerName) {
			return CanTriggerAnimations() && !string.IsNullOrEmpty(triggerName);
		}
		
		private bool CanTriggerAnimations() {
			if ((animator == null) && !lookedForAnimator) {
				lookedForAnimator = true;
				if (panel != null) animator = panel.GetComponentInParent<Animator>();
			}
			return (animator != null) && (animationTransitions != null);
		}
		
		private IEnumerator DisableAfterAnimation(UnityEngine.UI.Graphic panel) {
			isHiding = true;
			if (animator != null) {
				const float maxWaitDuration = 10;
				float timeout = Time.realtimeSinceStartup + maxWaitDuration;
				var oldHashId = UITools.GetAnimatorNameHash(animator.GetCurrentAnimatorStateInfo(0));
				while ((UITools.GetAnimatorNameHash(animator.GetCurrentAnimatorStateInfo(0)) == oldHashId) && (Time.realtimeSinceStartup < timeout)) {
					yield return null;
				}
				yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
			}
			isHiding = false;
			if (panel != null) Tools.SetGameObjectActive(panel, false);
		}
	}
		
}
#endif