using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChangeAge : MonoBehaviour {
    public Text counter;
    int age = 18;
	// Use this for initialization
	void Start () {
        counter.text = age.ToString();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void NextAgeRight()
    {
        age++;
        if (age == 66) age = 12;
        counter.text = age.ToString();
    }

    public void NextAgeLeft()
    {
        age--;
        if (age == 11) age = 65;
        counter.text = age.ToString();
    }
}
