using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChangePoints : MonoBehaviour {

    public Text counter;
	// Use this for initialization
	void Start () {
        counter.text = Random.Range(30, 50).ToString();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
