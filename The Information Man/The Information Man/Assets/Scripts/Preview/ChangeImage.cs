using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChangeImage : MonoBehaviour {

    public Image image;
    public Text counter;
    private string[] name = new string[] { "alb", "artur", "dolj", "ilya", "kos", "port", "tim", "girl", "misha", "gag"  };
    public int i = 0;

	// Use this for initialization
	void Start () {
        image.sprite = Resources.Load<Sprite>("Preview/alb") as Sprite;
        counter.text = (i+1).ToString();

        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            PlayerPrefs.SetInt("loading", 1);
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void NextImageRight()
    {
        if (i == name.Length - 1)  i = 0;  else i++;
        image.sprite = Resources.Load<Sprite>("Preview/" + name[i]) as Sprite;
        counter.text = (i+1).ToString();
    }

    public void NextImageLeft()
    {
        if (i == 0) i = name.Length - 1; else i--;
        image.sprite = Resources.Load<Sprite>("Preview/" + name[i]) as Sprite;
        counter.text = (i+1).ToString();
    }
}
