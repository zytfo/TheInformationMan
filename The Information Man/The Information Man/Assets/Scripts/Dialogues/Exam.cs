using UnityEngine;
using System;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class Exam : MonoBehaviour
{
    private Player player;
    private Text textPanel;
    private InputField inputField;
    public Sprite rightPicture;

    private DialogueAPI api;

    void Start()
    {
        api = GameObject.Find("DialoguePanel").GetComponent<DialogueAPI>();
        textPanel = api.textPanel;
        player = api.player;
        inputField = api.inputField;

        player.canMove = false;
    }

    void Update()
    {
        if (player.canMove)
        {
            api.DialogueStart(5, "Sheet of Paper", "Good Day, the Information Man!", rightPicture);
            api.SetHints("Greet the sheet of paper!\n1. Hi\n2. Hello");
        }
    }

    public void GetInput(string guess)
    {
        if (guess == "") inputField.ActivateInputField();
        else if (api.dialogueStep == 0 && api.IsGreeting(guess))
        {
            api.ProcessDialogue(guess + ".. What, are you talking??", "Of course, yes. Paper can communicate with people. I'm a representative of the royal\n sheets family. "
                +"I was made from the best wood in the whole country! So, I think we will get on well.");
            api.SetHints("Answer anything");
        }
        else if (api.dialogueStep == 1)
        {
            api.ProcessDialogue(guess, "Cool! But actually, I'm just a result of the process of imagination in your mind. All my\n words doesn't exist in reality. "
                + "Do you really think that I'm talking to you right now? Yes? No?");
            api.SetHints("Be accurate!");
        }
        else if (api.dialogueStep == 2)
        {
            api.ProcessDialogue("Yes, no.. Don't waste my time, you, a sheet of paper!", "OK, I understand. Exam. Exam is such an important event. "
                + "And you don't want to fail.\n I know. But I advice you to think about the life of paper in your spare time! "
                + "Cause sometimes people\n use us miserably. We don't like such service! Peace!");
        }
        else if (api.dialogueStep == 3)
        {
            api.ProcessDialogue(guess, "OK, let's postpone our important talk for a short time. We'll have enough time after\n the exam. "
                +"Let's unite and together solve these annoying tasks. Hope I'll bring tasks to you correctly.\n Are you ready for the first one?");
            api.SetHints("Answer anything");
        }
        else if (api.dialogueStep == 4)
        {
            System.Random rnd = new System.Random();
            String word = "";
            switch (rnd.Next(0, 3))
            {
                case 0: word = "I love Indormation Theory"; break;
                case 1: word = "Sheet of Paper is my friend"; break;
                case 2: word = "this is a game of the year"; break;
            }
            api.task(new Tasks.EntropyTask("My name is " + player.fullname + " and " + word));
            player.UpdateTaskPanel();
            api.ProcessDialogue("Yes, my companion!", "Task One: Easy one: " + api.task().taskDescription);
            api.SetHints("Use:\n \"Ctrl+T\" to see the task\n \"Ctrl+H\" to see helpful formulas");
        }
        else if (api.dialogueStep == 5)
        {
            if (api.task().CheckResult(guess, api.task().writeAnswer) == 1)
            {
                api.ProcessDialogue(guess, "My paper congratulations to you! Next task consists of five subtasks. Get ready:");
                api.SetHints("Answer anything");
            }
            else
            {
                api.AnotherAttempt();
                api.ProcessDialogue(guess, "My paper disappointment to you. Wrong one! Try again!");
            }
        }
        else if (api.dialogueStep == 6)
        {
            api.task(new Tasks.TwoRandomVariables());
            player.UpdateTaskPanel();
            api.ProcessDialogue(guess, "Task Two: " + api.task().taskDescription);
            api.SetHints("Use:\n \"Ctrl+T\" to see the task\n \"Ctrl+H\" to see helpful formulas");
        }
        else if (api.dialogueStep == 7)
        {
            if (api.task().CheckResult(guess, api.task().writeAnswer) == 1)
            {
                api.ProcessDialogue(guess, "First one is correct! Next:\n " + api.task().WriteSubTask());
                api.task().taskDescription = api.task().WriteTask();
                player.UpdateTaskPanel(); 
            }
            else
            {
                api.AnotherAttempt();
                api.ProcessDialogue(guess, "My paper disappointment to you. Wrong one! Try again!");
            }
        }
        else if (api.dialogueStep == 8)
        {
            if(api.task().CheckResult(guess, api.task().writeAnswer) == 1)
            {
                api.ProcessDialogue(guess, "Second one is correct! Next:\n " + api.task().WriteSubTask());
                api.task().taskDescription = api.task().WriteTask();
                player.UpdateTaskPanel();
            }
            else
            {
                api.AnotherAttempt();
                api.ProcessDialogue(guess, "My paper disappointment to you. Wrong one! Try again!");
            }
        }
        else if (api.dialogueStep == 9)
        {
            if (api.task().CheckResult(guess, api.task().writeAnswer) == 1)
            {
                api.ProcessDialogue(guess, "Third one is correct! Next:\n " + api.task().WriteSubTask());
                api.task().taskDescription = api.task().WriteTask();
                player.UpdateTaskPanel();
            }
            else
            {
                api.AnotherAttempt();
                api.ProcessDialogue(guess, "My paper disappointment to you. Wrong one! Try again!");
            }
        }
        else if (api.dialogueStep == 10)
        {
            if (api.task().CheckResult(guess, api.task().writeAnswer) == 1)
            {
                api.ProcessDialogue(guess, "Forth one is correct! Next:\n " + api.task().WriteSubTask());
                api.task().taskDescription = api.task().WriteTask();
                player.UpdateTaskPanel();
            }
            else
            {
                api.AnotherAttempt();
                api.ProcessDialogue(guess, "My paper disappointment to you. Wrong one! Try again!");
            }
        }
        else if (api.dialogueStep == 11)
        {
            if (api.task().CheckResult(guess, api.task().writeAnswer) == 1)
            {
                api.ProcessDialogue(guess, "Fifth one is correct! Cool! Task was done easily! Are you ready for the last one?");
                api.SetHints("Answer anything");
            }
            else
            {
                api.AnotherAttempt();
                api.ProcessDialogue(guess, "My paper disappointment to you. Wrong one! Try again!");
            }
        }
        else if (api.dialogueStep == 12)
        {
            api.task(new Tasks.HuffmanCodingTask());
            player.UpdateTaskPanel();
            api.ProcessDialogue(guess, "Task Three: Hard & Fun! \n" + api.task().taskDescription);
            api.SetHints("Use:\n \"Ctrl+T\" to see the task\n \"Ctrl+H\" to see helpful formulas");
        }
        else if (api.dialogueStep == 13)
        {
            if (api.task().CheckResult(guess, api.task().writeAnswer) == 1)
            {
                api.DialogueSuccess(guess, "Hooray!! Mate, you've done everything. Now, hurry up! Rewrite solutions carefully and\n "
                    + "pass them to the Professor. And hope for your 'A' grade!");
                api.SetHints("");
                StartCoroutine(EndOfExam());
            }
            else
            {
                api.AnotherAttempt();
                api.ProcessDialogue(guess, "My paper disappointment to you. Wrong one! Try again!");
            }
        }
        else if (api.dialogueStep == 14 && guess != "")
        {
            textPanel.text += "\n" + player.fullname + ": " + guess;
            inputField.text = "";
        }
        else
        {
            api.WrongInput(guess);
        }
    }

    public IEnumerator EndOfExam()
    {
        yield return new WaitForSeconds(7.0f);
        SceneManager.LoadScene("end");
    }

}

