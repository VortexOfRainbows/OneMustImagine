using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boulder : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool touchingGround = false;
    private void OnCollisionStay2D(Collision2D collision)
    {
        touchingGround = true;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        touchingGround = true;
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        Vector2 velocity = rb.velocity;
        if(velocity.y > 0)
        {
            velocity.y *= 0.9825f;
        }
        if(touchingGround)
        {
            velocity.x *= 0.99f;
            rb.gravityScale += 0.05f;
        }
        else
        {
            velocity.x *= 0.9825f;
            rb.gravityScale = 1.0f;
        }
        rb.gravityScale = Mathf.Min(rb.gravityScale, 10);
        rb.velocity = velocity;
        touchingGround = false;
    }
}
