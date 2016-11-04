using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChangeImage : MonoBehaviour {

    public Image image;
    public Text counter;
    string[] name = new string[] { "alb", "artur", "dolj", "ilya", "kos" };
    int i = 0;

	// Use this for initialization
	void Start () {
        image.sprite = Resources.Load<Sprite>("Preview/alb") as Sprite;
        counter.text = (i+1).ToString();
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
