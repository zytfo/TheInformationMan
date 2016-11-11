using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class Tasks {

    public abstract class Task
    {
        public string writeAnswer { get; set; }

        public abstract void GenerateValues();
        public abstract string WriteTask();
        public abstract string CalculateResult();

        public virtual int CheckResult(string input, string writeAnswer)
        {
            double userResult, writeResult;
            double.TryParse(writeAnswer, out writeResult);
            if (double.TryParse(input, out userResult))
                if (Math.Abs(writeResult - userResult) < 0.01)
                    return 1;
                else return 0;
            else return -1;
        }
    }

    public class ProbabilityTask : Task
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
            double tmp;
            double.TryParse(CalculateResult(), out tmp);
            writeAnswer = Math.Round(tmp, 2).ToString();
        }

        public override void GenerateValues()
        {
            System.Random rnd = new System.Random();
            boxesCount = rnd.Next(2, 4);
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
            colors.Add("white");
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
            int spaces = boxesCount - 1;
            foreach (Box b in boxes)
            {
                result += b.color + " box: ";
                for (int i = 0; i < spaces; ++i) result += " ";
                spaces--;
                //if (spaces == 0) result = result.Substring(0, result.Length - 1);
                foreach (Item i in b.items)
                {
                    result += i.characteristic + "s - " + i.count + "; ";
                }
                result += "\n";
            }
            result += "At first you pick the box. Then something inside it.\n";

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
                        + " being picked up, given that " + colors.ElementAt(boxesIndex) + " box was selected?";
                    break;
            }
            return result;
        }

        public override string CalculateResult()
        {
            double result = 0;
            switch ((int)taskType)
            {
                case 0:
                    int countItemAll = 0;
                    for (int i = 0; i < boxesCount; i++)
                        countItemAll += boxes.ElementAt(i).items.ElementAt(itemsIndex).count;
                    result = countItemAll / (double)experimentsCount;
                    break;
                case 1:
                    result = boxes.ElementAt(boxesIndex).items.ElementAt(itemsIndex).count / (double)experimentsCount;
                    break;
                case 2:
                    int countBoxAll = boxes.ElementAt(boxesIndex).itemsCount;
                    result = boxes.ElementAt(boxesIndex).items.ElementAt(itemsIndex).count / (double)countBoxAll;
                    break;
            }
            return result.ToString();
        }

    }

    public class Box
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

    public class Item
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

    public class EntropyTask : Task
    {
        public enum TaskType { seq, str };

        public string input { get; set; }
        public Box box { get; set; }
        private TaskType taskType { get; set; }

        public override void GenerateValues()
        {
            System.Random rnd = new System.Random();
            int itemsCount = rnd.Next(2, 9);
            int[] possibleSums = { 8, 16, 32, 64 };
            int sum = possibleSums[rnd.Next(0, 4)];
            List<Item> items = new List<Item>();
            List<int> itemsNumbers = new List<int>();
            for (int i = 0; i < itemsCount; i++)
            {
                int tmp = sum + i - itemsCount + 2;
                int number = rnd.Next(1, tmp);
                sum = Math.Max(1, sum - number);
                itemsNumbers.Add(number);
            }
            itemsNumbers.Sort();
            foreach (int iN in itemsNumbers) items.Add(new Item("", iN));
            box = new Box("", items);
        }

        public EntropyTask(string input)
        {
            taskType = (TaskType)1;
            this.input = input;
            double tmp;
            double.TryParse(CalculateResult(), out tmp);
            writeAnswer = Math.Round(tmp, 2).ToString();
        }

        public EntropyTask()
        {
            taskType = 0;
            GenerateValues();
            double tmp;
            double.TryParse(CalculateResult(), out tmp);
            writeAnswer = Math.Round(tmp, 2).ToString();
        }

        public double Log2(double n)
        {
            return (Math.Log(n) / Math.Log(2));
        }

        public int GCD(int a, int b)
        {
            return b == 0 ? a : GCD(b, a % b);
        }

        public override string WriteTask()
        {
            string result = "";
            if (taskType == 0)
            {
                int gcd;
                result = "Assume you have a distribution with following probabilities: ";
                for (int i = 0; i < box.items.Count - 1; ++i)
                {
                    gcd = GCD(box.items.ElementAt(i).count, box.itemsCount);
                    result += box.items.ElementAt(i).count / gcd + "/" + box.itemsCount / gcd;
                    result += "; ";
                }
                gcd = GCD(box.items.ElementAt(box.items.Count - 1).count, box.itemsCount);
                result += box.items.ElementAt(box.items.Count - 1).count / gcd + "/" + box.itemsCount / gcd;
                result += ".\nCalcucalute an entropy of the distribution";
            }
            else result = "Calculate an entropy of this sentence:\n" + input;
            return result;
        }

        public override string CalculateResult()
        {
            double result = 0;
            if (taskType == 0)
            {
                double probability;
                foreach (Item i in box.items)
                {
                    probability = i.count / (double)box.itemsCount;
                    result -= probability * Log2(probability);
                }
            }
            else
            {
                List<Symbol> symbols = new List<Symbol>();
                bool found;
                int currentSize;

                symbols.Add(new Symbol(input.ElementAt(0)));
                for (int i = 1; i < input.Count(); ++i)
                {
                    currentSize = symbols.Count();
                    found = false;
                    int j = 0;
                    while (!found && i < currentSize)
                    {
                        if (input.ElementAt(i) == symbols.ElementAt(j).getSymbol())
                        {
                            symbols.ElementAt(j).increaseOccurrences();
                            found = true;
                        }
                        ++i;
                    }
                    if (!found) symbols.Add(new Symbol(input.ElementAt(i)));
                }

                double strLength = input.Length;

                foreach (Symbol s in symbols)
                {
                    double probability = s.getOccurrences() / strLength;
                    result -= probability * Log2(probability);
                }
            }
            return result.ToString();
        }
    }

    class Symbol
    {
        private char symbol;
        private int occurrences;

        public Symbol(char symbol)
        {
            this.symbol = symbol;
            occurrences = 1;
        }

        public char getSymbol() { return symbol; }
        public int getOccurrences() { return occurrences; }

        public void setSymbol(char symbol) { this.symbol = symbol; }

        public void increaseOccurrences() { occurrences++; }
    }

    public class PoissonDistributionTask : Task
    {
        private int mean { get; set; }
        private int m { get; set; }

        public PoissonDistributionTask()
        {
            GenerateValues();
            double tmp;
            double.TryParse(CalculateResult(), out tmp);
            writeAnswer = Math.Round(tmp, 2).ToString();
        }

        private long Factorial(long x)
        {
            return (x == 0) ? 1 : x * Factorial(x - 1);
        }

        public override void GenerateValues()
        {
            System.Random rnd = new System.Random();
            mean = rnd.Next(1, 7);
            m = rnd.Next(1, 11);
        }

        public override string WriteTask()
        {
            string result = "I have counted that the number of students going through the skyway\n per minute is equal to " + mean
                 + ". Find the probability that " + m + " students will go near me next\n minute.";
            return result;
        }

        public override string CalculateResult()
        {
            return (Math.Pow(Math.E, -mean) * Math.Pow(mean, m) / (double)Factorial(m)).ToString();
        }
    }

    public class HuffmanCodingTask : Task
    {
        public string input { get; set; }
        public List<Item> ensemble { get; set; }
        public List<string> codes { get; set; }
        private int itemsCount;

        public override void GenerateValues()
        {
            System.Random rnd = new System.Random();
            itemsCount = rnd.Next(4, 9);
            int sum = rnd.Next(0, 57) + 8;
            int tmp = sum;
            double probability;
            string[] alphabet = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j" };
            ensemble = new List<Item>();
            for (int i = 0; i < itemsCount - 1; i++)
            {
                int number = rnd.Next(1, Math.Max(sum / 2, 2));
                sum = Math.Max(1, sum - number);
                probability = number / (double)tmp;
                ensemble.Add(new Item(alphabet[i], number, Math.Round(probability, 2)));
            }
            probability = sum / (double)tmp;
            ensemble.Add(new Item(alphabet[itemsCount - 1], sum, Math.Round(probability, 2)));
        }

        public HuffmanCodingTask()
        {
            GenerateValues();
            writeAnswer = CalculateResult();
        }

        public double Log2(double n)
        {
            return (Math.Log(n) / Math.Log(2));
        }

        public override string WriteTask()
        {
            string result = "";
            result = "Assume you have symbols with following probabilities:\n";
            for (int i = 0; i < itemsCount; ++i)
            {
                result += "  " + ensemble.ElementAt(i).characteristic + "  ";
            }
            result += "\n";
            for (int i = 0; i < itemsCount; ++i)
            {
                result += ensemble.ElementAt(i).probability.ToString("N2") + " ";
            }
            result += "\nCreate a coding using Huffman algorithm.";
            return result;
        }

        public override string CalculateResult()
        {
            string result = "";

            List<string> codes = Huffman(ensemble);

            for (int i = 0; i < codes.Count - 1; ++i)
                result += codes.ElementAt(i) + " ";
            result += codes.ElementAt(codes.Count - 1);

            return result;
        }

        public List<string> Huffman(List<Item> items)
        {
            if (items.Count == 2) return new List<string>() { "0", "1" };

            List<string> codes = new List<string>();
            Item min1 = items.ElementAt(0);
            int min1Index = 0;
            for (int i = 1; i < items.Count; ++i)
                if (items.ElementAt(i).count < min1.count)
                {
                    min1 = items.ElementAt(i);
                    min1Index = i;
                }
            items.RemoveAt(min1Index);
            Item min2 = items.ElementAt(0);
            int min2Index = 0;
            for (int i = 1; i < items.Count; ++i)
                if (items.ElementAt(i).count < min2.count)
                {
                    min2 = items.ElementAt(i);
                    min2Index = i;
                }
            items.RemoveAt(min2Index);
            items.Add(new Item(min1.characteristic + min2.characteristic, min1.count + min2.count,
                min1.probability + min2.probability));

            codes = Huffman(items);

            codes.Insert(min2Index, codes.ElementAt(codes.Count - 1) + "1");
            codes.Insert(min1Index, codes.ElementAt(codes.Count - 1) + "0");
            codes.RemoveAt(codes.Count - 1);

            return codes;
        }

        public override int CheckResult(string input, string writeAnswer)
        {
            if (input.Equals(writeAnswer))
                return 1;
            else
                return 0;
        }
    }

    public class SumTask : Task
    {
        private int a { get; set; }
        private int b { get; set; }

        public SumTask()
        {
            GenerateValues();
            writeAnswer = CalculateResult();
        }

        public override void GenerateValues()
        {
            System.Random rnd = new System.Random();
            a = rnd.Next(1, 11);
            b = rnd.Next(1, 11);
        }

        public override string WriteTask()
        {
            string result = "What is " + a + " + " + b + "?";
            return result;
        }

        public override string CalculateResult()
        {
            return (a + b).ToString();
        }
    }
}
