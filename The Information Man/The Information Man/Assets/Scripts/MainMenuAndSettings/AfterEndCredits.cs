using UnityEngine;
using System.Collections;

public class AfterEndCredits : MonoBehaviour {
    private float posY;
    private float posX;
    private float posZ;

    // Use this for initialization
    void Start () {
        posY = this.GetComponent<Transform>().position.y;
        posX = this.GetComponent<Transform>().position.x;
        posZ = this.GetComponent<Transform>().position.z;
    }
	
	// Update is called once per frame
	void Update () {
        this.GetComponent<Transform>().position = new Vector3(posX, posY + 2, posZ);
        this.posY = this.posY + 0.01f;
    }
}
