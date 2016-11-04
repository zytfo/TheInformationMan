using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChangeSkills : MonoBehaviour
{
    public Text[] counter = new Text[34];
    public Text counterForPoints;
    int number;
    // Use this for initialization
    void Start()
    {
        //counter.text = age.ToString();
        for (int i = 0; i < 34; i++)
        {
            counter[i].text = Random.Range(1, 50).ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SkillRight(Text counter)
    {
        if ((System.Int32.Parse(counterForPoints.text)) == 0) return;
        else
        {
            number = System.Int32.Parse(counter.text);
            number++;
            if (number == 51) return;
            counter.text = number.ToString();
            counterForPoints.text = (System.Int32.Parse(counterForPoints.text) - 1).ToString();
        }
    }

    public void SkillLeft(Text counter)
    {
        number = System.Int32.Parse(counter.text);
        number--;
        if (number == -1) return;
        counter.text = number.ToString();
        counterForPoints.text = (System.Int32.Parse(counterForPoints.text) + 1).ToString();
    }
}
