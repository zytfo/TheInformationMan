using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

public class FirstDialogue : MonoBehaviour {
	public GameObject leftPanel;
	public GameObject rightPanel;
	public GameObject textPanel;
	public GameObject inputField;
	public Sprite leftPicture;
	public Sprite rightPicture;
	public GameObject player;

    private bool paused = false;
    private Task t1;
    private int counter = 0;

    void OnTriggerEnter2D(Collider2D other)
	{
		if (other.name == "player" && !player.GetComponent<Player>().hadDialogue1)
		{
			leftPanel.GetComponent<UnityEngine.UI.Image> ().sprite = leftPicture;
			rightPanel.GetComponent<UnityEngine.UI.Image> ().sprite = rightPicture;
			textPanel.GetComponent<UnityEngine.UI.Text>().text = "Mr. Silitti: HA, ZDAROVA SANYA";
			inputField.GetComponent<UnityEngine.UI.InputField> ().text = "";
			inputField.GetComponent<UnityEngine.UI.InputField> ().readOnly = false;
            player.GetComponent<Player> ().SetMove(false);
            t1 = new ProbabilityTask();
        }
	}

	void Start () {
    }

	public void GetInput(string guess) {
        if (counter == 0 && guess == "Zdarova") {
			counter++;
			textPanel.GetComponent<UnityEngine.UI.Text> ().text += "\nMr. Sanya: Zdarova";
            textPanel.GetComponent<UnityEngine.UI.Text>().text += "\nMr. Silitti: Here's your first task:\n"
                /*+ t1.WriteTask() */ + "Write answer is: " + t1.writeAnswer + ". Any guesses?";
			inputField.GetComponent<UnityEngine.UI.InputField> ().text = "";
            guess = "";
		}
        if (counter == 1 && t1.CheckResult(guess, t1.writeAnswer) == 1)
        {
            textPanel.GetComponent<UnityEngine.UI.Text>().text += "\nMr. Sanya: " + guess;
            textPanel.GetComponent<UnityEngine.UI.Text>().text += "\nMr. Silitti: Basically, you're correct! You can go now.";
            inputField.GetComponent<UnityEngine.UI.InputField>().text = "";
            player.GetComponent<Player>().hadDialogue1 = true;
            counter++;
        } else if (counter == 1 && guess != "" && t1.CheckResult(guess, t1.writeAnswer) < 1)
        {
            textPanel.GetComponent<UnityEngine.UI.Text>().text += "\nMr. Sanya: " + guess;
            textPanel.GetComponent<UnityEngine.UI.Text>().text += "\nMr. Silitti: You're wrong! Try again!";
            inputField.GetComponent<UnityEngine.UI.InputField>().text = "";
        }
	}

    void Update()
    {
        if (counter == 2 && (Input.GetKey("left") || Input.GetKey("right")))
        {
            rightPanel.GetComponent<UnityEngine.UI.Image>().sprite = null;
            inputField.GetComponent<UnityEngine.UI.InputField>().readOnly = true;
            textPanel.GetComponent<UnityEngine.UI.Text>().text = "";
            inputField.GetComponent<UnityEngine.UI.InputField>().text = "";
            player.GetComponent<Player>().SetMove(true);
        }
    }
}

abstract class Task
{
    public double writeAnswer { get; set; }

    public abstract void GenerateValues();
    public abstract string WriteTask();
    public abstract double CalculateResult(); 

    public int CheckResult(string input, double writeAnswer)
    {
        double userResult;
        if (double.TryParse(input, out userResult))
            if (Math.Abs(writeAnswer - userResult) < 0.01)
                return 1;
            else return 0;
        else return -1;
    }
}

class ProbabilityTask : Task
{
    public enum ContentName { balls, fruits };
    public enum TaskType { marginal, joint, conditional };

    public List<Box> boxes { get; set; }
    private ContentName contentName { get; set; }
    private TaskType taskType { get; set; }

    private int boxesCount { get; set; }
    private int itemsCount { get; set; }
    private int boxesIndex { get; set; }
    private int itemsIndex { get; set; }
    private int experimentsCount { get; set; }

    private List<string> colors { get; set; }
    private List<List<string>> itemTypes { get; set; }

    public ProbabilityTask()
    {
        System.Random rnd = new System.Random();
        contentName = (ContentName)rnd.Next(0, 2);
        taskType = (TaskType)rnd.Next(0, 3);
        boxes = new List<Box>();
        FillTypes();
        GenerateValues();
        writeAnswer = Math.Round(CalculateResult(), 2);
    }

    public override void GenerateValues()
    {
        System.Random rnd = new System.Random();
        boxesCount = rnd.Next(2, 5);
        itemsCount = rnd.Next(2, 5);
        experimentsCount = 0;
        for (int i = 0; i < boxesCount; i++)
        {
            List<Item> items = new List<Item>();
            for (int j = 0; j < itemsCount; j++)
            {
                int number = rnd.Next(1, 10);
                experimentsCount += number;
                items.Add(new Item(itemTypes.ElementAt((int)contentName).ElementAt(j), number));
            }
            boxes.Add(new Box(colors.ElementAt(i), items));
        }
    }

    private void FillTypes()
    {
        colors = new List<string>();
        colors.Add("red");
        colors.Add("blue");
        colors.Add("green");
        colors.Add("yellow");

        itemTypes = new List<List<string>>();

        List<string> balls = new List<string>();
        balls.Add("red ball");
        balls.Add("blue ball");
        balls.Add("green ball");
        balls.Add("yellow ball");
        itemTypes.Add(balls);

        List<string> fruits = new List<string>();
        fruits.Add("apple");
        fruits.Add("orange");
        fruits.Add("pineapple");
        fruits.Add("banana");
        itemTypes.Add(fruits);
    }

    public override string WriteTask()
    {
        string result = "Assume you have following boxes containing ";
        result += contentName + "\n";
        foreach (Box b in boxes)
        {
            result += b.color + " box: ";
            foreach (Item i in b.items)
            {
                result += i.characteristic + "s - " + i.count + "; ";
            }
            result += "\n";
        }
        result += "At first you pick the box. Then something inside it. \n";

        System.Random rnd = new System.Random();
        boxesIndex = rnd.Next(1, boxesCount);
        itemsIndex = rnd.Next(1, itemsCount);
        switch ((int)taskType)
        {
            case 0:
                result += "What is the probability of picking " + itemTypes.ElementAt((int)contentName).ElementAt(itemsIndex)
                    + " regardless of the selected box?";
                break;
            case 1:
                result += "What is the probability of selecting " + colors.ElementAt(boxesIndex)
                    + " box and " + itemTypes.ElementAt((int)contentName).ElementAt(itemsIndex) + "?";
                break;
            case 2:
                result += "What is the probability of " + itemTypes.ElementAt((int)contentName).ElementAt(itemsIndex)
                    + " being picked up, given that " + colors.ElementAt(boxesIndex) + " box was selected ?";
                break;
        }
        return result;
    }

    public override double CalculateResult()
    {
        double result = 0;
        switch ((int)taskType)
        {
            case 0:
                int countItemAll = 0;
                for (int i = 0; i < boxesCount; i++)
                    countItemAll += boxes.ElementAt(i).items.ElementAt(itemsIndex).count;
                return countItemAll / (double)experimentsCount;
            case 1:
                return boxes.ElementAt(boxesIndex).items.ElementAt(itemsIndex).count / (double)experimentsCount;
            case 2:
                int countBoxAll = boxes.ElementAt(boxesIndex).itemsCount;
                return boxes.ElementAt(boxesIndex).items.ElementAt(itemsIndex).count / (double)countBoxAll;
        }
        return result;
    }

}

class Box
{
    public string color { get; set; }
    public List<Item> items { get; set; }
    public int itemsCount { get; set; }

    public Box(string color)
    {
        this.color = color;
        items = new List<Item>();
    }

    public Box(string color, List<Item> items)
    {
        this.color = color;
        this.items = items;
        itemsCount = 0;
        foreach (Item i in items) itemsCount += i.count;
    }

    public void AddItem(Item ball)
    {
        items.Add(ball);
    }
}

class Item
{
    public string characteristic { get; set; }
    public int count { get; set; }
    public double probability { get; set; }

    public Item(string characteristic)
    {
        this.characteristic = characteristic;
        count = 0;
    }

    public Item(string characteristic, int count)
    {
        this.characteristic = characteristic;
        this.count = count;
    }

    public Item(string characteristic, int count, double probability)
    {
        this.characteristic = characteristic;
        this.count = count;
        this.probability = probability;
    }
}

