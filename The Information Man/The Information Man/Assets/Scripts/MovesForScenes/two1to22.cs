using UnityEngine;
using System.Collections;

public class two1to22 : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "player")
        {
            Application.LoadLevel("stage22");
        }
    }
}
