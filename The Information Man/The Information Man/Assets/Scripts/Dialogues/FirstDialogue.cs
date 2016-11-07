using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FirstDialogue : MonoBehaviour {
    private Player player;
    private Text textPanel;
    private InputField inputField;
    public Image rightPicture;

    private DialogueAPI api;

    private bool paused = false;
    private int dialogueStep = 0;

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
		if (other.name == "player" && !player.hadDialogue[0])
		{
            rightPicture = null;
            api.DialogueStart(0, "Mr.Silitti", "Good Morning!", rightPicture);
        }
	}

	void Start () {
        api = GameObject.Find("DialoguePanel").GetComponent<DialogueAPI>();
        textPanel = api.textPanel;
        player = api.player;
        inputField = api.inputField;
}

	public void GetInput(string guess) {
        if (dialogueStep == 0 && api.IsGreeting(guess))
        {
            api.ProcessDialogue(guess, "You've came to the interview?");
            dialogueStep++;
        }
        else if (dialogueStep == 1 && (guess == "yes" || guess == "Yes"))
        {
            api.ProcessDialogue(guess, "Cool! Can you tell me your name?");
            dialogueStep++;
        }
        else if (dialogueStep == 2 && (guess == player.fullname 
            || string.Equals(guess, "The Information Man", StringComparison.CurrentCultureIgnoreCase)))
        {
            api.ProcessDialogue(guess, "Good. I will give you a few tasks to check your skills.\nAre you ready for the first task?");
            dialogueStep++;
        }
        else if (dialogueStep == 3)
        {
            task(new Tasks.SumTask());
            player.UpdateTaskPanel();
            api.ProcessDialogue(guess, task().WriteTask());
            dialogueStep++;
        }
        else if (dialogueStep == 4 && task().CheckResult(guess, task().writeAnswer) == 1)
        {
            task(null);
            player.UpdateTaskPanel();
            api.ProcessDialogue(guess, "Surprisingly, correct! OK. Next task.");
            dialogueStep++;
        }
        else if (dialogueStep == 4 && task().CheckResult(guess, task().writeAnswer) < 1)
        {
            api.AnotherAttempt();
            api.ProcessDialogue(guess, "You're wrong! Try again!");
        }
        else if (dialogueStep == 5 && guess == "easy")
        {
            task(new Tasks.ProbabilityTask());
            player.UpdateTaskPanel();
            api.ProcessDialogue(guess, "Let's check.\n" + task().WriteTask() + "\nRight answer is: " + task().writeAnswer);
            dialogueStep++;
        }
        else if (dialogueStep == 6 && task().CheckResult(guess, task().writeAnswer) == 1)
        {
            task(null);
            player.UpdateTaskPanel();
            api.DialogueSuccess(guess, "Basically, you're correct! Welcome to the Innopolis University!\nYou can go now.");
            dialogueStep++;
        }
        else if (dialogueStep == 6 && task().CheckResult(guess, task().writeAnswer) < 1)
        {
            api.AnotherAttempt();
            api.ProcessDialogue(guess, "You're wrong! Try again!");
        }
        else if (guess == "") inputField.ActivateInputField();
        else if (guess == "skip")
        {
            inputField.text = "";
            player.hadDialogue[0] = true;
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
            //api.rightPicture = null;
            inputField.readOnly = true;
            inputField.text = "";
            api.livesPanel.text = "";
            player.SetMove(true);
            dialogueStep = 0;
        }
        api.Shifter();
        if (inputField.isFocused)
            api.livesPanel.text = (player.curHealth).ToString();
    }

    

    IEnumerator WaitSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

    IEnumerator GameOver()
    { 
        textPanel.text += "\nMr. Silitti: Basically, it's a GAMEOVER.";
        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadScene("MainMenu");
    }
}

