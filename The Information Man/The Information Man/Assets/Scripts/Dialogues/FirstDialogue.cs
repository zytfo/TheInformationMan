using UnityEngine;
using System.Collections;
using System;

public class FirstDialogue : MonoBehaviour {
	public GameObject leftPanel;
	public GameObject rightPanel;
	public GameObject textPanel;
	public GameObject inputField;
	public Sprite leftPicture;
	public Sprite rightPicture;
	public GameObject player;

    private bool paused = false;
    private Tasks.Task t1;
    private int counter = 0;
    private int maxLines = 8;

    void OnTriggerEnter2D(Collider2D other)
	{
		if (other.name == "player" && !player.GetComponent<Player>().hadDialogue1)
		{
            leftPanel.GetComponent<UnityEngine.UI.Image> ().sprite = leftPicture;
			rightPanel.GetComponent<UnityEngine.UI.Image> ().sprite = rightPicture;
			textPanel.GetComponent<UnityEngine.UI.Text>().text = "Mr. Silitti: HA, ZDAROVA SANYA";
			inputField.GetComponent<UnityEngine.UI.InputField> ().text = "";
			inputField.GetComponent<UnityEngine.UI.InputField> ().readOnly = false;
            inputField.GetComponent<UnityEngine.UI.InputField>().ActivateInputField();
            player.GetComponent<Player> ().SetMove(false);
            t1 = new Tasks.ProbabilityTask();
        }
	}

	void Start () {
    }

	public void GetInput(string guess) {
        if (counter == 0 && string.Equals(guess, "zdarova", StringComparison.CurrentCultureIgnoreCase)) {
            textPanel.GetComponent<UnityEngine.UI.Text> ().text += "\nMr. Sanya: " + guess;
            textPanel.GetComponent<UnityEngine.UI.Text>().text += "\nMr. Silitti: Are you ready for your first task?";
			inputField.GetComponent<UnityEngine.UI.InputField> ().text = "";
            inputField.GetComponent<UnityEngine.UI.InputField>().ActivateInputField();
            guess = "";
            counter++;
        }
        if (counter == 1 && guess != "")
        {
            textPanel.GetComponent<UnityEngine.UI.Text>().text += "\nMr. Sanya: " + guess;
            textPanel.GetComponent<UnityEngine.UI.Text>().text += "\nMr. Silitti: "
                + t1.WriteTask() + "\nWrite answer is: " + t1.writeAnswer;
            inputField.GetComponent<UnityEngine.UI.InputField>().text = "";
            inputField.GetComponent<UnityEngine.UI.InputField>().ActivateInputField();
            guess = "";
            counter++;
        }
        if (counter == 2 && t1.CheckResult(guess, t1.writeAnswer) == 1)
        {
            textPanel.GetComponent<UnityEngine.UI.Text>().text += "\nMr. Sanya: " + guess;
            textPanel.GetComponent<UnityEngine.UI.Text>().text += "\nMr. Silitti: Basically, you're correct! You can go now.";
            inputField.GetComponent<UnityEngine.UI.InputField>().text = "";
            player.GetComponent<Player>().hadDialogue1 = true;
            counter++;
        } else if (counter == 2 && guess != "" && t1.CheckResult(guess, t1.writeAnswer) < 1)
        {
            AnotherAttempt();
            textPanel.GetComponent<UnityEngine.UI.Text>().text += "\nMr. Sanya: " + guess;
            textPanel.GetComponent<UnityEngine.UI.Text>().text += "\nMr. Silitti: You're wrong! Try again!";
            inputField.GetComponent<UnityEngine.UI.InputField>().text = "";
            inputField.GetComponent<UnityEngine.UI.InputField>().ActivateInputField();
        }
        if (guess == "") inputField.GetComponent<UnityEngine.UI.InputField>().ActivateInputField();
    }

    void Update()
    {
        if (counter == 3 && (Input.GetKey("left") || Input.GetKey("right")))
        {
            rightPanel.GetComponent<UnityEngine.UI.Image>().sprite = null;
            inputField.GetComponent<UnityEngine.UI.InputField>().readOnly = true;
            textPanel.GetComponent<UnityEngine.UI.Text>().text = "";
            inputField.GetComponent<UnityEngine.UI.InputField>().text = "";
            player.GetComponent<Player>().SetMove(true);
        }
        Remover();
    }

    void AnotherAttempt()
    {
        if (textPanel.GetComponent<UnityEngine.UI.Text>().text.EndsWith("Try again!"))
        {
            int index = textPanel.GetComponent<UnityEngine.UI.Text>().text.LastIndexOf('\n');
            textPanel.GetComponent<UnityEngine.UI.Text>().text = textPanel.GetComponent<UnityEngine.UI.Text>().text.Remove(index);
            index = textPanel.GetComponent<UnityEngine.UI.Text>().text.LastIndexOf('\n');
            textPanel.GetComponent<UnityEngine.UI.Text>().text = textPanel.GetComponent<UnityEngine.UI.Text>().text.Remove(index);
        }
    }

    void Remover()
    {
        if (textPanel.GetComponent<UnityEngine.UI.Text>().text.Split('\n').Length > maxLines)
        {
            string result = textPanel.GetComponent<UnityEngine.UI.Text>().text;
            int count = textPanel.GetComponent<UnityEngine.UI.Text>().text.Split('\n').Length - maxLines;
            for (int i = 0; i < count; ++i)
            {
                bool found = false;
                int j = 0;
                while (!found && j < result.Length)
                {
                    if (result[j] == '\n')
                    {
                        found = true;
                        result = result.Substring(j + 1);
                    }
                    j++;
                }
            }
            textPanel.GetComponent<UnityEngine.UI.Text>().text = result;
        }
        else return;
    }
}

