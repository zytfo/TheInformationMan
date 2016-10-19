#if !(UNITY_4_3 || UNITY_4_5)
using UnityEngine;
using UnityEngine.EventSystems;
using PixelCrushers.DialogueSystem;

namespace PixelCrushers.DialogueSystem {
	
	/// <summary>
	/// This script adds a key or button trigger to a UIButton.
	/// </summary>
	[AddComponentMenu("Dialogue System/UI/Unity UI/Effects/UI Button Key Trigger")]
	public class UIButtonKeyTrigger : MonoBehaviour {
		
		public KeyCode key = KeyCode.None;
		public string buttonName = string.Empty;

		private UnityEngine.UI.Button button = null;

		void Awake() {
			button = GetComponent<UnityEngine.UI.Button>();
			if (button == null) enabled = false;
		}

		void Update() {
			if (Input.GetKeyDown(key) || (!string.IsNullOrEmpty(buttonName) && Input.GetButtonDown(buttonName))) {
				var pointer = new PointerEventData(EventSystem.current);
				ExecuteEvents.Execute(button.gameObject, pointer, ExecuteEvents.submitHandler);
			}
		}

	}
	
}
#endif