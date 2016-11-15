using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public bool play = false;
    public bool settings = false;
    public bool quit = false;

    public Camera camera1;
    public Camera camera2;
    public Camera camera3;
    public GameObject grassObject;

    public bool back = false;
    public bool low = false;
    public bool medium = false;
    public bool high = false;
    public bool fantastic = false;
    public bool advancedSettings = false;

    public GameObject language;
    public GameObject lighting;
    public GameObject musicVolume;
    public GameObject superSampling;
    public GameObject motionBlur;
    public GameObject blood;
    public GameObject mode3D;
    public GameObject smoothing;
    public GameObject textures;
    public GameObject polygones;
    public GameObject effects;
    public GameObject postprocessing;
    public GameObject grid;
    public GameObject environment;
    public GameObject grass;
    public GameObject FXAA;
    public GameObject MSAA;
    public GameObject antialiasing;
    public GameObject alphaBlending;

    public GameObject resolution;
    public GameObject fullscreen;

    public void setLanguage()
    {
        language.GetComponent<TextMesh>().text = GameConfig.language;
    }

    public void setLighting()
    {
        lighting.GetComponent<TextMesh>().text = GameConfig.lighting;
    }

    public void setMusicVolume()
    {
        musicVolume.GetComponent<TextMesh>().text = (Mathf.RoundToInt(AudioListener.volume * 100)).ToString();
    }

    public void setSuperSampling()
    {
        superSampling.GetComponent<TextMesh>().text = GameConfig.superSampling;
    }

    public void setMotionBlur()
    {
        motionBlur.GetComponent<TextMesh>().text = GameConfig.motionBlur;
    }

    public void setBlood()
    {
        blood.GetComponent<TextMesh>().text = GameConfig.blood;
    }

    public void setMode3D()
    {
        mode3D.GetComponent<TextMesh>().text = GameConfig.mode3D;
    }

    public void setSmoothing()
    {
        smoothing.GetComponent<TextMesh>().text = GameConfig.smoothing;
    }

    public void setTextures()
    {
        textures.GetComponent<TextMesh>().text = GameConfig.textures;
    }

    public void setPolygones()
    {
        polygones.GetComponent<TextMesh>().text = GameConfig.polygones;
    }

    public void setEffects()
    {
        effects.GetComponent<TextMesh>().text = GameConfig.effects;
    }

    public void setPostprocessing()
    {
        postprocessing.GetComponent<TextMesh>().text = GameConfig.postprocessing;
    }

    public void setGrid()
    {
        grid.GetComponent<TextMesh>().text = GameConfig.grid;
    }

    public void setEnvironment()
    {
        environment.GetComponent<TextMesh>().text = GameConfig.environment;
    }

    public void setGrass()
    {
        grass.GetComponent<TextMesh>().text = GameConfig.grass;
    }

    public void setFXAA()
    {
        FXAA.GetComponent<TextMesh>().text = GameConfig.FXAA;
    }

    public void setMSAA()
    {
        MSAA.GetComponent<TextMesh>().text = GameConfig.MSAA;
    }

    public void setAntialiasing()
    {
        antialiasing.GetComponent<TextMesh>().text = GameConfig.antialiasing;
    }

    public void setAlphaBlending()
    {
        alphaBlending.GetComponent<TextMesh>().text = GameConfig.alphaBlending;
    }

    public void setResolution()
    {
        resolution.GetComponent<TextMesh>().text = GameConfig.resolutionWidth.ToString() + "x" + GameConfig.resolutionHeight.ToString();
    }

    public void setFullscreen()
    {
        fullscreen.GetComponent<TextMesh>().text = GameConfig.fullscreen;
    }

    public void getResolutions()
    {
        Resolution resolution1 = new Resolution();
        resolution1.width = 1024;
        resolution1.height = 576;
        Resolution resolution2 = new Resolution();
        resolution2.width = 1280;
        resolution2.height = 720;
        Resolution resolution3 = new Resolution();
        resolution3.width = 1366;
        resolution3.height = 768;
        Resolution resolution4 = new Resolution();
        resolution4.width = 1600;
        resolution4.height = 900;
        Resolution resolution5 = new Resolution();
        resolution5.width = 1920;
        resolution5.height = 1080;
        Resolution[] resolutions = Screen.resolutions;

        foreach (var res in resolutions)
        {
            Debug.Log(res);
            if (res.height == resolution1.height && res.width == resolution1.width)
            {
                GameConfig.resolutions[0] = true;
            }
            if (res.height == resolution2.height && res.width == resolution2.width)
            {
                GameConfig.resolutions[1] = true;
            }
            if (res.height == resolution3.height && res.width == resolution3.width)
            {
                GameConfig.resolutions[2] = true;
            }
            if (res.height == resolution4.height && res.width == resolution4.width)
            {
                GameConfig.resolutions[3] = true;
            }
            if (res.height == resolution5.height && res.width == resolution5.width)
            {
                GameConfig.resolutions[4] = true;
            }
        }
    }

    public void Start()
    {
        this.setLanguage();
        this.setLighting();
        this.setMusicVolume();
        this.setSuperSampling();
        this.setMotionBlur();
        this.setBlood();
        this.setMode3D();
        this.setSmoothing();
        this.setTextures();
        this.setPolygones();
        this.setEffects();
        this.setPostprocessing();
        this.setGrid();
        this.setEnvironment();
        this.setGrass();
        this.setFXAA();
        this.setMSAA();
        this.setAntialiasing();
        this.setAlphaBlending();
        this.setResolution();
        this.setFullscreen();
        this.getResolutions();
    }

    public void OnMouseEnter()
    {
        this.GetComponent<Renderer>().material.color = Color.red;
    }

    public void OnMouseExit()
    {
        this.GetComponent<Renderer>().material.color = Color.white;
    }

    public void OnMouseUp()
    {
        if (play)
        {
            Application.LoadLevel("preview");
        }
        if (settings)
        {
            camera1.enabled = false;
            camera2.enabled = true;
        }
        if (quit == true)
        {
            Application.Quit();
        }

        //=============
        if (back == true)
        {
            camera1.enabled = true;
            camera2.enabled = false;
            camera3.enabled = false;
            grassObject.SetActive(false);
        }
        if (low == true)
        {
            QualitySettings.currentLevel = QualityLevel.Simple;
        }
        if (medium == true)
        {
            QualitySettings.currentLevel = QualityLevel.Good;
        }
        if (high == true)
        {
            QualitySettings.currentLevel = QualityLevel.Beautiful;
        }
        if (fantastic == true)
        {
            QualitySettings.currentLevel = QualityLevel.Fantastic;
        }
        if (advancedSettings == true)
        {
            camera3.enabled = true;
            camera2.enabled = false;
        }
    }
}
