using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boulder : MonoBehaviour
{
    private Rigidbody2D rb;
    public bool touchingGround = false;
    private bool touchingIce = false;
    public AudioSource audioSource;
    private void OnCollisionStay2D(Collision2D collision)
    {
        touchingGround = true;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        touchingGround = true;
        audioSource.Play(0);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Ice"))
        {
            touchingIce = true;
        }
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
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
            if(touchingIce)
            {
                velocity.x *= 0.9975f;
            }
            else
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
        rb.rotation -= velocity.x * 0.5f;
        touchingGround = touchingIce = false;
    }
}
