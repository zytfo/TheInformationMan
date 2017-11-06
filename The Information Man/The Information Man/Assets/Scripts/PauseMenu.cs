using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {
	public GameObject PauseUI;
    private Player player;
    private GameObject dialoguePanel;
    private InputField inputField;

    private bool paused = false;

	public void Start()
	{
		PauseUI.SetActive(false);
        player = FindObjectOfType<Player>();
        inputField = player.dialoguePanel.GetComponentInChildren<InputField>();
        player.inputText = inputField.text;
        paused = false;
    }

	public void Update()
	{
		if (Input.GetKeyDown (KeyCode.Escape)) 
		{
            paused = !paused;
            bool isFocused = inputField.isFocused;

            if (paused)
            {
                PauseUI.SetActive(true);
                Time.timeScale = 0;
                Cursor.visible = true;
                player.taskPanel.SetActive(false);

                player.inputText = inputField.text;
                inputField.enabled = false;
            }

            if (!paused)
            {
                PauseUI.SetActive(false);
                Cursor.visible = false;
                Time.timeScale = 1;

                inputField.enabled = true;
                inputField.text = player.inputText;
                if (isFocused) inputField.ActivateInputField();
            }
        }
	}

	public void Resume() {
		paused = false;
        PauseUI.SetActive(false);
        Cursor.visible = false;
        Time.timeScale = 1;
        inputField.enabled = true;
        inputField.text = player.inputText;
    }

	public void Restart() {
        SceneManager.LoadScene("MainMenu");
	}

	public void MainMenu() {
        SceneManager.LoadScene("MainMenu");
	}

	public void Quit() {
		Application.Quit ();
	}
}
