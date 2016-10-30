using UnityEngine;
using System.Collections;

public class NextLevelScript : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "player")
        {
            Application.LoadLevel(1);
        }
    }
}
