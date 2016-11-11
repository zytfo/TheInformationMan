using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class FirstDialogue : MonoBehaviour {
    private Player player;
    private Text textPanel;
    private InputField inputField;
    public Sprite rightPicture;
    public BoxCollider2D box;

    private DialogueAPI api;

    private bool paused = false;
    private int dialogueStep = 0;

    void OnTriggerEnter2D(Collider2D other)
	{
		if (other.name == "player" && !player.hadDialogue[0])
		{
            api.DialogueStart(0, "Mr.Silitti", "Good Morning!", rightPicture);
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
        else if (dialogueStep == 0 && api.IsGreeting(guess))
        {
            api.ProcessDialogue(guess, "You've came to the interview?");
            dialogueStep++;
        }
        else if (dialogueStep == 1 && (guess == "yes" || guess == "Yes"))
        {
            api.ProcessDialogue(guess, "Cool! Can you tell me your name?");
            dialogueStep++;
        }
        else if (dialogueStep == 2)
        {
            if (guess == player.fullname
            || string.Equals(guess, "The Information Man", StringComparison.CurrentCultureIgnoreCase))
            {
                api.ProcessDialogue(guess, "Oh, I have you in my lisk!. OK, I will give you a few tasks to check your\n skills. "
                + "Are you ready for the first task?");
                dialogueStep++;
            }
            else api.DialogueFail(guess, "I don't have you in my list. Don't waste my time anymore!");
        }
        else if (dialogueStep == 3)
        {
            api.task(new Tasks.SumTask());
            player.UpdateTaskPanel();
            api.ProcessDialogue(guess, "Your answer doesn't matter actually. Never mind. " + api.task().WriteTask());
            dialogueStep++;
        }
        else if (dialogueStep == 4)
        {
            if (api.task().CheckResult(guess, api.task().writeAnswer) == 1)
            {
                api.task(null);
                player.UpdateTaskPanel();
                api.ProcessDialogue(guess, "Surprisingly, correct! OK. Next task.");
                dialogueStep++;
            } 
            else
            {
                api.AnotherAttempt();
                api.ProcessDialogue(guess, "You're wrong! Try again!");
            }
        }
        else if (dialogueStep == 5)
        {
            api.task(new Tasks.ProbabilityTask());
            player.UpdateTaskPanel();
            api.ProcessDialogue(guess, "I see your happy face. That's cool! This one may require more time to succeed.\n" 
                + api.task().WriteTask());
            dialogueStep++;
        }
        else if (dialogueStep == 6)
        {
            if (api.task().CheckResult(guess, api.task().writeAnswer) == 1)
            {
                api.task(null);
                player.UpdateTaskPanel();
                api.ProcessDialogue(guess, "Basically, you're correct! And the last question: why do you want to study \nin Innopolis University?");
                dialogueStep++;
            }
            else
            {
                api.AnotherAttempt();
                api.ProcessDialogue(guess, "You're wrong! Try again!");
            }
        }
        else if (dialogueStep == 7 && guess.Length > 40 && guess.Split().Length > 5)
        {
            api.DialogueSuccess(guess, "Very interesting. I think it's enough for you. Welcome to this wonderful place!\n" 
                + "Dormitory manager is waiting for you. You are free to go.");
            dialogueStep++;
        }
        else if (dialogueStep == 8 && guess != "")
        {
            textPanel.text += "\n" + player.fullname + ": " + guess;
            inputField.text = "";
        }
        else if (guess == "skip")
        {
            inputField.text = "";
            player.hadDialogue[api.dialogueNumber] = true;
            dialogueStep = 8;
        }
        else
        {
            api.WrongInput(guess);
        }
    }

    void Update()
    {
        if (dialogueStep == 8 && (Input.GetKey("left") || Input.GetKey("right")))
        {
            api.rightPicture.sprite = null;
            inputField.readOnly = true;
            inputField.text = "";
            inputField.DeactivateInputField();
            player.SetMove(true);
            dialogueStep = -1;
            box.enabled = true;
        }
    }

}

