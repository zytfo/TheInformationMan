using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float maxSpeed = 3;
    public float speed = 50f;
    //public float jumpPower = 150f;

    public int curHealth;
    public int maxHealth = 100;
	public bool isPaused = false;

    public bool grounded;
    private Animator anim;
    private Rigidbody2D rb2d;

    public bool canMove;

    private static bool _hadDialogue1;
    public bool hadDialogue1
    {
        get { return _hadDialogue1; }
        set { _hadDialogue1 = value; }
    }

    void Start()
    {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        curHealth = maxHealth;
        anim = gameObject.GetComponent<Animator>();
        canMove = true;
        switch (SceneManager.GetActiveScene().name)
        {
            case "stage2": transform.eulerAngles = new Vector2(0, 180);
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

            if (curHealth <= 0)
            {
                Die();
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
            rb2d.AddForce(Vector2.right * 0);
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
        Application.LoadLevel(Application.loadedLevel);
    }
}
