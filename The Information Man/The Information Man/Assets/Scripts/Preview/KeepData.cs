using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class KeepData : MonoBehaviour {

    public Text name;
    public Text counter;
	// Use this for initialization

    void FixedUpdate()
    {
        PlayerPrefs.SetString("name", name.text);
        PlayerPrefs.SetInt("imageNumber", System.Int32.Parse(counter.text));
        PlayerPrefs.Save();
    }

	// Update is called once per frame
	void Update () {
	
	}
}
