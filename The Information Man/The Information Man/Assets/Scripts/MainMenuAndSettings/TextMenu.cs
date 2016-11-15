using UnityEngine;
using System.Collections;

public class TextMenu : MonoBehaviour {
    public void setMode3D()
    {
        string str = GameConfig.mode3D;
        if (str == "Off")
        {
            this.GetComponent<Transform>().rotation = Quaternion.Euler(0, 0, 0); 
        }
        if (str == "On")
        {
            this.GetComponent<Transform>().rotation = Quaternion.Euler(20, -20, -20);
        }
    }
	
	public void Start () {
        this.setMode3D();
	}
}
