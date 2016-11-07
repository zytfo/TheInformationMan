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
	public bool alphaBlending = false;
	public bool superSampling = false;

	public GameObject spotLight1;
	public GameObject spotLight2;
	public GameObject spotLight3;

	public GameObject menuText;

	public GameObject grassObject;

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

		if (mode3D) {
			string str = this.GetComponent<TextMesh> ().text;
			if (str == "On") {
				this.GetComponent<TextMesh> ().text = "Off";
				menuText.GetComponent<Transform> ().rotation = Quaternion.Euler(0, 0, 0);
			}
			if (str == "Off") {
				this.GetComponent<TextMesh> ().text = "On";

				menuText.GetComponent<Transform> ().rotation = Quaternion.Euler(20, -20, -20);
			}
		}

		if (smoothing) {
			string str = this.GetComponent<TextMesh> ().text;
			if (str == "On") {
				this.GetComponent<TextMesh> ().text = "Off";
			} else {
				this.GetComponent<TextMesh> ().text = "On";
			}
		}

		if (textures) {
			string str = this.GetComponent<TextMesh> ().text;
			if (str == "High") {
				this.GetComponent<TextMesh> ().text = "Ultra";
				QualitySettings.currentLevel = QualityLevel.Fantastic;
			}
			if (str == "Ultra") {
				this.GetComponent<TextMesh> ().text = "Low";
				QualitySettings.currentLevel = QualityLevel.Simple;
			}
			if (str == "Low") {
				this.GetComponent<TextMesh> ().text = "Average";
				QualitySettings.currentLevel = QualityLevel.Good;
			}
			if (str == "Average") {
				this.GetComponent<TextMesh> ().text = "High";
				QualitySettings.currentLevel = QualityLevel.Beautiful;
			}
		}

		if (polygones) {
			string str = this.GetComponent<TextMesh> ().text;
			if (str == "High") {
				this.GetComponent<TextMesh> ().text = "Ultra";
			}
			if (str == "Ultra") {
				this.GetComponent<TextMesh> ().text = "Low";
			}
			if (str == "Low") {
				this.GetComponent<TextMesh> ().text = "Average";
			}
			if (str == "Average") {
				this.GetComponent<TextMesh> ().text = "High";
			}
		}

		if (effects) {
			string str = this.GetComponent<TextMesh> ().text;
			if (str == "High") {
				this.GetComponent<TextMesh> ().text = "Ultra";
			}
			if (str == "Ultra") {
				this.GetComponent<TextMesh> ().text = "Low";
			}
			if (str == "Low") {
				this.GetComponent<TextMesh> ().text = "Average";
			}
			if (str == "Average") {
				this.GetComponent<TextMesh> ().text = "High";
			}
		}

		if (postprocessing) {
			string str = this.GetComponent<TextMesh> ().text;
			if (str == "High") {
				this.GetComponent<TextMesh> ().text = "Ultra";
			}
			if (str == "Ultra") {
				this.GetComponent<TextMesh> ().text = "Low";
			}
			if (str == "Low") {
				this.GetComponent<TextMesh> ().text = "Average";
			}
			if (str == "Average") {
				this.GetComponent<TextMesh> ().text = "High";
			}
		}

		if (grid) {
			string str = this.GetComponent<TextMesh> ().text;
			if (str == "High") {
				this.GetComponent<TextMesh> ().text = "Ultra";
			}
			if (str == "Ultra") {
				this.GetComponent<TextMesh> ().text = "Low";
			}
			if (str == "Low") {
				this.GetComponent<TextMesh> ().text = "Average";
			}
			if (str == "Average") {
				this.GetComponent<TextMesh> ().text = "High";
			}
		}

		if (environment) {
			string str = this.GetComponent<TextMesh> ().text;
			if (str == "High") {
				this.GetComponent<TextMesh> ().text = "Ultra";
			}
			if (str == "Ultra") {
				this.GetComponent<TextMesh> ().text = "Low";
			}
			if (str == "Low") {
				this.GetComponent<TextMesh> ().text = "Average";
			}
			if (str == "Average") {
				this.GetComponent<TextMesh> ().text = "High";
			}
		}

		if (grass) {
			string str = this.GetComponent<TextMesh> ().text;
			grassObject.SetActive(true);
			if (str == "High") {
				this.GetComponent<TextMesh> ().text = "Ultra";
				grassObject.GetComponent<SpriteRenderer>().color = Color.green;
			}
			if (str == "Ultra") {
				this.GetComponent<TextMesh> ().text = "Low";
				grassObject.GetComponent<SpriteRenderer>().color = Color.black;
			}
			if (str == "Low") {
				this.GetComponent<TextMesh> ().text = "Average";
				grassObject.GetComponent<SpriteRenderer>().color = Color.gray;
			}
			if (str == "Average") {
				this.GetComponent<TextMesh> ().text = "High";
				grassObject.GetComponent<SpriteRenderer>().color = Color.yellow;
			}
		}

		if (FXAA) {
			string str = this.GetComponent<TextMesh> ().text;
			if (str == "On") {
				this.GetComponent<TextMesh> ().text = "Off";
			} else {
				this.GetComponent<TextMesh> ().text = "On";
			}
		}

		if (MSAA) {
			string str = this.GetComponent<TextMesh> ().text;
			if (str == "On") {
				this.GetComponent<TextMesh> ().text = "Off";
			} else {
				this.GetComponent<TextMesh> ().text = "On";
			}
		}

		if (antialiasing) {
			string str = this.GetComponent<TextMesh> ().text;
			if (str == "On") {
				this.GetComponent<TextMesh> ().text = "Off";
			} else {
				this.GetComponent<TextMesh> ().text = "On";
			}
		}

		if (alphaBlending) {
			string str = this.GetComponent<TextMesh> ().text;
			if (str == "On") {
				this.GetComponent<TextMesh> ().text = "Off";
			} else {
				this.GetComponent<TextMesh> ().text = "On";
			}
		}

		if (superSampling) {
			string str = this.GetComponent<TextMesh> ().text;
			if (str == "On") {
				this.GetComponent<TextMesh> ().text = "Off";
			} else {
				this.GetComponent<TextMesh> ().text = "On";
			}
		}

	}
}










