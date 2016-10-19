#if !(UNITY_4_3 || UNITY_4_5)
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace PixelCrushers.DialogueSystem {
	
	/// <summary>
	/// Controls for UnityUIDialogueUI's alert message.
	/// </summary>
	[System.Serializable]
	public class UnityUIAlertControls : AbstractUIAlertControls {

		/// <summary>
		/// The panel containing the alert controls. A panel is optional, but you may want one
		/// so you can include a background image, panel-wide effects, etc.
		/// </summary>
		[Tooltip("Optional panel containing the alert line; can contain other doodads and effects, too")]
		public UnityEngine.UI.Graphic panel;
		
		/// <summary>
		/// The label used to show the alert message text.
		/// </summary>
		[Tooltip("Shows the alert message text")]
		public UnityEngine.UI.Text line;
		
		/// <summary>
		/// Optional continue button to close the alert immediately.
		/// </summary>
		[Tooltip("Optional continue button; configure OnClick to invoke dialogue UI's OnContinue method")]
		public UnityEngine.UI.Button continueButton;

		[Serializable]
		public class AnimationTransitions {
			public string showTrigger = "Show";
			public string hideTrigger = "Hide";
		}

		[Tooltip("Optional animation transitions; panel should have an Animator")]
		public AnimationTransitions animationTransitions = new AnimationTransitions();

		private bool isVisible = false;
		
		/// <summary>
		/// Is an alert currently showing?
		/// </summary>
		/// <value>
		/// <c>true</c> if showing; otherwise, <c>false</c>.
		/// </value>
		public override bool IsVisible { get { return isVisible; } }

		private Animator animator = null;

		private bool lookedForAnimator = false;
		
		/// <summary>
		/// Sets the alert controls active. If a hide animation is available, this method
		/// depends on the hide animation to hide the controls.
		/// </summary>
		/// <param name='value'>
		/// <c>true</c> for active.
		/// </param>
		public override void SetActive(bool value) {
			if (value == true) {
				Tools.SetGameObjectActive(panel, true);
				Tools.SetGameObjectActive(line, true);
				if (!isVisible && CanTriggerAnimations() && !string.IsNullOrEmpty(animationTransitions.showTrigger)) {
					animator.SetTrigger(animationTransitions.showTrigger);
				}
			} else {
				if (isVisible && CanTriggerAnimations() && !string.IsNullOrEmpty(animationTransitions.hideTrigger)) {
					animator.SetTrigger(animationTransitions.hideTrigger);
					DialogueManager.Instance.StartCoroutine(DisableAfterAnimation());
				} else {
					Tools.SetGameObjectActive(panel, false);
					Tools.SetGameObjectActive(line, false);
				}
			}
			isVisible = value;
		}

		private bool CanTriggerAnimations() {
			if ((animator == null) && !lookedForAnimator) {
				lookedForAnimator = true;
				if (panel != null) animator = panel.GetComponent<Animator>();
				if ((animator == null) && (line != null)) animator = line.GetComponent<Animator>();
			}
			return (animator != null) && (animationTransitions != null);
		}

		private IEnumerator DisableAfterAnimation() {
			if (animator != null) {
				const float maxWaitDuration = 10;
				float timeout = Time.realtimeSinceStartup + maxWaitDuration;
				var oldHashId = UITools.GetAnimatorNameHash(animator.GetCurrentAnimatorStateInfo(0));
				while ((UITools.GetAnimatorNameHash(animator.GetCurrentAnimatorStateInfo(0)) == oldHashId) && (Time.realtimeSinceStartup < timeout)) {
					yield return null;
				}
				yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
			}
			Tools.SetGameObjectActive(panel, false);
			Tools.SetGameObjectActive(line, false);
		}
		
		/// <summary>
		/// Sets the alert message and begins the fade in/out routine.
		/// </summary>
		/// <param name='message'>
		/// Alert message.
		/// </param>
		/// <param name='duration'>
		/// Duration to show message.
		/// </param>
		public override void SetMessage(string message, float duration) {
			if (!string.IsNullOrEmpty(message)) {
				alertDoneTime = DialogueTime.time + duration;
				if (line != null) line.text = FormattedText.Parse(message, DialogueManager.MasterDatabase.emphasisSettings).text;
				Show();
			} else {
				Hide();
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
		
	}
		
}
#endif