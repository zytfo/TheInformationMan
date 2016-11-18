using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Hall : MonoBehaviour
{
    public Button[] door = new Button[3];
    public Image[] flat = new Image[3];
    bool[] open = new bool[3];
    int i, other;
    string[] names = new string[3] { "408", "305", "404" };

    private Player player;
    private Text textPanel;
    private InputField inputField;
    public Sprite rightPicture;
    private DialogueAPI api;

    // Use this for initialization
    void Start()
    {
        flat[0].enabled = false;
        flat[1].enabled = false;
        flat[2].enabled = false;

        api = GameObject.Find("DialoguePanel").GetComponent<DialogueAPI>();
        textPanel = api.textPanel;
        player = api.player;
        inputField = api.inputField;

        Cursor.visible = false;
        player.canMove = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.canMove)
        {
            api.DialogueStart(2, "Yakubovich", "Pam pam pam! Welcome to the famous game show!", rightPicture);
            api.attemptsLeft.SetActive(false);
        }
        if (api.dialogueStep == 2 || api.dialogueStep == 12) Cursor.visible = true;
        else Cursor.visible = false;
    }

    public void but408()
    {
        // при нажатии якубович открывает рандомную след. дверь и спрашивает, хочет ли игрок сменить дверь
        if (open[0] == false)
        {
            i = Random.Range(1, 3);
            if (i == 1) { i = 1; other = 2; } else { i = 2; other = 1; }
            flat[i].enabled = true;
            flat[i].sprite = Resources.Load<Sprite>("flat") as Sprite;
            door[i].enabled = false;
            open[0] = true;
            open[other] = true;
            api.ProcessDialogue("408", names[i] + " is not an elite room");
            Cursor.visible = false;
            inputField.ActivateInputField();
        }
        else
        {
            api.ProcessDialogue("408", "Good luck, the Information Man, good luck..");
            StartCoroutine(LevelLoad());
        }
    }

    public void but305()
    {
        // при нажатии якубович открывает рандомную след. дверь и спрашивает, хочет ли игрок сменить дверь
        if (open[1] == false)
        {
            i = Random.Range(0, 2);
            if (i == 0) { i = 0; other = 2; } else { i = 2; other = 0; }
            flat[i].enabled = true;
            flat[i].sprite = Resources.Load<Sprite>("flat") as Sprite;
            door[i].enabled = false;
            open[1] = true;
            open[other] = true;
            api.ProcessDialogue("305", names[i] + " is not an elite room");
            Cursor.visible = false;
            inputField.ActivateInputField();
        }
        else
        {
            api.ProcessDialogue("305", "Good luck, the Information Man, good luck..");
            StartCoroutine(LevelLoad());
        }
    }

    public void but404()
    {
        // при нажатии якубович открывает рандомную след. дверь и спрашивает, хочет ли игрок сменить дверь
        if (open[2] == false)
        {
            i = Random.Range(0, 2);
            if (i == 0) { i = 0; other = 1; } else { i = 1; other = 0; }
            flat[i].enabled = true;
            flat[i].sprite = Resources.Load<Sprite>("flat") as Sprite;
            door[i].enabled = false;
            open[2] = true;
            open[other] = true;
            api.ProcessDialogue("404", names[i] + " is not an elite room");
            Cursor.visible = false;
            inputField.ActivateInputField();
        }
        else
        {
            api.ProcessDialogue("404", "Good luck, the Information Man, good luck..");
            StartCoroutine(LevelLoad());
        }
    }

    public void GetInput(string guess)
    {
        if (guess == "" && api.dialogueStep != 4 && api.dialogueStep != 9) inputField.ActivateInputField();
        else if (api.dialogueStep == 0)
        {
            api.ProcessDialogue("Wait, what?", "Yes. Dormitory manager is away today. She has left for you 3 available rooms. "
            + "There is\n one elite room and two also good but not elite. You should pick one room. Then I will open one not elite\n room "
            + "and your task is to pick a room again. You can either change your first choice or pick the same\n room. It will be your room then :) "
            + "Are you ready?");
        }
        else if (api.dialogueStep == 1)
        {
            api.ProcessDialogue(guess, "Spin the barrel!.. I mean, pick the room!");
            Cursor.visible = true;
            inputField.DeactivateInputField();
        }
        else if (api.dialogueStep == 3)
        {
            api.ProcessDialogue(guess, "Spin the barrel!");
        }
        else if (api.dialogueStep == 4)
        {
            api.ProcessDialogue("...spinning the barrel...", "Can you tell where did you came from?");
        }
        else if (api.dialogueStep == 5 && guess.Length > 4)
        {
            api.ProcessDialogue(guess, "Interesting! Who do you want to send you greetings to?");
        }
        else if (api.dialogueStep == 6 && guess.Split().Length > 2)
        {
            api.ProcessDialogue(guess, "Have you got some gifts with you?");
        }
        else if (api.dialogueStep == 7)
        {
            api.ProcessDialogue(guess, "Cool. 300 points on the barrel. Letter!");
        }
        else if (api.dialogueStep == 8 && guess.Length == 1)
        {
            api.ProcessDialogue(guess, "No such letter. Basically, it's a \"Field of Dreams\" gameover. "
                + "But I give you the last chance.\n Spin the barrel-roll!");
        }
        else if (api.dialogueStep == 9)
        {
            api.ProcessDialogue("...spinning the barrel...", "Would you love to change your first choice of room?");
        }
        else if (api.dialogueStep == 10 && (guess == "yes" || guess == "Yes"))
        {
            api.task(new Tasks.DefinedTask("What is the probability that by switching in this game player wins?", "0.67"));
            player.UpdateTaskPanel();
            api.ProcessDialogue(guess, "Hhhm.. You think that switching is better? Can you give the actual probability that you will\n win by switching?");
        }
        else if (api.dialogueStep == 10 && (guess == "no" || guess == "No"))
        {
            api.task(new Tasks.DefinedTask("What is the probability that by selecting the same door in this game player wins?", "0.33"));
            player.UpdateTaskPanel();
            api.ProcessDialogue(guess, "Hhhm.. You think that switching is a bad idea? Can you give the actual probability that you\n will win by selecting the same door?");
        }
        else if (api.dialogueStep == 11)
        {
            if (api.task().CheckResult(guess, api.task().writeAnswer) == 1)
            {
                api.task(null);
                player.UpdateTaskPanel();
                api.DialogueSuccess(guess, "Exactly! OK, a matter of life and death! It's time, my friend! Pick the room!");
                Cursor.visible = true;
            }
            else
            {
                api.AnotherAttempt();
                api.ProcessDialogue(guess, "You're wrong! You thought that it's only a show? It's a life, my friend, life itself. Try again!");
            }
        }
        else
        {
            api.WrongInput(guess);
        }
    }

    public IEnumerator LevelLoad()
    {
        yield return new WaitForSeconds(3.0f);
        PlayerPrefs.SetInt("health", player.curHealth);
        SceneManager.LoadScene("stage3");
    }
}
