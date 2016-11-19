using UnityEngine;
using UnityEngine.UI;

public class PreExamDialogue : MonoBehaviour
{
    private Player player;
    private Text textPanel;
    private InputField inputField;
    public Sprite rightPicture;
    public BoxCollider2D box;

    private DialogueAPI api;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "player" && !player.hadDialogue[4])
        {
            api.DialogueStart(4, "Pr. Silitti", "Good day, " + player.fullname + ".. Why you are so late? Did you forget that you have a final exam today?", rightPicture);
            api.SetHints("Apologize to the proffessor!\nSay something containing \"sorry\"");
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
        else if (guess == "skip")
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
