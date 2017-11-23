using UnityEngine;
using System;
using UnityEngine.UI;

public class FirstDialogue : MonoBehaviour {
    private Player player;
    private Text textPanel;
    private InputField inputField;
    public BoxCollider2D box;

    private DialogueAPI api;
    private string professorName;
    private Sprite professorImage;

    void OnTriggerEnter2D(Collider2D other)
	{
		if (other.name == "player" && !player.hadDialogue[0])
		{
            switch (PlayerPrefs.GetInt("professor"))
            {
                case 0:
                    professorName = "Pr. Silitti";
                    professorImage = Resources.Load<Sprite>("Professors/silitti_face");
                    break;
                case 1:
                    professorName = "Pr. Shilov";
                    professorImage = Resources.Load<Sprite>("Professors/shilov_face");
                    break;
                case 2:
                    professorName = "Pr. Zouev";
                    professorImage = Resources.Load<Sprite>("Professors/zouev_face");
                    break;
            }
            api.DialogueStart(0, professorName, "Good Morning!", professorImage);
            api.SetHints("Greet the professor!\n1. Hi\n2. Hello");
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
            api.ProcessDialogue(guess, "You've come for an interview?");
            api.SetHints("Answer positively");
        }
        else if (api.dialogueStep == 1 && (guess == "yes" || guess == "Yes"))
        {
            api.ProcessDialogue(guess, "Cool! Can you tell me your name?");
            api.SetHints("Be accurate!");
        }
        else if (api.dialogueStep == 2)
        {
            if (guess == player.fullname || string.Equals(guess, "The Information Man", StringComparison.CurrentCultureIgnoreCase))
            {
                api.ProcessDialogue(guess, "Oh, I have you in my list! OK, I will give you few tasks to check your skills.\n " + "Are you ready for the first one?");
                api.SetHints("Answer anything");
            }
            else
                api.DialogueFail(guess, "I don't have you in my list. Don't waste my time anymore!");
        }
        else if (api.dialogueStep == 3)
        {
            api.task(new Tasks.SumTask());
            player.UpdateTaskPanel();
            switch (PlayerPrefs.GetInt("professor"))
            {
                case 0:
                    api.ProcessDialogue(guess, "Your answer doesn't matter, actually. Never mind. " + api.task().taskDescription);
                    break;
                case 1:
                    api.ProcessDialogue(guess, "Basically, your answer doesn't matter. Nevertheless, " + api.task().taskDescription);
                    break;
                case 2:
                    api.ProcessDialogue(guess, "Your compiler doesn't matter actually. Well. " + api.task().taskDescription);
                    break;
            }
            api.SetHints("Use:\n \"Ctrl+T\" to see the task\n \"Ctrl+H\" to see helpful formulas");
        }
        else if (api.dialogueStep == 4)
        {
            if (api.task().CheckResult(guess, api.task().writeAnswer) == 1)
            {
                api.task(null);
                player.UpdateTaskPanel();
                api.ProcessDialogue(guess, "Surprisingly, correct! OK. Next task.");
                api.SetHints("Answer anything");
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
            api.ProcessDialogue(guess, "I see your happy face. That's cool! Next one may require more time to succeed.\n" 
                + api.task().taskDescription);
            api.SetHints("Use:\n \"Ctrl+T\" to see the task\n \"Ctrl+H\" to see helpful formulas");
        }
        else if (api.dialogueStep == 6)
        {
            if (api.task().CheckResult(guess, api.task().writeAnswer) == 1)
            {
                api.task(null);
                player.UpdateTaskPanel();
                switch (PlayerPrefs.GetInt("professor"))
                {
                    case 0:
                        api.ProcessDialogue(guess, "Basically, you're correct! And the last one: why do you want to study in Innopolis University?");
                        api.SetHints("Write something in\n>20 symbols and at least 5 words");
                        break;
                    case 1:
                        api.ProcessDialogue(guess, "Well done! And the last one: what are the roots of the equation ax^2+bx+c?");
                        api.SetHints("Think carefully");
                        break;
                    case 2:
                        api.ProcessDialogue(guess, "You're right.. ahem! And the last one: name three best programming languages.");
                        api.SetHints("You your knowledge of Compilers course. Oh, wait...");
                        break;
                }
            }
            else
            {
                api.AnotherAttempt();
                api.ProcessDialogue(guess, "You're wrong! Try again!");
            }
        }
        else if (api.dialogueStep == 7 && PlayerPrefs.GetInt("professor") == 0 && guess.Length > 20 && guess.Split().Length > 4)
        {
            api.DialogueSuccess(guess, "Very interesting. I think it's enough for you. Welcome to this wonderful place!\n " 
                + "Dormitory manager is waiting for you. You are free to go.");
        }
        else if (api.dialogueStep == 7 && PlayerPrefs.GetInt("professor") == 1 && (guess.ToLower().StartsWith("x ") || guess.ToLower().StartsWith("x")
            || guess.ToLower().Contains(" x ") || (guess.ToLower().Contains("x1") && guess.ToLower().Contains("x2"))))
        {
            api.DialogueSuccess(guess, "Nice one! I think it's enough for you. Welcome to this wonderful place!\n "
                + "Dormitory manager is waiting for you. You are free to go.");
        }
        else if (api.dialogueStep == 7 && PlayerPrefs.GetInt("professor") == 2 && guess.Split().Length == 3 &&
            (guess.ToLower().Contains("go") || guess.ToLower().Contains("scala") || guess.ToLower().Contains("c++")))
        {
            api.DialogueSuccess(guess, "Good opinion. I think it's enough for you. Welcome to this wonderful place!\n "
                + "Dormitory manager is waiting for you. You are free to go.");
        }
        else if (api.dialogueStep == 8 && guess != "")
        {
            textPanel.text += "\n" + player.fullname + ": " + guess;
            inputField.text = "";
        }
        else if (guess == "hjkl")
        {
            api.DialogueSuccess(8);
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

