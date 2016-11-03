using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

public class FirstDialogue : MonoBehaviour {
	public GameObject leftPanel;
	public GameObject rightPanel;
	public GameObject textPanel;
	public GameObject inputField;
	public Sprite leftPicture;
	public Sprite rightPicture;
	public GameObject player;

    private bool paused = false;
    private Tasks.Task t1, t2;
    private int attempts = 3;
    private int counter = 0;
    private int maxLines = 8;

    void OnTriggerEnter2D(Collider2D other)
	{
		if (other.name == "player" && !player.GetComponent<Player>().hadDialogue1)
		{
            leftPanel.GetComponent<UnityEngine.UI.Image> ().sprite = leftPicture;
			rightPanel.GetComponent<UnityEngine.UI.Image> ().sprite = rightPicture;
			textPanel.GetComponent<UnityEngine.UI.Text>().text = "Mr. Silitti: Good Morning!";
			inputField.GetComponent<UnityEngine.UI.InputField> ().text = "";
			inputField.GetComponent<UnityEngine.UI.InputField> ().readOnly = false;
            inputField.GetComponent<UnityEngine.UI.InputField>().ActivateInputField();
            player.GetComponent<Player> ().SetMove(false);
            t1 = new Tasks.SumTask();
            t2 = new Tasks.ProbabilityTask();
        }
	}

	void Start () {
    }

	public void GetInput(string guess) {
        if (counter == 0 && guess != "" && string.Equals(guess, "zdarova", StringComparison.CurrentCultureIgnoreCase))
        {
            textPanel.GetComponent<UnityEngine.UI.Text>().text += "\n" + player.GetComponent<Player>().fullname + ": " + guess;
            textPanel.GetComponent<UnityEngine.UI.Text>().text += "\nMr. Silitti: You've came to the interview?";
            inputField.GetComponent<UnityEngine.UI.InputField>().text = "";
            inputField.GetComponent<UnityEngine.UI.InputField>().ActivateInputField();
            guess = "";
            counter++;
        }
        else if (counter == 1 && guess != "" && (guess == "yes" || guess == "Yes"))
        {
            textPanel.GetComponent<UnityEngine.UI.Text>().text += "\n" + player.GetComponent<Player>().fullname + ": " + guess;
            textPanel.GetComponent<UnityEngine.UI.Text>().text += "\nMr. Silitti: Cool! Can you tell me your name?";
            inputField.GetComponent<UnityEngine.UI.InputField>().text = "";
            inputField.GetComponent<UnityEngine.UI.InputField>().ActivateInputField();
            guess = "";
            counter++;
        }
        else if (counter == 2 && guess != "" && guess == player.GetComponent<Player>().fullname)
        {
            textPanel.GetComponent<UnityEngine.UI.Text>().text += "\n" + player.GetComponent<Player>().fullname + ": " + guess;
            textPanel.GetComponent<UnityEngine.UI.Text>().text += "\nMr. Silitti: Good. I will give you a few tasks to check your skills.\n"
                + "Are you ready for the first task?";
            inputField.GetComponent<UnityEngine.UI.InputField>().text = "";
            inputField.GetComponent<UnityEngine.UI.InputField>().ActivateInputField();
            guess = "";
            counter++;
        }
        else if (counter == 3 && guess != "")
        {
            textPanel.GetComponent<UnityEngine.UI.Text>().text += "\n" + player.GetComponent<Player>().fullname + ": " + guess;
            textPanel.GetComponent<UnityEngine.UI.Text>().text += "\nMr. Silitti: "
                + t1.WriteTask();
            inputField.GetComponent<UnityEngine.UI.InputField>().text = "";
            inputField.GetComponent<UnityEngine.UI.InputField>().ActivateInputField();
            guess = "";
            counter++;
        }
        else if (counter == 4 && t1.CheckResult(guess, t1.writeAnswer) == 1)
        {
            textPanel.GetComponent<UnityEngine.UI.Text>().text += "\n" + player.GetComponent<Player>().fullname + ": " + guess;
            textPanel.GetComponent<UnityEngine.UI.Text>().text += "\nMr. Silitti: Surprisingly, correct! OK. Next task.";
            inputField.GetComponent<UnityEngine.UI.InputField>().text = "";
            inputField.GetComponent<UnityEngine.UI.InputField>().ActivateInputField();
            guess = "";
            counter++;
        }
        else if (counter == 4 && guess != "" && t1.CheckResult(guess, t1.writeAnswer) < 1)
        {
            AnotherAttempt();
            textPanel.GetComponent<UnityEngine.UI.Text>().text += "\n" + player.GetComponent<Player>().fullname + ": " + guess;
            textPanel.GetComponent<UnityEngine.UI.Text>().text += "\nMr. Silitti: You're wrong! Try again!";
            inputField.GetComponent<UnityEngine.UI.InputField>().text = "";
            inputField.GetComponent<UnityEngine.UI.InputField>().ActivateInputField();
            attempts--;
            if (attempts < 0) StartCoroutine(GameOver(guess));
        }
        else if (counter == 5 && guess != "" && guess == "easy")
        {
            textPanel.GetComponent<UnityEngine.UI.Text>().text += "\n" + player.GetComponent<Player>().fullname + ": " + guess;
            textPanel.GetComponent<UnityEngine.UI.Text>().text += "\nMr. Silitti: Let's check.\n" + t2.WriteTask()
                + "\nRight answer is: " + t2.writeAnswer;
            inputField.GetComponent<UnityEngine.UI.InputField>().text = "";
            inputField.GetComponent<UnityEngine.UI.InputField>().ActivateInputField();
            guess = "";
            counter++;
        }
        else if (counter == 6 && guess != "" && t2.CheckResult(guess, t2.writeAnswer) == 1)
        {
            textPanel.GetComponent<UnityEngine.UI.Text>().text += "\n" + player.GetComponent<Player>().fullname + ": " + guess;
            textPanel.GetComponent<UnityEngine.UI.Text>().text += "\nMr. Silitti: Basically, you're correct! Welcome to the Innopolis University! "
                + "You can go now.";
            inputField.GetComponent<UnityEngine.UI.InputField>().text = "";
            player.GetComponent<Player>().hadDialogue1 = true;
            counter++;
        }
        else if (counter == 6 && guess != "" && t2.CheckResult(guess, t2.writeAnswer) < 1)
        {
            AnotherAttempt();
            textPanel.GetComponent<UnityEngine.UI.Text>().text += "\n" + player.GetComponent<Player>().fullname + ": " + guess;
            textPanel.GetComponent<UnityEngine.UI.Text>().text += "\nMr. Silitti: You're wrong! Try again!";
            inputField.GetComponent<UnityEngine.UI.InputField>().text = "";
            inputField.GetComponent<UnityEngine.UI.InputField>().ActivateInputField();
            attempts--;
            if (attempts < 0) StartCoroutine(GameOver(guess));
        }
        else if (guess == "") inputField.GetComponent<UnityEngine.UI.InputField>().ActivateInputField();
        else if (guess == "skip")
        {
            inputField.GetComponent<UnityEngine.UI.InputField>().text = "";
            player.GetComponent<Player>().hadDialogue1 = true;
            counter = 7;
        }
        else
        {
            textPanel.GetComponent<UnityEngine.UI.Text>().text += "\n" + player.GetComponent<Player>().fullname + ": " + guess;
            StartCoroutine(GameOver(guess));
        }
    }

    void Update()
    {
        if ((counter == 3 || counter == 7) && (Input.GetKey("left") || Input.GetKey("right")))
        {
            rightPanel.GetComponent<UnityEngine.UI.Image>().sprite = null;
            inputField.GetComponent<UnityEngine.UI.InputField>().readOnly = true;
            textPanel.GetComponent<UnityEngine.UI.Text>().text = "";
            inputField.GetComponent<UnityEngine.UI.InputField>().text = "";
            player.GetComponent<Player>().SetMove(true);
            counter = 0;
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

    IEnumerator WaitSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

    IEnumerator GameOver(string guess)
    { 
        yield return new WaitForSeconds(3.0f);
        textPanel.GetComponent<UnityEngine.UI.Text>().text += "\nMr. Silitti: Basically, it's a GAMEOVER.";
        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadScene("preview");
    }
}

