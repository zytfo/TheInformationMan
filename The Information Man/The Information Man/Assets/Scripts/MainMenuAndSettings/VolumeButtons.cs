using UnityEngine;
using System.Collections;

public class VolumeButtons : MonoBehaviour {
	public bool lessButton = false;
	public bool moreButton = false;

	public void OnMouseEnter() {
		this.GetComponent<Renderer>().material.color = Color.red;
	}

	public void OnMouseExit() {
		this.GetComponent<Renderer>().material.color = Color.white;
	}

	public void OnMouseUp() {
		if (lessButton) {
			AudioListener.volume -= 0.1f;
		}
		if (moreButton) {
			AudioListener.volume += 0.1f;
		}
	}
}
