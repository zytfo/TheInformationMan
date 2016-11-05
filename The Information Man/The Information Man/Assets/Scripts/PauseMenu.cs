using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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
    }

	public void Update()
	{
		if (Input.GetButtonDown ("Pause")) 
		{
            bool isFocused = inputField.isFocused;
            if (!paused)
            {
                player.inputText = inputField.text;
                inputField.enabled = false;
            }
            else
            {
                inputField.enabled = true;
                inputField.text = player.inputText;
                if (isFocused) inputField.ActivateInputField();
            }
            paused = !paused;
        }

		if (paused) 
		{
			PauseUI.SetActive (true);
			Time.timeScale = 0;
            Cursor.visible = true;
            player.taskPanel.SetActive(false);
        }

		if (!paused) 
		{
			PauseUI.SetActive (false);
			Time.timeScale = 1;
        }
	}

	public void Resume() {
		paused = false;
	}

	public void Restart() {
		Application.LoadLevel ("preview");
	}

	public void MainMenu() {
		Application.LoadLevel ("MainMenu");
	}

	public void Quit() {
		Application.Quit ();
	}
}
