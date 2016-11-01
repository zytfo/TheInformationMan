using UnityEngine;
using System.Collections;

public class SecondTo21 : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "player")
        {
            Application.LoadLevel("stage21");
        }
    }
}
