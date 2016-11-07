using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogueAPI : MonoBehaviour {
    public Player player;
    public Image rightPicture;
    public Text textPanel;
    public Text healthPanel;
    public InputField inputField;

    private int maxLines = 8;

    private string interlocutorName { get; set; }
    public int dialogueNumber { get; set; }

    void Start()
    {
        interlocutorName = "";
        dialogueNumber = -1;
    }

    void Update()
    {
        Shifter();
    }

    public void DialogueStart(int dialogueNumber, string interlocutorName, string greeting, Sprite rightPicture)
    {
        this.interlocutorName = interlocutorName;
        this.rightPicture.sprite = rightPicture;
        this.dialogueNumber = dialogueNumber;
        textPanel.text = interlocutorName + ": " + greeting;
        inputField.readOnly = false;
        inputField.ActivateInputField();
        player.SetMove(false);
    }

    public void ProcessDialogue(string playerStr, string otherStr)
    {
        string[] lines = textPanel.text.Split('\n');
        if (lines[lines.Length - 1].Contains(player.fullname + ":"))
            textPanel.text = textPanel.text.Remove(textPanel.text.LastIndexOf('\n'));
        textPanel.text += "\n" + player.fullname + ": " + playerStr;
        textPanel.text += "\n" + interlocutorName + ": " + otherStr;
        inputField.text = "";
        inputField.ActivateInputField();
        if (player.curHealth <= 0) StartCoroutine(GameOver());
    }

    public void DialogueSuccess(string playerStr, string otherStr)
    {
        ProcessDialogue(playerStr, otherStr);
        inputField.DeactivateInputField();
        player.taskPanel.SetActive(false);
        player.hadDialogue[dialogueNumber] = true;
        player.panelText = textPanel.text;
        //rightPicture.sprite = null;
    }

    public void DialogueFail(string playerStr, string otherStr)
    {
        player.curHealth = 0;
        ProcessDialogue(playerStr, otherStr);
        inputField.DeactivateInputField();
    }

    public void AnotherAttempt()
    {
        player.curHealth -= 20;
        if (textPanel.text.EndsWith("Try again!"))
        {
            textPanel.text = textPanel.text.Remove(textPanel.text.LastIndexOf('\n'));
            textPanel.text = textPanel.text.Remove(textPanel.text.LastIndexOf('\n'));
        }
    }
    public void WrongInput(string playerStr)
    {
        System.Random rnd = new System.Random();
        player.curHealth -= rnd.Next(1, 21);
        string[] lines = textPanel.text.Split('\n');
        if (lines[lines.Length - 1].Contains(player.fullname + ":"))
            textPanel.text = textPanel.text.Remove(textPanel.text.LastIndexOf('\n'));
        textPanel.text += "\n" + player.fullname + ": " + playerStr;
        inputField.text = "";
        inputField.ActivateInputField();
    }

    public bool IsGreeting(string input)
    {
        return input == "hi" || input == "Hi" || input == "hello" || input == "Hello"
            || input == "what's up" || input == "What's up";
    }

    public void Shifter()
    {
        if (textPanel.text.Split('\n').Length > maxLines)
        {
            string result = textPanel.text;
            int count = textPanel.text.Split('\n').Length - maxLines;
            for (int i = 0; i < count; ++i)
            {
                bool found = false;
                int j = 0;
                while (!found && j < result.Length)
                {
                    if (result[j] == '\n')
                    {
                        found = true;
                        result = result.Substring(j + 1);
                    }
                    j++;
                }
            }
            textPanel.text = result;
        }
        else return;
    }

    IEnumerator WaitSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

    public IEnumerator GameOver()
    {
        //textPanel.text += "\nMr. Silitti: Basically, it's a GAMEOVER.";
        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadScene("MainMenu");
    }
}
