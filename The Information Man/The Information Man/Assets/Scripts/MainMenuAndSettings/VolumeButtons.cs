using UnityEngine;
using System.Collections;

public class VolumeButtons : MonoBehaviour {
	public bool lessButton = false;
	public GameObject valuePanel;
	public bool moreButton = false;

	public void OnMouseEnter() {
		this.GetComponent<Renderer>().material.color = Color.red;
	}

	public void OnMouseExit() {
		this.GetComponent<Renderer>().material.color = Color.white;
	}

	public void OnMouseUp() {
		if (lessButton && valuePanel.GetComponent<TextMesh>().text != "0") {
			AudioListener.volume -= 0.1f;
			int value = int.Parse(valuePanel.GetComponent<TextMesh> ().text);
			value -= 10;
			valuePanel.GetComponent<TextMesh> ().text = value.ToString(); 
		}
		if (moreButton && valuePanel.GetComponent<TextMesh>().text != "100") {
			AudioListener.volume += 0.1f;
			int value = int.Parse(valuePanel.GetComponent<TextMesh> ().text);
			value += 10;
			valuePanel.GetComponent<TextMesh> ().text = value.ToString();  
		}
	}
}
