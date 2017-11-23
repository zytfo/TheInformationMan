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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "ExitFromStageRight")
        {
            StartCoroutine(WaitCoroutine());
        }
    }

    IEnumerator WaitCoroutine()
    {
        yield return new WaitForSeconds(3.0f);
        GetComponent<Rigidbody2D>().transform.position = new Vector3(-7f, -0.8f, 0f);
    }
}
