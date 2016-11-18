using UnityEngine;
using System;
using UnityEngine.UI;

public class LastDialogue : MonoBehaviour
{
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
            api.SetHints("Greet the professor!\n1. Hi\n2. Hello");
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
            api.ProcessDialogue(guess, "You've came to get the exam results?");
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
                api.ProcessDialogue(guess, "Oh, I have you in my list! You've written it brilliantly! Your grade is 'A'! Congrutulations!");
                api.SetHints("Thank the professor!");
            }
            else
                api.DialogueFail(guess, "I don't have you in my list. Don't waste my time anymore!");
        }
        else if (api.dialogueStep == 3)
        {
            api.DialogueSuccess(guess, "I let you go! Goodbye!");
            api.SetHints("");
        }
        else if (api.dialogueStep == 4 && guess != "")
        {
            textPanel.text += "\n" + player.fullname + ": " + guess;
            inputField.text = "";
        }
        else if (guess == "skip")
        {
            inputField.text = "";
            player.hadDialogue[api.dialogueNumber] = true;
            api.dialogueStep = 4;
        }
        else
        {
            api.WrongInput(guess);
        }
    }

    void Update()
    {
        if (api.dialogueStep == 4 && (Input.GetKey("left") || Input.GetKey("right")))
        {
            api.rightPicture.sprite = Resources.Load<Sprite>("elbrus") as Sprite;
            inputField.readOnly = true;
            inputField.text = "";
            inputField.DeactivateInputField();
            player.SetMove(true);
            api.dialogueStep = -1;
            box.enabled = true;
        }
    }

}

