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
    public GameObject attemptsLeft;
    public Button hint;
    public Text counter;
    public Text answers;
    public GameObject answersObj;

    private int maxLines;

    private string interlocutorName { get; set; }
    public int dialogueNumber { get; set; }
    public int dialogueStep { get; set; }
    int leftTry;

    void Start()
    {
        GetMaxLines();
        interlocutorName = "";
        dialogueNumber = -1;
        dialogueStep = -1;
        leftTry = 3;
    }

    void Update()
    {
        Shifter();
    }

    public Tasks.Task task()
    {
        return player.task;
    }

    public void task(Tasks.Task task)
    {
        player.task = task;
    }

    public void DialogueStart(int dialogueNumber, string interlocutorName, string greeting, Sprite rightPicture)
    {
        attemptsLeft.SetActive(true);
        counter.text = leftTry.ToString();
        this.interlocutorName = interlocutorName;
        this.rightPicture.sprite = rightPicture;
        this.dialogueNumber = dialogueNumber;
        dialogueStep = 0;

        textPanel.text = interlocutorName + ": " + greeting;
        inputField.readOnly = false;
        inputField.ActivateInputField();
        player.SetMove(false);
    }

    public void ProcessDialogue(string playerStr, string otherStr)
    {
        string[] lines = textPanel.text.Split('\n');
        if (lines[lines.Length - 1].Contains(player.fullname + ":"))
            EraseLine();
        textPanel.text += "\n" + player.fullname + ": " + playerStr;
        textPanel.text += "\n" + interlocutorName + ": " + otherStr;
        inputField.text = "";
        inputField.ActivateInputField();
        dialogueStep++;
        answersObj.SetActive(false);
    }

    public void DialogueSuccess(string playerStr, string otherStr)
    {
        textPanel.text += "\n" + player.fullname + ": " + playerStr;
        textPanel.text += "\n" + interlocutorName + ": " + otherStr;
        player.taskPanel.SetActive(false);
        attemptsLeft.SetActive(false);
        player.hadDialogue[dialogueNumber] = true;
        player.panelText = textPanel.text;
        inputField.text = "";
        dialogueStep++;
    }

    public void DialogueFail(string playerStr, string otherStr)
    {
        player.curHealth = 0;
        ProcessDialogue(playerStr, otherStr);
        inputField.DeactivateInputField();
    }

    public void AnotherAttempt()
    {
        player.decreaseHealth(20);
        if (textPanel.text.EndsWith("Try again!"))
        {
            textPanel.text = textPanel.text.Remove(textPanel.text.LastIndexOf('\n'));
            textPanel.text = textPanel.text.Remove(textPanel.text.LastIndexOf('\n'));
        }
        dialogueStep--;
    }

    public void WrongInput(string playerStr)
    {
        System.Random rnd = new System.Random();
        player.decreaseHealth(rnd.Next(1, 21));
        string[] lines = textPanel.text.Split('\n');
        if (lines[lines.Length - 1].Contains(player.fullname + ":"))
            EraseLine();
        textPanel.text += "\n" + player.fullname + ": " + playerStr;
        inputField.text = "";
        inputField.ActivateInputField();
    }

    public bool IsGreeting(string input)
    {
        return input == "hi" || input == "Hi" || input == "hello" || input == "Hello"
            || input == "what's up" || input == "What's up";
    }

    public bool IsCool(string input)
    {
        return input == "cool" || input == "Cool" || input == "great" || input == "Great"
            || input == "nice" || input == "Nice";
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

    public void EraseLine()
    {
        textPanel.text = textPanel.text.Remove(textPanel.text.LastIndexOf('\n'));
    }

    private void GetMaxLines()
    {
        int resolution = Screen.currentResolution.width;
        if (resolution == 1920) maxLines = 10;
        else if (resolution == 1600) maxLines = 10;
        else if (resolution == 1366) maxLines = 8;
        else if (resolution == 1280) maxLines = 7;
        else if (resolution == 1024) maxLines = 7;
        else maxLines = 8;
    }

    IEnumerator WaitSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

    public IEnumerator GameOver()
    {
        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadScene("MainMenu");
    }

    public void hintsButton()
    {
        if (answersObj.activeInHierarchy) { return; }
        else
        {
            leftTry--;
            if (leftTry == 0)
            {
                hint.interactable = false;
            }
            counter.text = leftTry.ToString();
            answersObj.SetActive(true);
        }
    }
}
