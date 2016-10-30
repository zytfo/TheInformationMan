using UnityEngine;
using System.Collections;

public class Dialogue : MonoBehaviour {
	public GameObject leftPanel;
	public GameObject rightPanel;
	public GameObject textPanel;
	public GameObject inputField;
	public Sprite leftPicture;
	public Sprite rightPicture;


	void Start () {
		leftPanel.GetComponent<UnityEngine.UI.Image> ().sprite = leftPicture;
		rightPanel.GetComponent<UnityEngine.UI.Image> ().sprite = rightPicture;
	}
}