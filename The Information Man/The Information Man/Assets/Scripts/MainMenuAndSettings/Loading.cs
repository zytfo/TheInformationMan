using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading : MonoBehaviour {
    public Camera loadingCamera;
    public Camera mainMenuCamera;
    public SpriteRenderer img;
    public GameObject Anniversary;
    public AudioSource audio;
    public AudioSource audioShot;
    private Color white = new Color(1,1,1,1);

    // Use this for initialization
    void Start () {
        if (PlayerPrefs.GetInt("loading") == 1)
        {
            StartCoroutine(MainMenu());
        } else
        {
            loadingCamera.enabled = true;
            mainMenuCamera.enabled = false;
            StartCoroutine(LoadLogo());
            StartCoroutine(LoadMainMenu());
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public IEnumerator LoadLogo()
    {
        yield return new WaitForSeconds(1.0f);
        do
        {
            // Start fading towards black.
            img.color = Color.Lerp(img.color, white, 0.5f * Time.deltaTime);
            Debug.Log(img.color);

            // If the screen is almost black...
            if (img.color.a >= 0.95f)
            {
                // ... reload the level
                yield break;
            }
            else
            {
                yield return null;
            }
        } while (true);
    }

    public IEnumerator LoadMainMenu()
    {
        yield return new WaitForSeconds(3.5f);
        img.color = white;
        yield return new WaitForSeconds(2.0f);

        do
        {
            // Start fading towards black.
            img.color = Color.Lerp(img.color, Color.black, 1.0f * Time.deltaTime);
            Debug.Log(img.color);

            // If the screen is almost black...
            if (img.color.r <= 0.05f)
            {
                // ... reload the level
                loadingCamera.enabled = false;
                mainMenuCamera.enabled = true;
                yield return new WaitForSeconds(2.0f);
                audioShot.enabled = true;
                audioShot.Play();
                Anniversary.SetActive(true);
                yield return new WaitForSeconds(0.8f);
                audioShot.enabled = false;
                audio.enabled = true;
                audio.Play();
                yield break;
            }
            else
            {
                yield return null;
            }
        } while (true);

    }

    public IEnumerator MainMenu()
    {
        loadingCamera.enabled = false;
        mainMenuCamera.enabled = true;
        yield return new WaitForSeconds(2.0f);
        audioShot.enabled = true;
        audioShot.Play();
        Anniversary.SetActive(true);
        yield return new WaitForSeconds(0.8f);
        audioShot.enabled = false;
        audio.enabled = true;
        audio.Play();

    }
}
