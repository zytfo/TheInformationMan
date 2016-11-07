using UnityEngine;
using System.Collections;

public class AdvancedSettings : MonoBehaviour {
	public bool language = false;
	public bool lighting = false;
	public bool musicVolume = false;
	public bool soundVolume = false;
	public bool motionBlur = false;
	public bool blood = false;
	public bool mode3D = false;
	public bool smoothing = false;
	public bool textures = false;
	public bool polygones = false;
	public bool effects = false;
	public bool postprocessing = false;
	public bool grid = false;
	public bool environment = false;
	public bool grass = false;
	public bool FXAA = false;
	public bool MSAA = false;
	public bool antialiasing = false;
	public bool alfaBlending = false;
	public bool superSampling = false;

	public GameObject spotLight1;
	public GameObject spotLight2;
	public GameObject spotLight3;

	public void OnMouseEnter() {
		this.GetComponent<Renderer>().material.color = Color.red;
	}

	public void OnMouseExit() {
		this.GetComponent<Renderer>().material.color = Color.white;
	}

	public void OnMouseUp() {
		if (language) {
			string str = this.GetComponent<TextMesh> ().text;
			if (str == "USA English") {
				this.GetComponent<TextMesh> ().text = "British English";
			} else {
				this.GetComponent<TextMesh> ().text = "USA English";
			}
		}

		if (lighting) {
			string str = this.GetComponent<TextMesh> ().text;
			if (str == "High") {
				this.GetComponent<TextMesh> ().text = "Ultra";
				spotLight1.GetComponent<Light> ().intensity = 3;
				spotLight2.GetComponent<Light> ().intensity = 3;
				spotLight3.GetComponent<Light> ().intensity = 3;
			}
			if (str == "Ultra") {
				this.GetComponent<TextMesh> ().text = "Low";
				spotLight1.GetComponent<Light> ().intensity = 0.11f;
				spotLight2.GetComponent<Light> ().intensity = 0.11f;
				spotLight3.GetComponent<Light> ().intensity = 0.11f;
			}
			if (str == "Low") {
				this.GetComponent<TextMesh> ().text = "Average";
				spotLight1.GetComponent<Light> ().intensity = 1;
				spotLight2.GetComponent<Light> ().intensity = 1;
				spotLight3.GetComponent<Light> ().intensity = 1;
			}
			if (str == "Average") {
				this.GetComponent<TextMesh> ().text = "High";
				spotLight1.GetComponent<Light> ().intensity = 2;
				spotLight2.GetComponent<Light> ().intensity = 2;
				spotLight3.GetComponent<Light> ().intensity = 2;
			}
		}

		if (motionBlur) {
			string str = this.GetComponent<TextMesh> ().text;
			if (str == "On") {
				this.GetComponent<TextMesh> ().text = "Off";
			} else {
				this.GetComponent<TextMesh> ().text = "On";
			}
		}

		if (blood) {
			string str = this.GetComponent<TextMesh> ().text;
			if (str == "On") {
				this.GetComponent<TextMesh> ().text = "Off";
				spotLight1.GetComponent<Light> ().color = Color.white;
				spotLight2.GetComponent<Light> ().color = Color.white;
				spotLight3.GetComponent<Light> ().color = Color.white;
			}
			if (str == "Off") {
				this.GetComponent<TextMesh> ().text = "On";
				spotLight1.GetComponent<Light> ().color = Color.red;
				spotLight2.GetComponent<Light> ().color = Color.red;
				spotLight3.GetComponent<Light> ().color = Color.red;
			}
		}
	}
}










