using UnityEngine;
using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class PainterDialogue : MonoBehaviour {
    private Player player;
    private Text textPanel;
    private InputField inputField;
    public Sprite rightPicture;

    private DialogueAPI api;

    private int dialogueStep = 0;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "player" && !player.hadDialogue[1])
        {
            api.DialogueStart(1, "Painter.Net", "Hey! Where are you going?! Did you see an announcement that " 
                + "skyway is\n closed because of unplanned works!", rightPicture);
        }
    }

    void Start()
    {
        api = GameObject.Find("DialoguePanel").GetComponent<DialogueAPI>();
        textPanel = api.textPanel;
        player = api.player;
        inputField = api.inputField;
    }

    public void GetInput(string guess)
    {
        if (guess == "") inputField.ActivateInputField();
        else if (dialogueStep == 0 && (guess.Contains("yes") || guess.Contains("no")))
        {
            api.ProcessDialogue(guess, "Great. Why are here then?");
            dialogueStep++;
        }
        else if (dialogueStep == 1 && guess.Length > 4 && guess.Contains("go"))
        {
            api.ProcessDialogue(guess, "OK, I will let you go.");
            dialogueStep++;
        }
        else if (dialogueStep == 2 && api.IsCool(guess))
        {
            api.ProcessDialogue(guess, "Not right now, haha. Just two questions actually. \n" +
                "Firstly, don't you know who is an author of this masterpiece on the wall?");
            dialogueStep++;
        }
        else if (dialogueStep == 3)
        {
            api.ProcessDialogue(guess, "I will remember your answer. Btw, I've never seen you in the University. \nAre you a freshman?");
            dialogueStep++;
        }
        else if (dialogueStep == 4 && (guess == "yes" || guess == "Yes"))
        {
            api.ProcessDialogue(guess, "Welcome to this wonderful place. I promise you'll have an unforgetable\nInformation Theory journey, "
                + "you, the Information Man. I'll give you a simple task that will\nsurely help you to get "
                + "into the swing of things here. Are you excited?");
            dialogueStep++;
        }
        else if (dialogueStep == 5)
        {
            api.task(new Tasks.PoissonDistributionTask());
            player.UpdateTaskPanel();
            api.ProcessDialogue(guess, "Cool! "
                + api.task().taskDescription);
            dialogueStep++;
        }
        else if (dialogueStep == 6)
        {
            if (api.task().CheckResult(guess, api.task().writeAnswer) == 1)
            {
                api.task(null);
                player.UpdateTaskPanel();
                api.DialogueSuccess(guess, "I see, you're a smart guy, the Information Man! I let you go.");
                dialogueStep++;
            }
            else
            {
                api.AnotherAttempt();
                api.ProcessDialogue(guess, "You're wrong! Are you really the Information Man then? Try again!");
            }
        }
        else if (dialogueStep == 7 && guess != "")
        {
            textPanel.text += "\n" + player.fullname + ": " + guess;
            inputField.text = "";
        }
        else if (guess == "skip")
        {
            inputField.text = "";
            player.hadDialogue[api.dialogueNumber] = true;
            dialogueStep = 7;
        }
        else
        {
            api.WrongInput(guess);
        }
    }

    void Update()
    {
        if (dialogueStep == 7 && (Input.GetKey("left") || Input.GetKey("right")))
        {
            api.rightPicture.sprite = null;
            inputField.readOnly = true;
            inputField.text = "";
            inputField.DeactivateInputField();
            player.SetMove(true);
            dialogueStep = -1;
        }
    }
}
