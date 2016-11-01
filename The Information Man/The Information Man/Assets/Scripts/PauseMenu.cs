using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour {
	public GameObject PauseUI;

	private bool paused = false;

	public void Start()
	{
		PauseUI.SetActive(false);
	}

	public void Update()
	{
		if (Input.GetButtonDown ("Pause")) 
		{
			paused = !paused;
		}

		if (paused) 
		{
			PauseUI.SetActive (true);
			Time.timeScale = 0;
            Cursor.visible = true;
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
