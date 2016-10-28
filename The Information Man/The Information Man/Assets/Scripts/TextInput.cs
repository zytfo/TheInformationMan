using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class TextInput : MonoBehaviour {

    InputField input;
    InputField.SubmitEvent se;
    public Text output;
    public int counter;
    public string newText;
	// Use this for initialization
	void Start () {
        counter = 0;
        input = gameObject.GetComponent<InputField>();
        se = new InputField.SubmitEvent();
        se.AddListener(SubmitInput);
        input.onEndEdit = se;
	}

    private void SubmitInput(string arg0)
    {
        string currentText = output.text.ToString();
        if (arg0 == "") return;
        if (counter == 0) { 
            newText = arg0;
            counter++;
        } else { 
            newText = currentText + "\n" + arg0; 
        }
        output.text = newText;
        input.text = "";
        input.ActivateInputField();
    }
	
}
