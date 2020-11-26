using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChangeScene : MonoBehaviour {
	public string stageName;

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.name == "player")
		{
            PlayerPrefs.SetInt("health", other.GetComponent<Player>().curHealth);
            other.GetComponent<Player>().panelText = other.GetComponent<Player>().textPanel.GetComponent<Text>().text;
            Application.LoadLevel (this.stageName);
		}
	}
}
