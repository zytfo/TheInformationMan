using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour
{

    public GameObject PauseUI;

    private bool paused = false;

    void Start()
    {
        PauseUI.SetActive(false);
    }

    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            paused = !paused;
        }

        if (paused)
        {
            PauseUI.SetActive(true);

            if (Input.GetAxis("Horizontal") < -0.1f)
            {
                PauseUI.transform.localScale = new Vector3(-1, 1, 1);
            }
            if (Input.GetAxis("Horizontal") > 0.1f)
            {
                PauseUI.transform.localScale = new Vector3(1, 1, 1);
            }
            
            Time.timeScale = 0;
        }

        if (!paused)
        {
            PauseUI.SetActive(false);
            Time.timeScale = 1;
        }
    }

    public void Resume() 
    {
        paused = false;
    }

    public void Restart()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

    public void MainMenu()
    {
        Application.LoadLevel(0);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
