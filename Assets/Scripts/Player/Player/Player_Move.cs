using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Move : MonoBehaviour
{
    public static Player_Move instance;

    Animator anim;
    Rigidbody2D rb;
    SpriteRenderer spr;
    public Transform groundFrontCheck;
    public Transform groundBackCheck;
    public Transform wallChk;
    public LayerMask groundLayer;
    public LayerMask wallLayer;
    public float groundDist;
    public float wallDist;


    float defaultGravity; //기본 중력
    bool isGround; //땅에 붙어있는가?
    bool isFrontGruond;
    bool isBackGruond;
    bool isWall; //벽에 붙어있는가?
    bool isControlPlayer; //벽

    // 걷기 관련 변수들
    public float xSpeed;
    private float moveDir;
    private float isRight=1;

    // 점프와  슈퍼점프 관련 변수들 
    public float ySpeed;
    public float superSpeed;
    private float yMaxSpeed=0;
    public float jumpTime;
    private float jumpTimeCounter;
    private bool isSuperJump;
    private bool isChargeJump;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spr = GetComponent<SpriteRenderer>();
    }

    void Start() //초기 변수 제어
    {
        instance = this;
        defaultGravity = rb.gravityScale;
    }

    public void CheckingMap()
    {
        isWall = Physics2D.Raycast(wallChk.position, Vector2.right * isRight, wallDist, wallLayer);
        isFrontGruond = Physics2D.Raycast(groundFrontCheck.position, Vector2.down, groundDist, groundLayer);
        isBackGruond = Physics2D.Raycast(groundBackCheck.position, Vector2.down, groundDist, groundLayer);
        if (isFrontGruond || isBackGruond)
            isGround = true;
        else
            isGround = false;
    }

    void Update()
    {
        Animation();
        CheckingMap();
        InputSuperJump();
       
       
        if (!isControlPlayer) {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                isRight = -1;
                FlipX();
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                isRight = 1;
                FlipX();
            }
        } 

        
    }
    void FixedUpdate()
    {
        if (!isControlPlayer)
        {
            Walk();
            Jump();
            WallSlide();
            SuperJump();
        }
    }
    public void WallJump()
    {
        if (Input.GetAxis("Jump") != 0)
        {
            if (!isGround)
            {
                isControlPlayer = true;
                Invoke("FreezX", 0.3f);
                isRight *= -1;
                FlipX();
                rb.velocity = new Vector2(xSpeed * isRight, ySpeed * 0.8f);
            }
        }
    }
    private void WallSlide()
    {
        if (isWall)
        {
            isControlPlayer = false;
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.6f);
            WallJump();
        }
    } 
    private void Walk()
    {
        moveDir = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveDir * xSpeed, rb.velocity.y);    
    }
    private void Jump()
    {
        if (Input.GetAxis("Jump")!=0 && isGround && !isWall)
        {
            rb.velocity = new Vector2(rb.velocity.x, ySpeed);
        }
    }

    public void Animation()
    {
        if (Mathf.Abs(rb.velocity.x) > 0.1f)
            anim.SetBool("isRun", true);
        else
            anim.SetBool("isRun", false);
        if (rb.velocity.y > 0.1f)
            anim.SetBool("isFall", false);
        else
            anim.SetBool("isFall", true);
        if (isGround)
            anim.SetBool("isGround", true);
        else
            anim.SetBool("isGround", false);
        if (isWall)
            anim.SetBool("isWall", true);
        else
            anim.SetBool("isWall", false);
    }

    public void SuperJump()
    {
        if (isChargeJump)
        {
            rb.velocity = new Vector2(0, 10+yMaxSpeed*0.1f);
            yMaxSpeed = 0;
            isChargeJump = false;
        }
    }
    public void InputSuperJump()
    {
        if ( Input.GetKeyDown(KeyCode.C)&& isGround)
        {
            isControlPlayer = true;
            rb.velocity = Vector2.zero;
            isSuperJump = true;
            yMaxSpeed += superSpeed;
            jumpTimeCounter = jumpTime;
        }
        if (Input.GetKey(KeyCode.C) && isSuperJump)
        {
            if (jumpTimeCounter > 0)
            {
                yMaxSpeed += superSpeed;
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isSuperJump = false;
            }
        }
        if (Input.GetKeyUp(KeyCode.C) &&isGround )
        {
            if (yMaxSpeed > 100)
                yMaxSpeed = 100;
            isControlPlayer = false;
            isChargeJump = true;
            isSuperJump = false;
        }
    }

    private void FlipX()
    {
        if (isRight == -1)
            spr.flipX = true;
        else if(isRight== 1)
            spr.flipX = false;
    } //좌우전환
    private void FreezX()
    {
        isControlPlayer = false;
        FlipX();
    } //움직임 제어 풀기
  
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(groundFrontCheck.position,Vector2.down*groundDist);
        Gizmos.DrawRay(groundBackCheck.position, Vector2.down * groundDist);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(wallChk.position, Vector2.right * wallDist*isRight);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Spike")
            rb.AddForce(new Vector2(10*isRight, 5), ForceMode2D.Impulse);  
    } // 충돌
    private void OnTriggerEnter2D(Collider2D collision) // 트리거 충돌
    {
     
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
    }
}
