using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FirstDialogue : MonoBehaviour {
    public Player player;
    public Image leftPanel;
	public Image rightPanel;
	public Text textPanel;
    public Text livesPanel;
    public InputField inputField;
	public Sprite leftPicture;
	public Sprite rightPicture;

    private bool paused = false;
    private int dialogueStep = 0;
    private int maxLines = 8;

    Tasks.Task task()
    {
        return player.task;
    }

    void task(Tasks.Task task)
    {
        player.task = task;
    }

    void OnTriggerEnter2D(Collider2D other)
	{
		if (other.name == "player" && !player.hadDialogue1)
		{
            leftPanel.GetComponent<Image> ().sprite = leftPicture;
			rightPanel.GetComponent<Image> ().sprite = rightPicture;
			textPanel.GetComponent<Text>().text = "Mr. Silitti: Good Morning!";
            inputField.readOnly = false;
            inputField.ActivateInputField();
            player.GetComponent<Player> ().SetMove(false);
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
        else if (dialogueStep == 2 && (guess == player.fullname 
            || string.Equals(guess, "The Information Man", StringComparison.CurrentCultureIgnoreCase)))
        {
            ProcessDialogue(guess, "Good. I will give you a few tasks to check your skills.\nAre you ready for the first task?");
            dialogueStep++;
        }
        else if (dialogueStep == 3)
        {
            task(new Tasks.SumTask());
            player.UpdateTaskPanel();
            ProcessDialogue(guess, task().WriteTask());
            dialogueStep++;
        }
        else if (dialogueStep == 4 && task().CheckResult(guess, task().writeAnswer) == 1)
        {
            task(null);
            player.UpdateTaskPanel();
            ProcessDialogue(guess, "Surprisingly, correct! OK. Next task.");
            dialogueStep++;
        }
        else if (dialogueStep == 4 && task().CheckResult(guess, task().writeAnswer) < 1)
        {
            AnotherAttempt();
            ProcessDialogue(guess, "You're wrong! Try again!");
        }
        else if (dialogueStep == 5 && guess == "easy")
        {
            task(new Tasks.ProbabilityTask());
            player.UpdateTaskPanel();
            ProcessDialogue(guess, "Let's check.\n" + task().WriteTask() + "\nRight answer is: " + task().writeAnswer);
            dialogueStep++;
        }
        else if (dialogueStep == 6 && task().CheckResult(guess, task().writeAnswer) == 1)
        {
            task(null);
            player.UpdateTaskPanel();
            DialogueSuccess(guess, "Basically, you're correct! Welcome to the Innopolis University!\nYou can go now.");
            dialogueStep++;
        }
        else if (dialogueStep == 6 && task().CheckResult(guess, task().writeAnswer) < 1)
        {
            AnotherAttempt();
            ProcessDialogue(guess, "You're wrong! Try again!");
        }
        else if (guess == "") inputField.ActivateInputField();
        else if (guess == "skip")
        {
            inputField.text = "";
            player.hadDialogue1 = true;
            dialogueStep = 7;
        }
        else
        {
            textPanel.text += "\n" + player.fullname + ": " + guess;
            StartCoroutine(GameOver());
        }
    }

    void Update()
    {
        if ((dialogueStep == 3 || dialogueStep == 7) && (Input.GetKey("left") || Input.GetKey("right")))
        {
            rightPanel.sprite = null;
            inputField.readOnly = true;
            inputField.text = "";
            livesPanel.text = "";
            player.SetMove(true);
            dialogueStep = 0;
        }
        Shifter();
        if (inputField.isFocused)
            livesPanel.text = (player.curHealth).ToString();
    }

    void ProcessDialogue(string playerStr, string otherStr)
    {
        textPanel.text += "\n" + player.fullname + ": " + playerStr;
        textPanel.text += "\nMr. Silitti: " + otherStr;
        inputField.text = "";
        inputField.ActivateInputField();
        if (player.curHealth <= 0) StartCoroutine(GameOver());
    }

    void DialogueSuccess(string playerStr, string otherStr)
    {
        textPanel.text += "\n" + player.fullname + ": " + playerStr;
        textPanel.text += "\nMr. Silitti: " + otherStr;
        inputField.text = "";
        player.taskPanel.SetActive(false);
        player.hadDialogue1 = true;
        player.panelText = textPanel.text;
    }

    void AnotherAttempt()
    {
        //attempts--;
        player.curHealth -= 20;
        //livesPanel.text = (player.curHealth / 20).ToString();
        if (textPanel.text.EndsWith("Try again!"))
        {
            int index = textPanel.text.LastIndexOf('\n');
            textPanel.text = textPanel.text.Remove(index);
            index = textPanel.text.LastIndexOf('\n');
            textPanel.text = textPanel.text.Remove(index);
        }
    }

    bool IsGreeting(string input) {
        return input.Contains("hi") || input.Contains("Hi") || input.Contains("hello") || input.Contains("Hello")
            || input.Contains("what's up") || input.Contains("What's up");
    }

    void Shifter()
    {
        if (textPanel.text.Split('\n').Length > maxLines)
        {
            string result = textPanel.text;
            int count = textPanel.text.Split('\n').Length - maxLines;
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
            textPanel.text = result;
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
        textPanel.text += "\nMr. Silitti: Basically, it's a GAMEOVER.";
        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadScene("MainMenu");
    }
}

