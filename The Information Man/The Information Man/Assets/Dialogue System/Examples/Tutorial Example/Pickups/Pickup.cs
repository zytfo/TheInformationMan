using UnityEngine;
using PixelCrushers.DialogueSystem;

namespace PixelCrushers.DialogueSystem.Examples {

	public class Pickup : MonoBehaviour {

		public string message;

		public void OnTriggerEnter(Collider other) {
			DialogueManager.ShowAlert("Got " + name);
			Sequencer.Message(message);
			Destroy(gameObject);
		}

	}

}
