using UnityEngine;
using System.Collections;

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
        player.GetComponent<Player>().SetMove(false);
        fadeScr.EndScene(SceneNumber);
    }
}
