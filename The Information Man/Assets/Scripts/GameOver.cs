using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour {

    public void TryAgain()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
