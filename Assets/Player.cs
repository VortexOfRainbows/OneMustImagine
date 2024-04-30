using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject boulder;
    [SerializeField] private GameObject arrow;
    [SerializeField] private GameObject jumpCollider;
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
    private bool InTheAir => !touchingGround && !touchingWall;
    private float moveCounter = 0;
    //private bool JustJumped = false;
    private bool canJump = true;
    private bool touchingGround = false;
    private bool touchingWall = false;
    private float jumpDegrees = 0;
    private float justLaunchedBoulder = 0;
    private void OnCollisionStay2D(Collision2D collision)
    {
        if(!touchingGround)
            touchingWall = true;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!touchingGround)
            touchingWall = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!touchingGround)
            touchingWall = true;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!touchingGround)
            touchingWall = true;
    }
    public void RefreshJump()
    {
        if (rb.velocity.y <= 0)
        {
            canJump = true;
            rb.angularVelocity *= 0.0f;
        }
        touchingGround = true;
    }
    private bool lastMouseState;
    private void FixedUpdate()
    {
        Vector2 velocity = rb.velocity;
        float rotation = rb.rotation;
        if (touchingGround)
        {
            velocity.x *= 0.9f;
        }
        else
        {
            velocity.x *= 0.95f;
            canJump = false;
        }
        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Space))
        {
            if (canJump && touchingGround)
            {
                //JustJumped = true;
                canJump = false;
                velocity *= 0.2f;
                velocity += new Vector2(0, 10).RotatedBy(jumpDegrees);
            }
        }
        bool moveLeft = Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D);
        bool moveRight = !Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D);
        if (moveLeft)
        {
            if(moveCounter == 0 && (touchingGround || !touchingWall))
                moveCounter = 0.1f;
        }
        if (Input.GetKey(KeyCode.S))
        {

        }
        if (moveRight)
        {
            if (moveCounter == 0 && (touchingGround || !touchingWall))
                moveCounter = -0.1f;
        }
        if(InTheAir)
        {
            if((!moveLeft && moveCounter > 0) || (!moveRight && moveCounter < 0))
            {
                moveCounter = 0;
            }
            else if(moveCounter != 0)
            {
                moveCounter = 150 * Mathf.Sign(moveCounter);
            }
        }            
        if(moveCounter != 0)
        {
            float dir = Mathf.Sign(moveCounter);
            float absCounter = Mathf.Abs(moveCounter);
            float sin = Mathf.Sin(absCounter / 120f * Mathf.PI);
            if (InTheAir)
                sin = 1;
            if (sin < 0)
                sin = 0;
            velocity.x = sin * dir * -3.6f;
            if(absCounter < 90)
                rb.freezeRotation = true;
            else
                rb.freezeRotation = false;
            float increment = (0.5f + 3.5f * sin) * dir;
            rotation += increment;
            if (absCounter >= 128 && !InTheAir)
            {
                moveCounter = 0;
            }
            else
            {
                moveCounter += increment;
            }
        }
        else
        { 
            rb.freezeRotation = false; 
        }
        if (rb.velocity.y < 0)
        {
            rb.gravityScale += 0.02f;
        }
        else
        {
            rb.gravityScale = 2;
        }
        arrow.SetActive(false);
        if (Vector2.Distance(transform.position, boulder.transform.position) < 1.1f)
        {
            Rigidbody2D rb2 = boulder.GetComponent<Rigidbody2D>();
            bool releaseMouse = lastMouseState && !Input.GetMouseButton(0);
            if (Input.GetMouseButton(0) || releaseMouse)
            {
                float maxDistance = 5;
                Vector2 toMouse = (Vector2)(mainCamera.ScreenToWorldPoint(Input.mousePosition) - boulder.transform.position);
                float scale = toMouse.magnitude;
                scale = Mathf.Min(scale, maxDistance);
                if(releaseMouse)
                {
                    rb2.gravityScale = 1.0f;
                    rb2.velocity *= 0.1f;
                    rb2.velocity += toMouse.normalized * scale * 6.1f;
                    justLaunchedBoulder = 100;
                }
                else
                {
                    arrow.SetActive(true);
                    arrow.transform.localScale = new Vector3(arrow.transform.localScale.x, scale, arrow.transform.localScale.z);
                    arrow.transform.position = (Vector2)boulder.transform.position + toMouse.normalized * scale * 0.5f;
                    arrow.transform.rotation = (toMouse.ToRotation() - Mathf.PI / 2).ToQuaternion();
                }
            }
            if (justLaunchedBoulder <= 0 && Input.GetMouseButton(1) && !releaseMouse && Vector2.Distance(transform.position, boulder.transform.position) < 1.05f)
            {
                rb2.gravityScale = 1.0f;
                Vector2 velocityToContribute = rb.velocity;
                velocityToContribute.y *= 0.25f;
                rb2.MovePosition((Vector2)boulder.transform.position + velocityToContribute * Time.fixedDeltaTime);
            }
        };
        justLaunchedBoulder--;
        rb.velocity = velocity;
        rb.rotation = rotation;
        touchingGround = touchingWall = false;
        lastMouseState = Input.GetMouseButton(0);
    }
}
