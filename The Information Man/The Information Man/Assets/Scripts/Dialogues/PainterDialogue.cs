using UnityEngine;
using UnityEngine.UI;

public class PainterDialogue : MonoBehaviour {
    private Player player;
    private Text textPanel;
    private InputField inputField;
    public Sprite rightPicture;

    private DialogueAPI api;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "player" && !player.hadDialogue[1])
        {
            api.DialogueStart(1, "Painter.Net", "Hey! Where are you going?! Did you see an announcement that " 
                + "skyway is closed\n because of unplanned works!", rightPicture);
            api.SetHints("1. Yes\n2. No");
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
        else if (api.dialogueStep == 0 && (guess.Contains("yes") || guess.Contains("no")))
        {
            api.ProcessDialogue(guess, "Great. Why are here then?");
            api.SetHints("Write something containing \"go\"");
        }
        else if (api.dialogueStep == 1 && guess.Contains("go"))
        {
            api.ProcessDialogue(guess, "OK, I will let you go.");
            api.SetHints("Answer anything");
        }
        else if (api.dialogueStep == 2)
        {
            api.ProcessDialogue(guess, "Not right now, haha. Just two questions actually. Firstly, don't you know who is the author\n of this masterpiece on the wall?");
            api.SetHints("Don't lie!");
        }
        else if (api.dialogueStep == 3)
        {
            api.ProcessDialogue(guess, "I will remember your answer. Btw, I've never seen you here before. Are you a freshman?");
            api.SetHints("You're a freshman. Confirm it!");
        }
        else if (api.dialogueStep == 4 && (guess == "yes" || guess == "Yes"))
        {
            api.ProcessDialogue(guess, "Welcome to this wonderful place. I promise you'll have an unforgetable Information Theory\n journey, "
                + "you, the Information Man. I'll give you a simple task that will surely help you to get into the\n swing of things here. Are you excited?");
            api.SetHints("Answer anything");
        }
        else if (api.dialogueStep == 5)
        {
            api.task(new Tasks.PoissonDistributionTask());
            player.UpdateTaskPanel();
            api.ProcessDialogue(guess, "Cool! " + api.task().taskDescription);
            api.SetHints("Use:\n \"Ctrl+T\" to see the task\n \"Ctrl+H\" to see helpful formulas");
        }
        else if (api.dialogueStep == 6)
        {
            if (api.task().CheckResult(guess, api.task().writeAnswer) == 1)
            {
                api.task(null);
                player.UpdateTaskPanel();
                api.DialogueSuccess(guess, "I see, you're a smart guy, the Information Man! I let you go.");
            }
            else
            {
                api.AnotherAttempt();
                api.ProcessDialogue(guess, "You're wrong! Are you really the Information Man then? Try again!");
            }
        }
        else if (api.dialogueStep == 7 && guess != "")
        {
            textPanel.text += "\n" + player.fullname + ": " + guess;
            inputField.text = "";
        }
        else if (guess == "skip")
        {
            api.DialogueSuccess(7);
        }
        else
        {
            api.WrongInput(guess);
        }
    }

    void Update()
    {
        if (api.dialogueStep == 7 && (Input.GetKey("left") || Input.GetKey("right")))
        {
            api.rightPicture.sprite = Resources.Load<Sprite>("elbrus") as Sprite;
            inputField.readOnly = true;
            inputField.text = "";
            inputField.DeactivateInputField();
            player.SetMove(true);
            api.dialogueStep = -1;
        }
    }
}
