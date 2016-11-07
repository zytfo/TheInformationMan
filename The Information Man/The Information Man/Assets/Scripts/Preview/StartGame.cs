using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StartGame : MonoBehaviour {

    public InputField name;
    public Toggle isMale;
    public InputField biography;
    public Text age;
    public Button yes;
    public Button no;

	// Use this for initialization
	void Start () {
        yes.interactable = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (checkCond()) yes.interactable = true; else yes.interactable = false; 
	}

    bool checkCond()
    {
        if ((name.text.Length > 3) && (isMale.isOn) && (biography.text.Length == 140) && (System.Int32.Parse(age.text) > 15) && (System.Int32.Parse(age.text) <= 45)) return true; else return false;
    } 

    public void StartNewGame()
    {
        Application.LoadLevel("stage1");
    }

    public void NoButton()
    {
        Application.LoadLevel("MainMenu");
    }
}
