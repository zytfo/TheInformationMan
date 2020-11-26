using UnityEngine;
using System.Collections;

public class AppQuit : MonoBehaviour {

    void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("loading", 0);
    }
}
