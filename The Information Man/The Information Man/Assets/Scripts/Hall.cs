using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Hall : MonoBehaviour
{
    public Button[] door = new Button[3];
    public Image[] flat = new Image[3];
    bool[] open = new bool[3];
    int i, other;
    // Use this for initialization
    void Start()
    {
        flat[0].enabled = false;
        flat[1].enabled = false;
        flat[2].enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void but408()
    {
        // при нажатии якубович открывает рандомную след. дверь и спрашивает, хочет ли игрок сменить дверь
        if (open[0] == false)
        {
            i = Random.Range(1, 3);
            if (i == 1) { i = 1; other = 2; } else { i = 2; other = 1; }
            flat[i].enabled = true;
            flat[i].sprite = Resources.Load<Sprite>("flat") as Sprite;
            door[i].enabled = false;
            open[0] = true;
            open[other] = true;

        }
        else { Application.LoadLevel("stage3"); }
    }

    public void but305()
    {
        // при нажатии якубович открывает рандомную след. дверь и спрашивает, хочет ли игрок сменить дверь
        if (open[1] == false)
        {
            i = Random.Range(0, 2);
            if (i == 0) { i = 0; other = 2; } else { i = 2; other = 0; }
            flat[i].enabled = true;
            flat[i].sprite = Resources.Load<Sprite>("flat") as Sprite;
            door[i].enabled = false;
            open[1] = true;
            open[other] = true;
        }
        else { Application.LoadLevel("stage3"); }
    }

    public void but404()
    {
        // при нажатии якубович открывает рандомную след. дверь и спрашивает, хочет ли игрок сменить дверь
        if (open[2] == false)
        {
            i = Random.Range(0, 2);
            if (i == 0) { i = 0; other = 1; } else { i = 1; other = 0; }
            flat[i].enabled = true;
            flat[i].sprite = Resources.Load<Sprite>("flat") as Sprite;
            door[i].enabled = false;
            open[2] = true;
            open[other] = true;
        }
        else { Application.LoadLevel("stage3"); }
    }
}
