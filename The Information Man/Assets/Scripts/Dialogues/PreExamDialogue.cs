﻿using UnityEngine;
using UnityEngine.UI;

public class PreExamDialogue : MonoBehaviour
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
        if (other.name == "player" && !player.hadDialogue[4])
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
            api.DialogueStart(4, professorName, "Good day, " + player.fullname + ".. Why are you so late? Did you forget that you have a final exam today?", professorImage);
            api.SetHints("Apologize to the professor!\nSay something containing \"sorry\"");
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
        else if (api.dialogueStep == 0 && guess.Contains("sorry"))
        {
            api.DialogueSuccess(guess, "OK, that won't make sense if you don't succeed in the exam. All your classmates are doing last\n tasks now, I think. "
                +"You have only 30 minutes out of 90 left. Good luck, the Information Man!");
        }
        else if (api.dialogueStep == 1 && guess != "")
        {
            textPanel.text += "\n" + player.fullname + ": " + guess;
            inputField.text = "";
        }
        else if (guess == "hjkl")
        {
            api.DialogueSuccess(1);
        }
        else
        {
            api.WrongInput(guess);
        }
    }

    void Update()
    {
        if (api.dialogueStep == 1 && (Input.GetKey("left") || Input.GetKey("right")))
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
