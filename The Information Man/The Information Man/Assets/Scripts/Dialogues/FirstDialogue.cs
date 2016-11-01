using UnityEngine;
using System.Collections;

public class FirstDialogue : MonoBehaviour {
	public GameObject leftPanel;
	public GameObject rightPanel;
	public GameObject textPanel;
	public GameObject inputField;
	public Sprite leftPicture;
	public Sprite rightPicture;
	public GameObject player;

	private bool paused = false;

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.name == "player")
		{
			leftPanel.GetComponent<UnityEngine.UI.Image> ().sprite = leftPicture;
			rightPanel.GetComponent<UnityEngine.UI.Image> ().sprite = rightPicture;
			textPanel.GetComponent<UnityEngine.UI.Text>().text = "HA ZDAROVA SANYA";
			inputField.GetComponent<UnityEngine.UI.InputField> ().text = "Enter your answer ...";
			inputField.GetComponent<UnityEngine.UI.InputField> ().readOnly = false;
			player.GetComponent<Player> ().SetMove(false);
		}
	}

	void Start () {
	}

	public void GetInput(string guess) {
		if (guess == "Zdarova") {
			rightPanel.GetComponent<UnityEngine.UI.Image> ().sprite = null;
			inputField.GetComponent<UnityEngine.UI.InputField> ().readOnly = true;
			textPanel.GetComponent<UnityEngine.UI.Text> ().text = "";
			inputField.GetComponent<UnityEngine.UI.InputField> ().text = "";
			player.GetComponent<Player> ().SetMove(true);
		} else {
			inputField.GetComponent<UnityEngine.UI.InputField> ().text = "Answer is wrong!1!";
		}
	}

	void Update () {
		/*if (inputField.GetComponent<UnityEngine.UI.InputField> ().OnSubmit ()) {
			rightPanel.GetComponent<UnityEngine.UI.Image> ().sprite = null;
			inputField.GetComponent<UnityEngine.UI.InputField> ().readOnly = true;
			textPanel.GetComponent<UnityEngine.UI.Text>().text = "";
			player.GetComponent<Player> ().isPaused = false;
		}*/
	}
}
