using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Move : MonoBehaviour
{

    // getaxis = -1.0~1.0f
    // getaxisraw = -1,0,1 3가지만 반환
    // getbuttondown = 버튼을 누를때 한번 true 발생
    // getbuttonup = 버튼을 눌렀다 땔 경우 true 발생
    // getbutton = 버튼을 누르고 있을 때 계속해서 true 발생
    // tirggerenter2d = 충돌 시, 한번만 호출
    // tirggerstay2d  = 충돌 시, 지속적으로 호출
    // exit = 충돌에서 벗어나면 한번 호출

    // GetComponent 관련 변수
    Animator anim;
    public Rigidbody2D rb;
    SpriteRenderer spr;

    bool canMove = true;
    public LayerMask groundLayer;

    [Header("Ground Check")]
    [SerializeField] Transform footPos;
    [SerializeField] Transform checkPos;
    public float slopeDistance;
    public float angle;
    public Vector2 perp;
    public bool isSlope;

    public Vector2 boxSize;
    private bool isGround; // 두 다리 중 하나라도 땅에 붙어 있는가?

    [Header("Move Speed")]
    [SerializeField] private float xSpeed; // 좌우 이동 속도
    [SerializeField] private float ySpeed; // 점프 속도

    private float moveDir; // 방향키를 입력 받는다.
    private float jumpDir; // 점프의 입력을 받늗다.
   
    private float jumpTime;
    private float chargeTime=0.2f;
    private bool isJump;
    private float maxSpeed=10f;
    bool keyDownJump;
    bool keyJump;
    bool keyUpJump;

    public void ControlPlayer(bool _canMove)
    {
        canMove = _canMove;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        CheckGround();
        if (canMove)
        {
            MoveKeyInput();
            Jump();
        }
        StopSlopeSlide();
        FlipPlayer();
        CheckSlope();
    }

    void FixedUpdate()
    {
        Walk();
    }


    void CheckSlope()
    {
        RaycastHit2D hit = Physics2D.Raycast(checkPos.position, Vector2.down, slopeDistance, groundLayer);
        perp = Vector2.Perpendicular(hit.normal).normalized;
        angle=Vector2.Angle(hit.normal, Vector2.up);
        Debug.DrawLine(hit.point, hit.point + hit.normal, Color.black);
        Debug.DrawLine(hit.point, hit.point + perp, Color.white);
        if (angle != 0)
        {
            isSlope = true;
        }
        else
        {
            isSlope = false;
        }
    }
    void Walk()
    {
       /* rb.AddForce(Vector2.right * moveDir * xSpeed, ForceMode2D.Impulse);
        if (rb.velocity.x > maxSpeed)
            rb.velocity = new Vector2(maxSpeed, rb.velocity.y);
        else if (rb.velocity.x < -maxSpeed)
            rb.velocity = new Vector2(-maxSpeed, rb.velocity.y);*/
        if (isSlope && isGround &&!isJump)
        {
            rb.velocity = perp * maxSpeed * moveDir * -1f;
        }
        else if (!isSlope && isGround&&!isJump)
        {
            rb.velocity = new Vector2(moveDir * maxSpeed, 0);
        }
        else
        {
            rb.velocity = new Vector2(moveDir * maxSpeed, rb.velocity.y);
        }
       
       
    }

    void MoveKeyInput()
    {
        moveDir = Input.GetAxisRaw("Horizontal");
        keyDownJump = Input.GetKeyDown(KeyCode.Space);
        keyJump = Input.GetKey(KeyCode.Space);
        keyUpJump = Input.GetKeyUp(KeyCode.Space);
    }

    void CheckGround()
    {
        if (Physics2D.OverlapBox(footPos.position, boxSize, 0f, groundLayer))
            isGround = true;
        else
            isGround = false;
    }

    void StopSlopeSlide()
    {
        if (moveDir == 0)
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        else
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
   
    void Jump()
    {
        if (keyDownJump && isGround)
        {
            isJump = true;
            jumpTime = chargeTime;
            rb.velocity = new Vector2(rb.velocity.x, ySpeed);
        }
        else if (keyJump && isJump)
        {
            if (jumpTime > 0f)
            {
                rb.velocity = new Vector2(rb.velocity.x, ySpeed);
                jumpTime -= Time.deltaTime;
            }
        }
        else if (keyUpJump)
            isJump = false;
    }

    void FlipPlayer()
    {
        if (moveDir == 1)
            spr.flipX = false;
        else if (moveDir == -1)
            spr.flipX = true;
    }

    void OnDrawGizmos()
    {
        //Gizmos.color = Color.green;
        //Gizmos.DrawRay(groundCheckPos.position,Vector2.down*groundDistY);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(footPos.position, boxSize);
    }
   


    public void Dead()
    {
        // 죽는 모션 
        // 다시 리스폰 하도록 함.

    }
}
