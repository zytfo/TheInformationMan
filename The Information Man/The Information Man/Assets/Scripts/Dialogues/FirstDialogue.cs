using UnityEngine;
using System;
using UnityEngine.UI;

public class FirstDialogue : MonoBehaviour {
    private Player player;
    private Text textPanel;
    private InputField inputField;
    public Sprite rightPicture;
    public BoxCollider2D box;

    private DialogueAPI api;

    void OnTriggerEnter2D(Collider2D other)
	{
		if (other.name == "player" && !player.hadDialogue[0])
		{
            api.DialogueStart(0, "Pr. Silitti", "Good Morning!", rightPicture);
        }
	}

	void Start () {
        api = GameObject.Find("DialoguePanel").GetComponent<DialogueAPI>();
        textPanel = api.textPanel;
        player = api.player;
        inputField = api.inputField;
        box.enabled = false;
    }

	public void GetInput(string guess) {
        if (guess == "") inputField.ActivateInputField();
        else if (api.dialogueStep == 0 && api.IsGreeting(guess))
        {
            api.ProcessDialogue(guess, "You've came to the interview?");
        }
        else if (api.dialogueStep == 1 && (guess == "yes" || guess == "Yes"))
        {
            api.ProcessDialogue(guess, "Cool! Can you tell me your name?");
        }
        else if (api.dialogueStep == 2)
        {
            if (guess == player.fullname
            || string.Equals(guess, "The Information Man", StringComparison.CurrentCultureIgnoreCase))
            {
                api.ProcessDialogue(guess, "Oh, I have you in my list! OK, I will give you a few tasks to check your skills.\n " + "Are you ready for the first task?");
            }
            else api.DialogueFail(guess, "I don't have you in my list. Don't waste my time anymore!");
        }
        else if (api.dialogueStep == 3)
        {
            api.task(new Tasks.SumTask());
            player.UpdateTaskPanel();
            api.ProcessDialogue(guess, "Your answer doesn't matter actually. Never mind. " + api.task().taskDescription);
        }
        else if (api.dialogueStep == 4)
        {
            if (api.task().CheckResult(guess, api.task().writeAnswer) == 1)
            {
                api.task(null);
                player.UpdateTaskPanel();
                api.ProcessDialogue(guess, "Surprisingly, correct! OK. Next task.");
            } 
            else
            {
                api.AnotherAttempt();
                api.ProcessDialogue(guess, "You're wrong! Try again!");
            }
        }
        else if (api.dialogueStep == 5)
        {
            api.task(new Tasks.ProbabilityTask());
            player.UpdateTaskPanel();
            api.ProcessDialogue(guess, "I see your happy face. That's cool! This one may require more time to succeed.\n" 
                + api.task().taskDescription);
        }
        else if (api.dialogueStep == 6)
        {
            if (api.task().CheckResult(guess, api.task().writeAnswer) == 1)
            {
                api.task(null);
                player.UpdateTaskPanel();
                api.ProcessDialogue(guess, "Basically, you're correct! And the last one: why do you want to study in Innopolis University?");
            }
            else
            {
                api.AnotherAttempt();
                api.ProcessDialogue(guess, "You're wrong! Try again!");
            }
        }
        else if (api.dialogueStep == 7 && guess.Length > 20 && guess.Split().Length > 4)
        {
            api.DialogueSuccess(guess, "Very interesting. I think it's enough for you. Welcome to this wonderful place!\n " 
                + "Dormitory manager is waiting for you. You are free to go.");
        }
        else if (api.dialogueStep == 8 && guess != "")
        {
            textPanel.text += "\n" + player.fullname + ": " + guess;
            inputField.text = "";
        }
        else if (guess == "skip")
        {
            inputField.text = "";
            player.hadDialogue[api.dialogueNumber] = true;
            api.dialogueStep = 8;
        }
        else
        {
            api.WrongInput(guess);
        }
    }

    void Update()
    {
        if (api.dialogueStep == 8 && (Input.GetKey("left") || Input.GetKey("right")))
        {
            api.rightPicture.sprite = null;
            inputField.readOnly = true;
            inputField.text = "";
            inputField.DeactivateInputField();
            player.SetMove(true);
            api.dialogueStep = -1;
            box.enabled = true;
        }
    }

}

