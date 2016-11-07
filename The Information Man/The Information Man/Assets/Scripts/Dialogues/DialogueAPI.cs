using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogueAPI : MonoBehaviour {
    public Player player;
    public Image rightPicture;
    public Text textPanel;
    public Text livesPanel;
    public InputField inputField;

    private int maxLines = 8;

    private string interlocutorName { get; set; }
    private int dialogueNumber { get; set; }

    void Start()
    {
        interlocutorName = "";
        dialogueNumber = -1;
    }

    public void DialogueStart(int dialogueNumber, string interlocutorName, string greeting, Image rightPicture)
    {
        this.interlocutorName = interlocutorName;
        //this.rightPicture.sprite = rightPicture.sprite;
        textPanel.GetComponent<Text>().text = interlocutorName + ": " + greeting;
        inputField.readOnly = false;
        inputField.ActivateInputField();
        player.SetMove(false);
    }

    public void ProcessDialogue(string playerStr, string otherStr)
    {
        textPanel.text += "\n" + player.fullname + ": " + playerStr;
        textPanel.text += "\nMr. Silitti: " + otherStr;
        inputField.text = "";
        inputField.ActivateInputField();
        if (player.curHealth <= 0) StartCoroutine(GameOver());
    }

    public void DialogueSuccess(string playerStr, string otherStr)
    {
        textPanel.text += "\n" + player.fullname + ": " + playerStr;
        textPanel.text += "\nMr. Silitti: " + otherStr;
        inputField.text = "";
        player.taskPanel.SetActive(false);
        player.hadDialogue[0] = true;
        player.panelText = textPanel.text;
    }

    public void AnotherAttempt()
    {
        //attempts--;
        player.curHealth -= 20;
        //livesPanel.text = (player.curHealth / 20).ToString();
        if (textPanel.text.EndsWith("Try again!"))
        {
            int index = textPanel.text.LastIndexOf('\n');
            textPanel.text = textPanel.text.Remove(index);
            index = textPanel.text.LastIndexOf('\n');
            textPanel.text = textPanel.text.Remove(index);
        }
    }

    public bool IsGreeting(string input)
    {
        return input.Contains("hi") || input.Contains("Hi") || input.Contains("hello") || input.Contains("Hello")
            || input.Contains("what's up") || input.Contains("What's up");
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

    public IEnumerator GameOver()
    {
        textPanel.text += "\nMr. Silitti: Basically, it's a GAMEOVER.";
        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadScene("MainMenu");
    }
}
