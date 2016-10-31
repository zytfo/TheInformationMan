using UnityEngine;
using System.Collections;

public class PreviewToFirst : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void StartNewGame()
    {
        Application.LoadLevel("stage1");
    }
}
