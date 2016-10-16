using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public float PlayerSpeed;
    public float jumpPower;

    void Update()
    {
        //Amount to move
        // Input.GetAxis MAKES MOOVEMENT LEFT TO RIGHT WITH SMOOTHING
        float amountToMove = Input.GetAxis("Horizontal") * PlayerSpeed * Time.deltaTime;
        //Move the Player
        transform.Translate(Vector3.right * amountToMove);

        //When Players reaches desired (L/R)possition make him stop
        Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);
        viewPos.x = Mathf.Clamp01(viewPos.x);
        viewPos.y = Mathf.Clamp01(viewPos.y);
        transform.position = Camera.main.ViewportToWorldPoint(viewPos);

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (Input.GetKey(KeyCode.UpArrow))
            rb.AddForce(Vector2.up * jumpPower);
    }
}