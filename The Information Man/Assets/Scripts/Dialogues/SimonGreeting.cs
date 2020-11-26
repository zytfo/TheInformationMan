using System;
using UnityEngine;
using UnityEngine.UI;

public class SimonGreeting : MonoBehaviour {
    private Player player;
    private Text textPanel;
    private InputField inputField;
    public Sprite rightPicture;
    public BoxCollider2D box;

    private DialogueAPI api;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "player" && !player.hadDialogue[3])
        {
            api.DialogueStart(3, "Simon Barrel-Roll", "*sighing* Hi! ", rightPicture);
            api.SetHints("Greet the guy!\n1. Hi\n2. Hello");
        }
    }

    void Start()
    {
        api = GameObject.Find("DialoguePanel").GetComponent<DialogueAPI>();
        textPanel = api.textPanel;
        player = api.player;
        inputField = api.inputField;
        box.enabled = false;
    }

    public void GetInput(string guess)
    {
        if (guess == "") inputField.ActivateInputField();
        else if (api.dialogueStep == 0 && api.IsGreeting(guess))
        {
            api.ProcessDialogue(guess, " You are my new neighbor, aren't you?");
            api.SetHints("Answer positively");
        }
		else if (api.dialogueStep == 1 && (guess.ToLower() == "yes" || guess.ToLower() == "yeah"))
        {
            api.ProcessDialogue(guess, "You think yes, but I think no. You can't be my neighbor so quickly.");
            api.SetHints("Answer anything");
        }
        else if (api.dialogueStep == 2)
        {
            api.ProcessDialogue(guess, "Firstly, what is your name?");
            api.SetHints("Be accurate!");
        }
        else if (api.dialogueStep == 3 && (guess == player.fullname
            || string.Equals(guess, "The Information Man", StringComparison.CurrentCultureIgnoreCase)))
        {
            api.ProcessDialogue(guess, "Such an unusual name. You can compete for the prize of being my new neighbor!");
            api.SetHints("Answer anything");
        }
        else if (api.dialogueStep == 4)
        {
            api.ProcessDialogue("And yours?", "I'm Simon Barrel-Roll. Glad we know each others' names, such a helpful information.");
            api.SetHints("Answer anything");
        }
        else if (api.dialogueStep == 5)
        {
            api.ProcessDialogue(guess, "I've heard someones voice? Who is this? Ah, never mind. Well, didn't you know that\n all my previous neighbors had an IQ > 120? You should solve some tasks to "
                + "proudly call me your\n neighbor!");
            api.SetHints("Answer anything");
        }
        else if (api.dialogueStep == 6)
        {
            api.task(new Tasks.EntropyTask());
            player.UpdateTaskPanel();
            api.ProcessDialogue(guess, "Here's the task: \n" + api.task().taskDescription);
            api.SetHints("Use:\n \"Ctrl+T\" to see the task\n \"Ctrl+H\" to see helpful formulas");
        }
        else if (api.dialogueStep == 7)
        {
            if (api.task().CheckResult(guess, api.task().writeAnswer) == 1)
            {
                api.task(null);
                player.UpdateTaskPanel();
                api.ProcessDialogue(guess, "Absobloodylootely! Welcome to this room. Hope you won't leave me soon!");
                api.SetHints("Answer anything");
            }
            else
            {
                api.AnotherAttempt();
                api.ProcessDialogue(guess, "Hahaha, wrong! Try again!");
            }
        }
        else if (api.dialogueStep == 8)
        {
            api.ProcessDialogue(guess, "Here's your bed! I heard you're going to have a hard day tomorrow, so dispose\n yourself with comfort "
                + "and have a nice dream!");
            api.SetHints("Answer anything");
        }
        else if (api.dialogueStep == 9)
        {
            api.DialogueSuccess(guess, "See you soon! Goodnight!");
        }
        else if (api.dialogueStep == 10 && guess != "")
        {
            textPanel.text += "\n" + player.fullname + ": " + guess;
            inputField.text = "";
        }
        else if (guess == "hjkl")
        {
            api.DialogueSuccess(10);
        }
        else
        {
            api.WrongInput(guess);
        }
    }

    void Update()
    {
        if (api.dialogueStep == 10 && (Input.GetKey("left") || Input.GetKey("right")))
        {
            api.rightPicture.sprite = Resources.Load<Sprite>("logo") as Sprite;
            inputField.readOnly = true;
            inputField.text = "";
            inputField.DeactivateInputField();
            player.SetMove(true);
            api.dialogueStep = -1;
            box.enabled = true;
        }
    }
}
