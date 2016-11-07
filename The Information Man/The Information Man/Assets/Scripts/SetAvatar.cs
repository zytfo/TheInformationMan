using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SetAvatar : MonoBehaviour {

    public Image avatarLeft;
    public Image avatarRight;
    string correct;
    public int index;
    string[] name = new string[] { "alb", "artur", "dolj", "ilya", "kos" };


	// Update is called once per frame
	void Start () {
        index = PlayerPrefs.GetInt("imageNumber");
        correct = name[index-1];
        avatarLeft.sprite = Resources.Load<Sprite>("Preview/" + correct) as Sprite;    
	}
}
