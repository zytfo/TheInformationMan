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
    public bool resolution = false;
    public bool fullscreen = false;

	public GameObject spotLight1;
	public GameObject spotLight2;
	public GameObject spotLight3;

	public GameObject menuText1;
    public GameObject menuText2;
    public GameObject menuText3;

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
                GameConfig.language = "British English";

            } else {
				this.GetComponent<TextMesh> ().text = "USA English";
                GameConfig.language = "USA English";
            }
		}

		if (lighting) {
			string str = this.GetComponent<TextMesh> ().text;
			if (str == "High") {
				this.GetComponent<TextMesh> ().text = "Ultra";
				spotLight1.GetComponent<Light> ().intensity = 3;
				spotLight2.GetComponent<Light> ().intensity = 3;
				spotLight3.GetComponent<Light> ().intensity = 3;
                GameConfig.lighting = "Ultra";
			}
			if (str == "Ultra") {
				this.GetComponent<TextMesh> ().text = "Low";
				spotLight1.GetComponent<Light> ().intensity = 0.11f;
				spotLight2.GetComponent<Light> ().intensity = 0.11f;
				spotLight3.GetComponent<Light> ().intensity = 0.11f;
                GameConfig.lighting = "Low";
            }
			if (str == "Low") {
				this.GetComponent<TextMesh> ().text = "Average";
				spotLight1.GetComponent<Light> ().intensity = 1;
				spotLight2.GetComponent<Light> ().intensity = 1;
				spotLight3.GetComponent<Light> ().intensity = 1;
                GameConfig.lighting = "Average";
            }
			if (str == "Average") {
				this.GetComponent<TextMesh> ().text = "High";
				spotLight1.GetComponent<Light> ().intensity = 2;
				spotLight2.GetComponent<Light> ().intensity = 2;
				spotLight3.GetComponent<Light> ().intensity = 2;
                GameConfig.lighting = "High";
            }
		}

        if (superSampling)
        {
            string str = this.GetComponent<TextMesh>().text;
            if (str == "On")
            {
                this.GetComponent<TextMesh>().text = "Off";
                GameConfig.superSampling = "Off";
            }
            else
            {
                this.GetComponent<TextMesh>().text = "On";
                GameConfig.superSampling = "On";
            }
        }

        if (motionBlur) {
			string str = this.GetComponent<TextMesh> ().text;
			if (str == "On") {
				this.GetComponent<TextMesh> ().text = "Off";
                GameConfig.motionBlur = "Off";
            } else {
				this.GetComponent<TextMesh> ().text = "On";
                GameConfig.motionBlur = "On";
            }
		}

		if (blood) {
			string str = this.GetComponent<TextMesh> ().text;
			if (str == "On") {
				this.GetComponent<TextMesh> ().text = "Off";
				spotLight1.GetComponent<Light> ().color = Color.white;
				spotLight2.GetComponent<Light> ().color = Color.white;
				spotLight3.GetComponent<Light> ().color = Color.white;
                GameConfig.blood = "Off";
			}
			if (str == "Off") {
				this.GetComponent<TextMesh> ().text = "On";
				spotLight1.GetComponent<Light> ().color = Color.red;
				spotLight2.GetComponent<Light> ().color = Color.red;
				spotLight3.GetComponent<Light> ().color = Color.red;
                GameConfig.blood = "On";
            }
		}

		if (mode3D) {
			string str = this.GetComponent<TextMesh> ().text;
			if (str == "On") {
				this.GetComponent<TextMesh> ().text = "Off";
				menuText1.GetComponent<Transform>().rotation = Quaternion.Euler(0, 0, 0);
                menuText2.GetComponent<Transform>().rotation = Quaternion.Euler(0, 0, 0);
                menuText3.GetComponent<Transform>().rotation = Quaternion.Euler(0, 0, 0);
                GameConfig.mode3D = "Off";
            }
			if (str == "Off") {
				this.GetComponent<TextMesh> ().text = "On";
                menuText1.GetComponent<Transform>().rotation = Quaternion.Euler(20, -20, -20);
                menuText2.GetComponent<Transform>().rotation = Quaternion.Euler(20, -20, -20);
                menuText3.GetComponent<Transform> ().rotation = Quaternion.Euler(20, -20, -20);
                GameConfig.mode3D = "On";
            }
		}

		if (smoothing) {
			string str = this.GetComponent<TextMesh> ().text;
			if (str == "On") {
				this.GetComponent<TextMesh> ().text = "Off";
                GameConfig.smoothing = "Off";
            } else {
				this.GetComponent<TextMesh> ().text = "On";
                GameConfig.smoothing = "On";
            }
		}

		if (textures) {
			string str = this.GetComponent<TextMesh> ().text;
			if (str == "High") {
                this.GetComponent<TextMesh>().text = "Ultra";
                QualitySettings.currentLevel = QualityLevel.Fantastic;
                GameConfig.textures = "Ultra";
			}
			if (str == "Ultra") {
				this.GetComponent<TextMesh> ().text = "Low";
				QualitySettings.currentLevel = QualityLevel.Simple;
                GameConfig.textures = "Low";
            }
			if (str == "Low") {
				this.GetComponent<TextMesh> ().text = "Average";
				QualitySettings.currentLevel = QualityLevel.Good;
                GameConfig.textures = "Average";
            }
			if (str == "Average") {
				this.GetComponent<TextMesh> ().text = "High";
				QualitySettings.currentLevel = QualityLevel.Beautiful;
                GameConfig.textures = "High";
            }
		}

		if (polygones) {
			string str = this.GetComponent<TextMesh> ().text;
			if (str == "High") {
				this.GetComponent<TextMesh> ().text = "Ultra";
                GameConfig.polygones = "Ultra";
			}
			if (str == "Ultra") {
				this.GetComponent<TextMesh> ().text = "Low";
                GameConfig.polygones = "Low";
            }
			if (str == "Low") {
				this.GetComponent<TextMesh> ().text = "Average";
                GameConfig.polygones = "Average";
            }
			if (str == "Average") {
				this.GetComponent<TextMesh> ().text = "High";
                GameConfig.polygones = "High";
            }
		}

		if (effects) {
			string str = this.GetComponent<TextMesh> ().text;
			if (str == "High") {
				this.GetComponent<TextMesh> ().text = "Ultra";
                GameConfig.effects = "Ultra";
			}
			if (str == "Ultra") {
				this.GetComponent<TextMesh> ().text = "Low";
                GameConfig.effects = "Low";
            }
			if (str == "Low") {
				this.GetComponent<TextMesh> ().text = "Average";
                GameConfig.effects = "Average";
            }
			if (str == "Average") {
				this.GetComponent<TextMesh> ().text = "High";
                GameConfig.effects = "High";
            }
		}

		if (postprocessing) {
			string str = this.GetComponent<TextMesh> ().text;
			if (str == "High") {
				this.GetComponent<TextMesh> ().text = "Ultra";
                GameConfig.postprocessing = "Ultra";
			}
			if (str == "Ultra") {
				this.GetComponent<TextMesh> ().text = "Low";
                GameConfig.postprocessing = "Low";
            }
			if (str == "Low") {
				this.GetComponent<TextMesh> ().text = "Average";
                GameConfig.postprocessing = "Average";
            }
			if (str == "Average") {
				this.GetComponent<TextMesh> ().text = "High";
                GameConfig.postprocessing = "High";
            }
		}

		if (grid) {
			string str = this.GetComponent<TextMesh> ().text;
			if (str == "High") {
				this.GetComponent<TextMesh> ().text = "Ultra";
                GameConfig.grid = "Ultra";
            }
			if (str == "Ultra") {
				this.GetComponent<TextMesh> ().text = "Low";
                GameConfig.grid = "Low";
            }
			if (str == "Low") {
				this.GetComponent<TextMesh> ().text = "Average";
                GameConfig.grid = "Average";
            }
			if (str == "Average") {
				this.GetComponent<TextMesh> ().text = "High";
                GameConfig.grid = "High";
            }
		}

		if (environment) {
			string str = this.GetComponent<TextMesh> ().text;
			if (str == "High") {
				this.GetComponent<TextMesh> ().text = "Ultra";
                GameConfig.environment = "Ultra";
            }
			if (str == "Ultra") {
				this.GetComponent<TextMesh> ().text = "Low";
                GameConfig.environment = "Low";
            }
			if (str == "Low") {
				this.GetComponent<TextMesh> ().text = "Average";
                GameConfig.environment = "Average";
            }
			if (str == "Average") {
				this.GetComponent<TextMesh> ().text = "High";
                GameConfig.environment = "High";
            }
		}

		if (grass) {
			string str = this.GetComponent<TextMesh> ().text;
			grassObject.SetActive(true);
			if (str == "High") {
				this.GetComponent<TextMesh> ().text = "Ultra";
				grassObject.GetComponent<SpriteRenderer>().color = Color.green;
                GameConfig.grass = "Ultra";
			}
			if (str == "Ultra") {
				this.GetComponent<TextMesh> ().text = "Low";
				grassObject.GetComponent<SpriteRenderer>().color = Color.black;
                GameConfig.grass = "Low";
            }
			if (str == "Low") {
				this.GetComponent<TextMesh> ().text = "Average";
				grassObject.GetComponent<SpriteRenderer>().color = Color.gray;
                GameConfig.grass = "Average";
            }
			if (str == "Average") {
				this.GetComponent<TextMesh> ().text = "High";
				grassObject.GetComponent<SpriteRenderer>().color = Color.yellow;
                GameConfig.grass = "High";
            }
		}

		if (FXAA) {
			string str = this.GetComponent<TextMesh> ().text;
			if (str == "On") {
				this.GetComponent<TextMesh> ().text = "Off";
                GameConfig.FXAA = "Off";
			} else {
				this.GetComponent<TextMesh> ().text = "On";
                GameConfig.FXAA = "On";
            }
		}

		if (MSAA) {
			string str = this.GetComponent<TextMesh> ().text;
			if (str == "On") {
				this.GetComponent<TextMesh> ().text = "Off";
                GameConfig.MSAA = "Off";
            } else {
				this.GetComponent<TextMesh> ().text = "On";
                GameConfig.MSAA = "On";
            }
		}

		if (antialiasing) {
			string str = this.GetComponent<TextMesh> ().text;
			if (str == "0") {
				this.GetComponent<TextMesh> ().text = "2";
                QualitySettings.antiAliasing = 2;
                GameConfig.antialiasing = "2";
			}
            if (str == "2")
            {
                this.GetComponent<TextMesh>().text = "4";
                QualitySettings.antiAliasing = 4;
                GameConfig.antialiasing = "4";
            }
            if (str == "4")
            {
                this.GetComponent<TextMesh>().text = "8";
                QualitySettings.antiAliasing = 8;
                GameConfig.antialiasing = "8";
            }
            if (str == "8")
            {
                this.GetComponent<TextMesh>().text = "0";
                QualitySettings.antiAliasing = 0;
                GameConfig.antialiasing = "0";
            }
        }

		if (alphaBlending) {
			string str = this.GetComponent<TextMesh> ().text;
			if (str == "On") {
				this.GetComponent<TextMesh> ().text = "Off";
                GameConfig.alphaBlending = "Off";
			} else {
				this.GetComponent<TextMesh> ().text = "On";
                GameConfig.alphaBlending = "On";
			}
		}

        if (resolution)
        {
            string str = this.GetComponent<TextMesh>().text;
            bool flag = true;
            while (flag)
            {
                if (str == "1600x900")
                {
                    if (GameConfig.resolutions[4])
                    {
                        Screen.SetResolution(1920, 1080, Screen.fullScreen);
                        GameConfig.resolutionWidth = 1920;
                        GameConfig.resolutionHeight = 1080;
                        this.GetComponent<TextMesh>().text = GameConfig.resolutionWidth.ToString() + "x" + GameConfig.resolutionHeight.ToString();
                        flag = false;
                    }
                    else
                    {
                        str = "1920x1080";
                    }
                }
                if (str == "1920x1080")
                {
                    if (GameConfig.resolutions[0])
                    {
                        Screen.SetResolution(1024, 576, Screen.fullScreen);
                        GameConfig.resolutionWidth = 1024;
                        GameConfig.resolutionHeight = 576;
                        this.GetComponent<TextMesh>().text = GameConfig.resolutionWidth.ToString() + "x" + GameConfig.resolutionHeight.ToString();
                        flag = false;
                    }
                    else
                    {
                        str = "1024x576";
                    }
                }
                if (str == "1024x576")
                {
                    if (GameConfig.resolutions[1])
                    {
                        Screen.SetResolution(1280, 720, Screen.fullScreen);
                        GameConfig.resolutionWidth = 1280;
                        GameConfig.resolutionHeight = 720;
                        this.GetComponent<TextMesh>().text = GameConfig.resolutionWidth.ToString() + "x" + GameConfig.resolutionHeight.ToString();
                        flag = false;
                    }
                    else
                    {
                        str = "1280x720";
                    }
                }
                if (str == "1280x720")
                {
                    if (GameConfig.resolutions[2])
                    {
                        Screen.SetResolution(1366, 768, Screen.fullScreen);
                        GameConfig.resolutionWidth = 1366;
                        GameConfig.resolutionHeight = 768;
                        this.GetComponent<TextMesh>().text = GameConfig.resolutionWidth.ToString() + "x" + GameConfig.resolutionHeight.ToString();
                        flag = false;
                    }
                    else
                    {
                        str = "1366x768";
                    }
                }
                if (str == "1366x768")
                {
                    if (GameConfig.resolutions[3])
                    {
                        Screen.SetResolution(1600, 900, Screen.fullScreen);
                        GameConfig.resolutionWidth = 1600;
                        GameConfig.resolutionHeight = 900;
                        this.GetComponent<TextMesh>().text = GameConfig.resolutionWidth.ToString() + "x" + GameConfig.resolutionHeight.ToString();
                        flag = false;
                    }
                    else
                    {
                        str = "1600x900";
                    }
                }
            }
        }

        if (fullscreen)
        {
            string str = this.GetComponent<TextMesh>().text;
            if (str == "On")
            {
                this.GetComponent<TextMesh>().text = "Off";
                GameConfig.fullscreen = "Off";
                Screen.fullScreen = false;
            }
            if (str == "Off")
            {
                this.GetComponent<TextMesh>().text = "On";
                GameConfig.fullscreen = "On";
                Screen.fullScreen = true;
            }
        }

    }
}










