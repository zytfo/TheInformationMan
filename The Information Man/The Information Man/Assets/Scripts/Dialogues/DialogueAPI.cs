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
    public GameObject timePanel;
    private Text time;

    private int maxLines = 8;

    private string interlocutorName { get; set; }
    public int dialogueNumber { get; set; }
    public int dialogueStep { get; set; }
    private int leftTry;
    private int minutes;
    private int seconds;

    void Start()
    {
        fontSizeGenerate();
        interlocutorName = "";
        dialogueNumber = -1;
        dialogueStep = -1;
        time = timePanel.GetComponentInChildren<Text>();
        if (SceneManager.GetActiveScene().name == "stage1" || SceneManager.GetActiveScene().name == "stage3")
        {
            leftTry = 3;
            PlayerPrefs.SetInt("attempts", leftTry);
            PlayerPrefs.Save();
        } else leftTry = PlayerPrefs.GetInt("attempts");
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
        if (leftTry < 1) hint.interactable = false;
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
        DialogueSuccessActions();
    }

    public void DialogueSuccess(int dialogueStep)
    {
        DialogueSuccessActions();
        this.dialogueStep = dialogueStep;
    }

    private void DialogueSuccessActions()
    {
        player.taskPanel.SetActive(false);
        attemptsLeft.SetActive(false);
        player.hadDialogue[dialogueNumber] = true;
        player.panelText = textPanel.text;
        inputField.text = "";
        dialogueStep++;
        answersObj.SetActive(false);
        SetHints("");
        PlayerPrefs.SetInt("attempts", leftTry);
        PlayerPrefs.Save();
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
        if (player.curHealth > 0) inputField.ActivateInputField();
        else inputField.readOnly = true;
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

    /*
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
    */

    public void fontSizeGenerate()
    {
        if (GameConfig.resolutionWidth == 1024)
        {
            textPanel.fontSize = 8;
            healthPanel.fontSize = 8;
            inputField.textComponent.fontSize = 8;
            counter.fontSize = 8;
            answers.fontSize = 8;
        }
        if (GameConfig.resolutionWidth == 1280)
        {
            textPanel.fontSize = 16;
            healthPanel.fontSize = 16;
            inputField.textComponent.fontSize = 16;
            counter.fontSize = 16;
            answers.fontSize = 16;
        }
        if (GameConfig.resolutionWidth == 1366)
        {
            textPanel.fontSize = 16;
            healthPanel.fontSize = 16;
            inputField.textComponent.fontSize = 16;
            counter.fontSize = 16;
            answers.fontSize = 16;
        }
        if (GameConfig.resolutionWidth == 1600)
        {
            textPanel.fontSize = 20;
            healthPanel.fontSize = 20;
            inputField.textComponent.fontSize = 20;
            counter.fontSize = 20;
            answers.fontSize = 20;
        }
        if (GameConfig.resolutionWidth == 1920)
        {
            textPanel.fontSize = 25;
            healthPanel.fontSize = 25;
            inputField.textComponent.fontSize = 25;
            counter.fontSize = 25;
            answers.fontSize = 25;
        }
    }

    public void hintsButton()
    {
        if (answersObj.activeInHierarchy) { return; }
        else
        {
            leftTry--;
            if (leftTry <= 0)
            {
                leftTry = 0;
                hint.interactable = false;
            }
            counter.text = leftTry.ToString();
            answersObj.SetActive(true);
        }
    }

    public void SetHints(string hints)
    {
        answers.text = hints;
    }

    public void UpdateTime()
    {
        string mins = minutes.ToString();
        string secs = seconds.ToString();
        if (minutes.ToString().Length == 1) mins = "0" + mins;
        if (seconds.ToString().Length == 1) secs = "0" + secs;
        time.text = mins + ":" + secs;
    }

    public void StartTimer(int minutes, int seconds)
    {
        this.minutes = minutes;
        this.seconds = seconds;
        StartCoroutine(Timer());
    }

    public void DecreaseTime()
    {
        if (minutes == 0 && seconds == 0) return;
        if (seconds == 0)
        {
            minutes--;
            seconds = 59;
        }
        else seconds--;
        UpdateTime();
    }

    public IEnumerator Timer()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.0f);
            DecreaseTime();
            if (minutes == 0 && seconds == 0) player.curHealth = 0;
        }
    }
}
