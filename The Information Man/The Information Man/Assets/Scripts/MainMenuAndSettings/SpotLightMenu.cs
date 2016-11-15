using UnityEngine;
using System.Collections;

public class SpotLightMenu : MonoBehaviour {
    public void setLighting()
    {
        string str = GameConfig.lighting;
        if (str == "High")
        {
            this.GetComponent<Light>().intensity = 2;
        }
        if (str == "Ultra")
        {
            this.GetComponent<Light>().intensity = 3;
        }
        if (str == "Low")
        {
            this.GetComponent<Light>().intensity = 0.11f;
        }
        if (str == "Average")
        {
            this.GetComponent<Light>().intensity = 1;
        }
    }

    public void setBlood()
    {
        string str = GameConfig.blood;
        if (str == "On")
        {
            this.GetComponent<Light>().color = Color.red;
        }
        if (str == "Off")
        {
            this.GetComponent<Light>().color = Color.white;
        }
    }

    public void Start () {
        setLighting();
        setBlood();
	}
}
