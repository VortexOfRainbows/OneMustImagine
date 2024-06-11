using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public static Camera MainCamera;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject boulder;
    [SerializeField] private GameObject arrow;
    [SerializeField] private GameObject jumpCollider;
    [SerializeField] private GameObject UICanvas;
    [SerializeField] private Text HeightText;
    private Rigidbody2D rb;
    private float StartingHeight = -36.74f;
    private float CurrentHeight = -36.74f;
    private float FinalHeight = 68.77f;
    private float HeightPercent => (CurrentHeight - StartingHeight) / (FinalHeight - StartingHeight);
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        HeightText.text = (HeightPercent * 100f).ToString("##");
        if(HeightText.text == string.Empty)
        {
            HeightText.text += "0";
        }
        HeightText.text += " m";
        Vector2 lerp = Vector2.Lerp(mainCamera.transform.position, transform.position, 0.8f);
        mainCamera.transform.position = new Vector3(lerp.x, lerp.y + 2.5f, mainCamera.transform.position.z);
        if(mainCamera != null)
        {
            Player.MainCamera = mainCamera;
        }
    }
    public bool InTheAir => !touchingGround && !touchingWall;
    private float moveCounter = 0;
    //private bool JustJumped = false;
    private bool CanJump => canJumpTimer > 0;
    private int canJumpTimer = 0;
    private bool touchingGround = false;
    private bool touchingWall = false;
    private bool touchingIce = false;
    private float jumpDegrees = 0;
    private float justLaunchedBoulder = 0;
    public static bool BoulderJustThrown { get; private set; }
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
        if (collision.CompareTag("Ice"))
        {
            touchingIce = true;
        }
        if (!touchingGround && !collision.CompareTag("Wind") && !collision.CompareTag("GameEnd"))
            touchingWall = true;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Ice"))
        {
            touchingIce = true;
        }
        if (!touchingGround && !collision.CompareTag("Wind") && !collision.CompareTag("GameEnd"))
            touchingWall = true;
        if (collision.CompareTag("GameEnd"))
        {
            UICanvas.gameObject.SetActive(true);
            Time.timeScale = 0.5f;
        }
    }
    public void RefreshJump()
    {
        if (rb.velocity.y <= 0)
        {
            canJumpTimer = 60;
            rb.angularVelocity *= 0.0f;
        }
        touchingGround = true;
    }
    private bool lastMouseState;
    private float totalMoveDebug = 0;
    private void FixedUpdate()
    {
        Vector2 velocity = rb.velocity;
        float rotation = rb.rotation;
        if (touchingGround)
        {
            if(touchingIce)
            {
                velocity.x *= 0.9995f;
            }
            else
            {
                velocity.x *= 0.9f;
            }
        }
        else
        {
            velocity.x *= 0.95f;
            canJumpTimer--;
        }
        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Space))
        {
            if (CanJump && (touchingGround || canJumpTimer > 0))
            {
                //JustJumped = true;
                canJumpTimer = 0;
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
                //Debug.Log("reset movement: " + moveCounter);
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
            //velocity.x = sin * dir * -4.25f;
            if (touchingGround)
            {
                if (touchingIce)
                {
                    velocity.x *= 0.9875f;
                }
                else
                {
                    velocity.x *= 0.9f;
                }
            }
            else
            {
                velocity.x *= 0.94f;
                canJumpTimer--;
            }
            float maxSpeed = Mathf.Abs(sin * dir * -6.4f);
            if (Mathf.Abs(velocity.x) < maxSpeed) //-104.5728 is the speed avg over the movement time
            {
                velocity.x += sin * dir * -0.575f;
            }
            totalMoveDebug += velocity.x;
            if (absCounter < 90)
                rb.freezeRotation = true;
            else
                rb.freezeRotation = false;
            float increment = (0.5f + 3.75f * sin) * dir;
            rotation += increment;
            if (absCounter >= 122 && !InTheAir)
            {
                moveCounter = 0;
            }
            else
            {
                moveCounter += increment;
            }
            Debug.Log(totalMoveDebug);
        }
        else
        {
            totalMoveDebug = 0;
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
        BoulderJustThrown = false;
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
                    BoulderJustThrown = true;
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
                BoulderJustThrown = true;
                rb2.gravityScale = 1.0f;
                Vector2 velocityToContribute = rb.velocity;
                velocityToContribute.y *= InTheAir ? 0.0f : 1.0f;
                rb2.velocity = new Vector2(rb2.velocity.x * 0.95f, rb2.velocity.y);
                rb2.MovePosition((Vector2)boulder.transform.position + velocityToContribute * Time.fixedDeltaTime * 1.01f);
            }
        };
        justLaunchedBoulder--;
        rb.velocity = velocity;
        rb.rotation = rotation;
        touchingGround = touchingWall = touchingIce = false;
        lastMouseState = Input.GetMouseButton(0);
        
        if(boulder.GetComponent<Boulder>().WasTouchingGround)
        {
            CurrentHeight = boulder.transform.localPosition.y;
            if (CurrentHeight > FinalHeight)
                CurrentHeight = FinalHeight;
        }
    }
}
