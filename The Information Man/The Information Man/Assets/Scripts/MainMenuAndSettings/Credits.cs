using UnityEngine;
using System.Collections;

public class Credits : MonoBehaviour {
    public bool ilya = false;
    public bool alex = false;
    public bool artur = false;
    public bool albert = false;

    public GameObject spotLight;

    public void OnMouseEnter()
    {
        spotLight.SetActive(true);
        this.GetComponent<Renderer>().material.color = Color.red;
    }

    public void OnMouseExit()
    {
        spotLight.SetActive(false);
        this.GetComponent<Renderer>().material.color = Color.white;
    }
}
