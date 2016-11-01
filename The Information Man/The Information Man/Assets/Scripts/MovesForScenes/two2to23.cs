using UnityEngine;
using System.Collections;

public class two2to23 : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "player")
        {
            Application.LoadLevel("stage23");
        }
    }
}
