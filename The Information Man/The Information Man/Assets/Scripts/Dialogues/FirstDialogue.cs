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
    private int dialogueStep = 0;
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
        if (dialogueStep == 0 && IsGreeting(guess))
        {
            ProcessDialogue(guess, "You've came to the interview?");
            dialogueStep++;
        }
        else if (dialogueStep == 1 && (guess == "yes" || guess == "Yes"))
        {
            ProcessDialogue(guess, "Cool! Can you tell me your name?");
            dialogueStep++;
        }
        else if (dialogueStep == 2 && guess == player.GetComponent<Player>().fullname)
        {
            ProcessDialogue(guess, "Good. I will give you a few tasks to check your skills.\nAre you ready for the first task?");
            dialogueStep++;
        }
        else if (dialogueStep == 3)
        {
            ProcessDialogue(guess, t1.WriteTask());
            dialogueStep++;
        }
        else if (dialogueStep == 4 && t1.CheckResult(guess, t1.writeAnswer) == 1)
        {
            ProcessDialogue(guess, "Surprisingly, correct! OK. Next task.");
            dialogueStep++;
        }
        else if (dialogueStep == 4 && t1.CheckResult(guess, t1.writeAnswer) < 1)
        {
            AnotherAttempt();
            ProcessDialogue(guess, "You're wrong! Try again!");
        }
        else if (dialogueStep == 5 && guess == "easy")
        {
            ProcessDialogue(guess, "Let's check.\n" + t2.WriteTask() + "\nRight answer is: " + t2.writeAnswer);
            dialogueStep++;
        }
        else if (dialogueStep == 6 && t2.CheckResult(guess, t2.writeAnswer) == 1)
        {
            DialogueSuccess(guess, "Basically, you're correct! Welcome to the Innopolis University!\nYou can go now.");
            dialogueStep++;
        }
        else if (dialogueStep == 6 && t2.CheckResult(guess, t2.writeAnswer) < 1)
        {
            AnotherAttempt();
            ProcessDialogue(guess, "You're wrong! Try again!");
        }
        else if (dialogueStep == 7 && t2.CheckResult(guess, t2.writeAnswer) < 1)
        {
            AnotherAttempt();
            ProcessDialogue(guess, "You're wrong! Try again!");
        }
        else if (guess == "") inputField.GetComponent<UnityEngine.UI.InputField>().ActivateInputField();
        else if (guess == "skip")
        {
            inputField.GetComponent<UnityEngine.UI.InputField>().text = "";
            player.GetComponent<Player>().hadDialogue1 = true;
            dialogueStep = 7;
        }
        else
        {
            textPanel.GetComponent<UnityEngine.UI.Text>().text += "\n" + player.GetComponent<Player>().fullname + ": " + guess;
            StartCoroutine(GameOver());
        }
    }

    void Update()
    {
        if ((dialogueStep == 3 || dialogueStep == 7) && (Input.GetKey("left") || Input.GetKey("right")))
        {
            rightPanel.GetComponent<UnityEngine.UI.Image>().sprite = null;
            inputField.GetComponent<UnityEngine.UI.InputField>().readOnly = true;
            //textPanel.GetComponent<UnityEngine.UI.Text>().text = "";
            inputField.GetComponent<UnityEngine.UI.InputField>().text = "";
            player.GetComponent<Player>().SetMove(true);
            dialogueStep = 0;
        }
        Remover();
    }

    void ProcessDialogue(string playerStr, string otherStr)
    {
        textPanel.GetComponent<UnityEngine.UI.Text>().text += "\n" + player.GetComponent<Player>().fullname + ": " + playerStr;
        textPanel.GetComponent<UnityEngine.UI.Text>().text += "\nMr. Silitti: " + otherStr;
        inputField.GetComponent<UnityEngine.UI.InputField>().text = "";
        inputField.GetComponent<UnityEngine.UI.InputField>().ActivateInputField();
        if (attempts == 0) StartCoroutine(GameOver());
    }

    void DialogueSuccess(string playerStr, string otherStr)
    {
        textPanel.GetComponent<UnityEngine.UI.Text>().text += "\n" + player.GetComponent<Player>().fullname + ": " + playerStr;
        textPanel.GetComponent<UnityEngine.UI.Text>().text += "\nMr. Silitti: " + otherStr;
        inputField.GetComponent<UnityEngine.UI.InputField>().text = "";
        player.GetComponent<Player>().hadDialogue1 = true;
    }

    void AnotherAttempt()
    {
        attempts--;
        if (textPanel.GetComponent<UnityEngine.UI.Text>().text.EndsWith("Try again!"))
        {
            int index = textPanel.GetComponent<UnityEngine.UI.Text>().text.LastIndexOf('\n');
            textPanel.GetComponent<UnityEngine.UI.Text>().text = textPanel.GetComponent<UnityEngine.UI.Text>().text.Remove(index);
            index = textPanel.GetComponent<UnityEngine.UI.Text>().text.LastIndexOf('\n');
            textPanel.GetComponent<UnityEngine.UI.Text>().text = textPanel.GetComponent<UnityEngine.UI.Text>().text.Remove(index);
        }
    }

    bool IsGreeting(string input) {
        return input.Contains("hi") || input.Contains("Hi") || input.Contains("hello") || input.Contains("Hello")
            || input.Contains("what's up") || input.Contains("What's up");
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

    IEnumerator GameOver()
    { 
        //yield return new WaitForSeconds(0.0f);
        textPanel.GetComponent<UnityEngine.UI.Text>().text += "\nMr. Silitti: Basically, it's a GAMEOVER.";
        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadScene("preview");
    }
}

