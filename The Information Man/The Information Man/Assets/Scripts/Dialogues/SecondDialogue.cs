using UnityEngine;
using System.Collections;

public class SecondDialogue : MonoBehaviour {
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
			textPanel.GetComponent<UnityEngine.UI.Text>().text = "Mr. Силити: Ti DUMAL CHTO уйдешь от меня! Где проект, Natural?";
			inputField.GetComponent<UnityEngine.UI.InputField> ().text = "";
			inputField.GetComponent<UnityEngine.UI.InputField> ().readOnly = false;
			player.GetComponent<Player> ().SetMove(false);
		}
	}

	void Start () {
	}

	public void GetInput(string guess) {
		int counter = 0;
		if (guess == "Voton" && counter == 0) {
			counter++;
			textPanel.GetComponent<UnityEngine.UI.Text> ().text += "\nMr. Sanya: Voton \nMr.Силити: Pshol otsuda!";
			inputField.GetComponent<UnityEngine.UI.InputField> ().text = "";
		}
		if (guess == "Idu") {
			textPanel.GetComponent<UnityEngine.UI.Text> ().text = "";
			rightPanel.GetComponent<UnityEngine.UI.Image> ().sprite = null;
			inputField.GetComponent<UnityEngine.UI.InputField> ().readOnly = true;
			inputField.GetComponent<UnityEngine.UI.InputField> ().text = "";
			player.GetComponent<Player> ().SetMove(true);
		}
	}
}
