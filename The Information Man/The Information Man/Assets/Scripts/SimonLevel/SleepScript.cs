using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SleepScript : MonoBehaviour {
    ScreenFader fadeScr;
    public GameObject player;
    public int SceneNumber;
	// Use this for initialization



    void Awake()
    {
        fadeScr = GameObject.FindObjectOfType<ScreenFader>();
    }
	// Update is called once per frame

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerPrefs.SetInt("health", other.GetComponent<Player>().curHealth);
        other.GetComponent<Player>().panelText = other.GetComponent<Player>().textPanel.GetComponent<Text>().text;
        player.GetComponent<Player>().SetMove(false);
        fadeScr.EndScene(SceneNumber);
    }
}
