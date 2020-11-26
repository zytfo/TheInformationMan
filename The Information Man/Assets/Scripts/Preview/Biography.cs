using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Biography : MonoBehaviour {

    public Text counterOfLetters;
    public InputField biographyField;

	// Use this for initialization
	void Start () {
        counterOfLetters.text = "0";
	}
	
	// Update is called once per frame
	void Update () {
        counterOfLetters.text = biographyField.text.Length.ToString();
	}
}
