using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SetAvatar : MonoBehaviour {

    public Image avatarLeft;
    public Image avatarRight;
    string correct;
    public int index;
	private string[] name = new string[] { 
		"alb", "artur", "dolj", "ilya", "kos", "port", "tim", "girl", "misha", "gag",
		"leha", "borya", "sasha", "bulat", "alb2", "artur2", "ilya2", "kos2"
	};


    // Update is called once per frame
    void Start () {
        index = PlayerPrefs.GetInt("imageNumber");
        correct = name[index-1];
        avatarLeft.sprite = Resources.Load<Sprite>("Preview/" + correct) as Sprite;    
	}
}
