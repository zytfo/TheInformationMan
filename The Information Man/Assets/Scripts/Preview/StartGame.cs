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
    public Text scienceSkill;
    public Text trapsSkill;
    public Text pilotingSkill;
    public Text lungsSkill;
    public Text survSkill;

    ScreenFader fadeScr;
    void Awake()
    {
        fadeScr = GameObject.FindObjectOfType<ScreenFader>();
    }
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
        if ((name.text.Length > 3) && (isMale.isOn) && (biography.text.Length >= 140) && (System.Int32.Parse(age.text) > 15) && (System.Int32.Parse(age.text) <= 45)) return true; else return false;
    } 

    public void StartNewGame()
    {
        PlayerPrefs.SetInt("health", 100);
        PlayerPrefs.SetInt("attempts", 3);
        System.Random rnd = new System.Random();
        int[] skills = new int[] {
            System.Convert.ToInt32(scienceSkill.text),
            System.Convert.ToInt32(trapsSkill.text),
            System.Convert.ToInt32(pilotingSkill.text),
            System.Convert.ToInt32(lungsSkill.text)
        };
        int sum = skills[0] + skills[1] + skills[2] + skills[3];
        Debug.Log(sum);
        if (skills[3] < 15)
        {
            PlayerPrefs.SetInt("professor", 2);
        } else if (skills[2] < 15)
        {
            PlayerPrefs.SetInt("professor", 1);
        }
        else if (skills[1] < 15)
        {
            PlayerPrefs.SetInt("professor", 0);
        } else if (sum < 60)
        {
            PlayerPrefs.SetInt("professor", 2);
        }
        else if (sum < 110)
        {
            PlayerPrefs.SetInt("professor", 1);
        }
        else
        {
            PlayerPrefs.SetInt("professor", 0);
        }
        PlayerPrefs.SetInt("survivalRate", System.Convert.ToInt32(survSkill.text));
        PlayerPrefs.Save();
        fadeScr.EndScene(2);
    }

    public void NoButton()
    {
        fadeScr.EndScene(0);
    }
}
