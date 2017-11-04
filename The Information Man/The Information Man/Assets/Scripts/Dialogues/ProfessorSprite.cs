using UnityEngine;
using System.Collections;

public class ProfessorSprite : MonoBehaviour {


    // Use this for initialization
    void Start () {
        switch (PlayerPrefs.GetInt("professor"))
        {
            case 0:
                GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Professors/silitti");
                break;
            case 1:
                GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Professors/shilov");
                break;
            case 2:
                GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Professors/zouev1");
                break;
            case 3:
                GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Professors/chelovek");
                break;
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
