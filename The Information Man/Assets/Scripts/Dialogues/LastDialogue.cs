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
            api.ProcessDialogue(guess, "You've come to get the exam results?");
            api.SetHints("Answer positively");
        }
		else if (api.dialogueStep == 1 && (guess.ToLower() == "yes" || guess.ToLower() == "yeah"))
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
        }
        else if (api.dialogueStep == 4 && guess != "")
        {
            textPanel.text += "\n" + player.fullname + ": " + guess;
            inputField.text = "";
        }
        else if (guess == "hjkl")
        {
            api.DialogueSuccess(4);
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

