using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public class Player : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    private Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        Vector2 lerp = Vector2.Lerp(mainCamera.transform.position, transform.position, 0.8f);
        mainCamera.transform.position = new Vector3(lerp.x, lerp.y, mainCamera.transform.position.z);
    }
    private float moveCounter = 0;
    private bool canJump = true;
    private bool touchingGround = false;
    private float jumpDegrees = 0;
    [SerializeField] private float verticalAdjustmentMult = 2f;
    private void OnCollisionStay2D(Collision2D collision)
    {
        touchingGround = true;
        if(collision.otherCollider.CompareTag("JumpCollider"))
        {
            if (rb.velocity.y <= 0)
            {
                canJump = true;
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        touchingGround = true;
        if (collision.otherCollider.CompareTag("JumpCollider"))
        {
            if (rb.velocity.y <= 0)
            {
                canJump = true;
            }
        }
    }
    private void FixedUpdate()
    {
        Vector2 velocity = rb.velocity;
        float rotation = rb.rotation;
        if (touchingGround)
            velocity.x *= 0.9f;
        else
            velocity.x *= 0.95f;
        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Space))
        {
            if (canJump && touchingGround)
            {
                canJump = false;
                velocity *= 0.2f;
                velocity += new Vector2(0, 8).RotatedBy(jumpDegrees);
            }
        }
        bool moveLeft = Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D);
        bool moveRight = !Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D);
        if (moveLeft)
        {
            if(moveCounter == 0)
                moveCounter = 0.1f;
        }
        if (Input.GetKey(KeyCode.S))
        {

        }
        if (moveRight)
        {
            if (moveCounter == 0)
                moveCounter = -0.1f;
        }
        if(!touchingGround)
        {
            if(!moveLeft && !moveRight)
            {
                moveCounter = 0;
            }
        }            
        if(moveCounter != 0)
        {
            float dir = Mathf.Sign(moveCounter);
            float absCounter = Mathf.Abs(moveCounter);
            float sin = Mathf.Sin(absCounter / 120f * Mathf.PI);
            if (!touchingGround)
                sin = 1;
            velocity.x = sin * dir * -2.7f;
            if(absCounter < 90)
                rb.freezeRotation = true;
            else
                rb.freezeRotation = false;
            rotation += (0.3f + 4.0f * sin) * dir;
            if (absCounter >= 120 && touchingGround)
            {
                moveCounter = 0;
            }
            else
            {
                moveCounter += (0.3f + 4.0f * sin) * dir;
            }
            if (touchingGround)
                velocity.y -= 0.5f;
        }
        else
        {
            rb.freezeRotation = false;
        }
        if (touchingGround)
            velocity.y -= 1.0f;
        if (rb.velocity.y < 0)
        {
            rb.gravityScale += 0.01f;
        }
        else
        {
            rb.gravityScale = 1;
        }
        //rotation -= summedDirection.x * 15;
        //velocity += summedDirection;

        rb.velocity = velocity;
        rb.rotation = rotation;
        touchingGround = false;
    }
}
