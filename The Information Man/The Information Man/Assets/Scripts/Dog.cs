using UnityEngine;
using System.Collections;

public class Dog : MonoBehaviour {

    //public float speed = 50f;
    public Vector2 speed = new Vector2(0.1f, 0);
    public Vector2 direction = new Vector2(-1, 0);
    public Vector2 movement;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        movement = new Vector2(speed.x * direction.x, speed.y * direction.y);
    }

    void FixedUpdate() {
        GetComponent<Rigidbody2D>().velocity = movement;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "ExitFromStageRight" || collision.gameObject.name == "ExitFromStageLeft")
        {
            collision.rigidbody.position = new Vector3(transform.position.x + 0.5f,
                -0.8f, transform.position.z);
        }
        /*if (collision.transform.GetComponent<Rigidbody2D>() == GameObject.Find("player").GetComponent<Rigidbody2D>())
        {
            Instantiate(this.GetComponent<Object>(), new Vector3(-5f, -0.85f, 0f), new Quaternion());
        }*/
    }
}
