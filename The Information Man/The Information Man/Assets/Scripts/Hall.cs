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

    private bool paused = false;
    private int dialogueStep = 0;

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
        if (player.canMove) api.DialogueStart(2, "Yakubovich", "Pam pam pam! Welcome to the famous game show!", rightPicture);
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
            api.ProcessDialogue("404", "Good luck, the Information Man, good luck..");
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
            api.ProcessDialogue("404", "Good luck, the Information Man, good luck..");
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
        if (guess == "" && dialogueStep != 1 && dialogueStep != 3 && dialogueStep != 8) inputField.ActivateInputField();
        else if (dialogueStep == 0)
        {
            api.ProcessDialogue("Wait, what?", "Yes. Dormitory manager is away today. She has left for you 3 available\n rooms."
            + "There is one elite room and two also good but not elite. You should pick one\n room. Then I will open one not elite room "
            + "and your task is to pick a room again. You\n can either change your first choice or pick the same room. It will be your room then :)"
            + "\n Are you ready?");
            dialogueStep++;
        }
        else if (dialogueStep == 1)
        {
            api.ProcessDialogue(guess, "Spin the barrel!.. I mean, pick the room!");
            Cursor.visible = true;
            inputField.DeactivateInputField();
            dialogueStep++;
        }
        else if (dialogueStep == 2 && api.IsCool(guess))
        {
            api.ProcessDialogue(guess, "Spin the barrel!");
            dialogueStep++;
        }
        else if (dialogueStep == 3)
        {
            api.ProcessDialogue("...spinning the barrel...", "Can you tell where did you came from?");
            dialogueStep++;
        }
        else if (dialogueStep == 4 && guess.Length > 10)
        {
            api.ProcessDialogue(guess, "Interesting! Who do you want to send you greetings to?");
            dialogueStep++;
        }
        else if (dialogueStep == 5 && guess.Length > 10 && guess.Split().Length > 2)
        {
            api.ProcessDialogue(guess, "Have you got some gifts with you?");
            dialogueStep++;
        }
        else if (dialogueStep == 6)
        {
            api.ProcessDialogue(guess, "Cool. 300 points on the barrel. Letter!");
            dialogueStep++;
        }
        else if (dialogueStep == 7 && guess.Length == 1)
        {
            api.ProcessDialogue(guess, "No such letter. Basically, it's a \"Field of Dreams\" gameover.\n "
                + "But I give you the last chance. Spin the barrel-roll!");
            dialogueStep++;
        }
        else if (dialogueStep == 8)
        {
            api.ProcessDialogue("...spinning the barrel...", "Would you love to change your first choice of room?");
            dialogueStep++;
        }
        else if (dialogueStep == 9)
        {
            api.ProcessDialogue(guess, "A matter of life and death! Pick the room!");
            Cursor.visible = true;
            dialogueStep++;
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
