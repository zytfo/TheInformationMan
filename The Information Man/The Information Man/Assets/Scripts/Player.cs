﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float maxSpeed = 3;
    public float speed = 50f;
    //public float jumpPower = 150f;

    public string fullname { get; set; }

    public int curHealth { get; set; }
    public int maxHealth = 100;

	public bool isPaused = false;

    public bool grounded;
    private Animator anim;
    private Rigidbody2D rb2d;

    public bool canMove;
    private bool isDead;

    public GameObject dialoguePanel;
    public GameObject taskPanel { get; set; }
    public Text textPanel { get; set; }
    public Text healthPanel { get; set; }

    private static string _panelText;
    public string panelText
    {
        get { return _panelText; }
        set { _panelText = value; }
    }

    private static string _inputText;
    public string inputText
    {
        get { return _inputText; }
        set { _inputText = value; }
    }

    public Tasks.Task task { get; set; }

    public bool[] hadDialogue { get; set; }

    void Start()
    {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        curHealth = PlayerPrefs.GetInt("health");
        anim = gameObject.GetComponent<Animator>();
        canMove = true;
        isDead = false;
        fullname = PlayerPrefs.GetString("name");

        dialoguePanel = GameObject.Find("DialoguePanel");
        textPanel = GameObject.Find("TextPanel").GetComponent<Text>();
        healthPanel = GameObject.Find("Health").GetComponent<Text>();
        textPanel.text = panelText;
        taskPanel = dialoguePanel.transform.Find("TaskPanel").gameObject;
        task = null;

        hadDialogue = new bool[5];

        switch (SceneManager.GetActiveScene().name)
        {
            case "stage2": transform.eulerAngles = new Vector2(0, 0);
                break;
            case "stage21": transform.eulerAngles = new Vector2(0, 180);
                break;
            case "stage22": transform.eulerAngles = new Vector2(0, 180);
                break;
            case "stage23": transform.eulerAngles = new Vector2(0, 180);
                break;
        }
    }

	public void SetMove(bool move) {
		this.canMove = move;
	}

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown("t"))
        {
            UpdateTaskPanel();
            taskPanel.SetActive(!taskPanel.activeInHierarchy);
        }

        healthPanel.text = curHealth.ToString();

        if (curHealth <= 0)
        {
            if (!isDead)
            {
                isDead = true;
                Die();
            } 
        }

        if (!canMove)
        {
            anim.SetFloat("speed",Mathf.Abs(Input.GetAxis("Vertical")));
            return;
        }
        else
        {
            if (Input.GetKey("left") && Input.GetKey("right"))
            {
                anim.SetFloat("speed", Mathf.Abs(Input.GetAxis("Vertical")));
            }
            else 
            {
                anim.SetFloat("speed", Mathf.Abs(Input.GetAxis("Horizontal")));
            }

            if (Input.GetAxis("Horizontal") < -0.1f)
            {
                transform.eulerAngles = new Vector2(0, 180);
            }
            if (Input.GetAxis("Horizontal") > 0.1f)
            {
                transform.eulerAngles = new Vector2(0, 0);
            }
            /*if (Input.GetButtonDown("Jump") && grounded)
            {
                rb2d.AddForce(Vector2.up * jumpPower);
            }*/

            Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);
            viewPos.x = Mathf.Clamp01(viewPos.x);
            viewPos.y = Mathf.Clamp01(viewPos.y);
            transform.position = Camera.main.ViewportToWorldPoint(viewPos);

            if (curHealth > maxHealth)
            {
                curHealth = maxHealth;
            }
        }
    }

    void FixedUpdate()
    {
        Vector3 easeVelocity = rb2d.velocity;
        easeVelocity.y = rb2d.velocity.y;
        easeVelocity.z = 0.0f;
        easeVelocity.x *= 0.75f;

        if (!canMove)
        {
            rb2d.velocity = easeVelocity;
            return;
        }
        float h = Input.GetAxis("Horizontal");
        
        if (grounded)
        {
            rb2d.velocity = easeVelocity;
        }

        if (Input.GetKey("left") && Input.GetKey("right"))
        {
            stopPlayer();
        }
        else if (Input.GetKey("left"))
        {
            rb2d.AddForce(Vector2.right * speed * h);
        }
        else if (Input.GetKey("right"))
        {
            rb2d.AddForce(Vector2.right * speed * h);
        }
        
        if (rb2d.velocity.x > maxSpeed)
        {
            rb2d.velocity = new Vector2(maxSpeed, rb2d.velocity.y);
        }

        if (rb2d.velocity.x < -maxSpeed)
        {
            rb2d.velocity = new Vector2(-maxSpeed, rb2d.velocity.y);
        }

        if (curHealth > maxHealth)
        {
            curHealth = maxHealth;
        }

        if (curHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        StartCoroutine(GameOver());
    }

    public void UpdateTaskPanel()
    {
        if (task == null)
            taskPanel.GetComponentInChildren<Text>().GetComponent<Text>().text = "No tasks at the moment, you, lucky man!";
        else
            taskPanel.GetComponentInChildren<Text>().text = task.WriteTask() + "\nRight answer is: " + task.writeAnswer;
    }

    void stopPlayer()
    {
        rb2d.AddForce(Vector2.right * 0);
    }

    public void decreaseHealth(int change)
    {
        curHealth = Mathf.Max(0, curHealth - change);
    }

    public IEnumerator GameOver()
    {
        textPanel.text += "\n---We haven't find enough desire to win from you. Basically, it's a GAMEOVER---";
        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene("MainMenu");
    }
}
