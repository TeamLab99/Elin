using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Move : MonoBehaviour
{
    public static Player_Move instance;

    Animator anim;
    Rigidbody2D rb;
    SpriteRenderer spr;
    [SerializeField]
    public Transform groundCheck;
    public Transform wallChk;
    public LayerMask groundLayer;
    public LayerMask wallLayer;
    public float groundDist;
    public float wallDist;


    float defaultGravity; //기본 중력
    bool isGround; //땅에 붙어있는가?
    bool isWall; //벽에 붙어있는가?
    bool isWallJump; //벽

    // 걷기 관련 변수들
    public float xSpeed;
    private float moveDir;
    private float isRight;

    // 점프와  슈퍼점프 관련 변수들 
    public float ySpeed;
    public float superSpeed;
    private float yMaxSpeed;
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
        isRight = 1;
        defaultGravity = rb.gravityScale;
        yMaxSpeed = 5;
    }

    void Update()
    {
        moveDir = Input.GetAxisRaw("Horizontal");
        isGround = Physics2D.Raycast(groundCheck.position, Vector2.down, groundDist, groundLayer);
        isWall = Physics2D.Raycast(wallChk.position, Vector2.right * isRight, wallDist, wallLayer);
        InputSuperJump();
        if (!isWallJump) {
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
        } //좌우 전환 + 좌우 움직임 (벽 점프중이 아닐 시에만)


        {
            if (Mathf.Abs(moveDir) > 0.1f)
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
        } //애니메이션 표기
    }
    void FixedUpdate()
    {
        if (!isWallJump)
        {
            Walk();
            Jump();
            SuperJump();
            WallSlide();
        } // 벽 점프중이 아닐 시 움직임
    }

    private void WallSlide()
    {
        if (isWall)
        {
            isWallJump = false;
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.6f);
            if (Input.GetAxis("Jump") != 0)
            {
                isWallJump = true;
                Invoke("FreezX", 0.3f);
                isRight *= -1;
                FlipX();
                rb.velocity = new Vector2(xSpeed * isRight, ySpeed * 0.8f);
            }
        }
    } //벽타기 + 벽점프
    private void Walk()
    {
        if (isGround)
            rb.velocity = new Vector2(moveDir * xSpeed, 0);
        else
            rb.velocity = new Vector2(moveDir * xSpeed, rb.velocity.y);
    } //걷기
    private void Jump()
    {
        if (Input.GetAxis("Jump")!=0 && isGround)
        {
            rb.velocity = new Vector2(rb.velocity.x, ySpeed);
        }
    }

    public void SuperJump()
    {
        if (isChargeJump)
        {
            rb.velocity = new Vector2(0, yMaxSpeed*0.2f);
            yMaxSpeed = 5;
            isChargeJump = false;
        }
       
    }
    public void InputSuperJump()
    {
        if ( Input.GetKeyDown(KeyCode.C)&& isGround)
        {
            isWallJump = true;
            Debug.Log("작동합니다1.");
            isSuperJump = true;
            yMaxSpeed += superSpeed;
            jumpTimeCounter = jumpTime;
           // rb.velocity = Vector2.up * 10;
        }
        if (Input.GetKey(KeyCode.C) && isSuperJump)
        {
            if (jumpTimeCounter > 0)
            {
                yMaxSpeed += superSpeed;
                //rb.velocity = Vector2.up * spuerSpeed;
                jumpTimeCounter -= Time.deltaTime;
                Debug.Log("작동합니다.2");
                if (yMaxSpeed > 100)
                    yMaxSpeed = 100;
            }
            else
            {
                isSuperJump = false;
            }
        }
        if (Input.GetKeyUp(KeyCode.C) &&isGround )
        {
            Debug.Log("작동합니다3.");
            isChargeJump = true;
            Debug.Log(yMaxSpeed);
            isWallJump = false;
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
        isWallJump = false;
        FlipX();
    } //움직임 제어 풀기
  
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(groundCheck.position,Vector2.down*groundDist);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(wallChk.position, Vector2.right * wallDist*isRight);
    } //검출선

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
