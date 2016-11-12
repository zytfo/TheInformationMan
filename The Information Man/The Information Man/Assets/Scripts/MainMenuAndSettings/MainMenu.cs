using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public bool play = false;
    public bool settings = false;
    public bool quit = false;

    public Camera camera1;
    public Camera camera2;
    public Camera camera3;
    public GameObject resolution;
    public GameObject fullscreen;

    public bool back = false;
    public bool low = false;
    public bool medium = false;
    public bool high = false;
    public bool fantastic = false;
    public bool advancedSettings = false;

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
            resolution.GetComponent<TextMesh>().text = Screen.currentResolution.width.ToString() + "x" + Screen.currentResolution.height.ToString();
            if (Screen.fullScreen)
            {
                fullscreen.GetComponent<TextMesh>().text = "On";
            }
            else
            {
                fullscreen.GetComponent<TextMesh>().text = "Off";
            }
        }
    }
}
