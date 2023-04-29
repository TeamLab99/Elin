using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerAnimation{
    Idle,
    Run,
    Jump,
    Fall,
    Slide
}

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
    Rigidbody2D rb;
    SpriteRenderer spr;
    bool canMove = true;
    public LayerMask wallLayer = 7;
    public LayerMask groundLayer;

    [Header("Ground Check")]
    [SerializeField] Transform groundCheckPos;
    public float groundDistY;
    public float groundDistX;

    [Header("Wall Check")]
    [SerializeField] float wallDist; // 벽과의 거리
    private bool isWall; // 벽에 붙어있는가?
    private bool isGround; // 두 다리 중 하나라도 땅에 붙어 있는가?
    private bool isControlPlayer; // 플레이어의 움직임을 제어하는가?

    [Header("Move Speed")]
    [SerializeField] private float xSpeed; // 좌우 이동 속도
    [SerializeField] private float ySpeed; // 점프 속도
    private float defaultGravity = 2f; // 기본 중력
    private float fallGravity = 2.5f; // 낙하 중력

    private float moveDir; // 방향키를 입력 받는다.
    private float jumpDir; // 점프의 입력을 받늗다.
    private float isRight=1; // 오른쪽을 보면 1, 왼쪽을 보면 0
   
    // 점프와  슈퍼점프 관련 변수들 
    public float chargeSpeed; // 점프 속도 충전 
    private float yMaxSpeed = 0; // 최대 점프 속도
    public float jumpTime; // 점프 충전 시간
    private float jumpTimeCounter; // 점프 충전 
    private bool isSuperJump; // 슈퍼 점프를 하는 중인가?
    private bool isChargeJump; // 슈퍼 점프를 할 수 있는가?

    PlayerAnimation playerAnimation=PlayerAnimation.Idle;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        InputKey();
        Animation();
        CheckingMap();
        ReverseSprite();
        InputSuperJump();
    }
    void FixedUpdate()
    {
        SetGravity();
        if (canMove)
        {
            if (!isControlPlayer)
            {
                Walk();
                Jump();
                WallSlide();
                SuperJump();
            }
        }
        else
            rb.velocity = new Vector2(0, rb.velocity.y);
    }

    void InputKey()
    {
        moveDir = Input.GetAxisRaw("Horizontal");
        jumpDir = Input.GetAxisRaw("Jump");
    }

    void CheckingMap()
    {
        if (Physics2D.Raycast(groundCheckPos.position+new Vector3(groundDistX,0,0), Vector2.down, groundDistY, groundLayer) || 
            Physics2D.Raycast(groundCheckPos.position- new Vector3(groundDistX,0, 0), Vector2.down, groundDistY, groundLayer)) // 두 다리 중 하나라도 걸쳐 있다면 땅위에 있음
        {
            isGround = true;
            rb.gravityScale = defaultGravity;
        }
        else
            isGround = false;

        
    }

    void WallJump()
    {
        if (Input.GetAxis("Jump") != 0)
        {
            //eventCameraEffects.TriggerCameraEffects(0);
            if (!isGround)
            {
                isControlPlayer = true;
                Invoke("ControlPlayer", 0.3f);
                isRight *= -1;
                FlipX();
                rb.velocity = new Vector2(xSpeed * isRight, ySpeed * 0.8f);
            }
        }
    }
    // 벽 슬라이드
    void WallSlide()
    {
        if (isWall)
        {
            isControlPlayer = false;
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.6f);
            WallJump();
        }
    } 
    // 걷기
    void Walk()
    {
        if (moveDir > 0.25f)
            moveDir = 1f;
        else if (moveDir < -0.25f)
            moveDir = -1f;
        else
            moveDir = 0f;
       rb.velocity = new Vector2(moveDir * xSpeed, rb.velocity.y);
    }
    // 점프
    void Jump()
    {
        if (isGround && !isWall)
            rb.velocity = new Vector2(rb.velocity.x, ySpeed * jumpDir);
    }
    // 애니메이션 
    void Animation()
    {
        if(Mathf.Abs(moveDir)>0.1f)
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

        /*switch (animationState)
        {
            case AnimationState.Idle:
                anim.SetInteger("State", 0);
                break;
            case AnimationState.Run:
                anim.SetInteger("State", 1);
                break;
            case AnimationState.Jump:
                anim.SetInteger("State", 2);
                break;
            case AnimationState.Fall:
                anim.SetInteger("State", 3);
                break;
            case AnimationState.WallSlide:
                anim.SetInteger("State", 4);
                break;
        }*/
    }
    // 슈퍼 점프 입력 (선행)
    void InputSuperJump()
    {
        if ( Input.GetKeyDown(KeyCode.C)&& isGround) // 땅에 붙어 있고, 
        {
            isSuperJump = true;
            isControlPlayer = true; // 어떤 키 입력도 받지 않게 됨
            rb.velocity = Vector2.zero; // 기 모으는 동안 움직이기 않게 하기 위함 
            jumpTimeCounter = jumpTime; // 점프 시간 초기화
        }
        if (Input.GetKey(KeyCode.C) && isSuperJump) // 슈퍼 점프를 하는중일때
        {
            if (jumpTimeCounter > 0)
            {
                yMaxSpeed +=chargeSpeed; // 슈퍼 점프의 강도가 더 강해짐
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isSuperJump = false;
            }
        }
        if (Input.GetKeyUp(KeyCode.C) &&isGround )
        {
            if (yMaxSpeed > 80) // 최대 속도 조절
                yMaxSpeed = 80;
            if (yMaxSpeed != 0) 
            {
                isControlPlayer = false;
                isChargeJump = true;
                isSuperJump = false;
            }
        }
    }
    // 슈퍼 점프 (결과)
    void SuperJump()
    {
        if (isChargeJump)
        {
            rb.velocity = new Vector2(0, ySpeed + yMaxSpeed * 0.1f);
            yMaxSpeed = 0;
            isChargeJump = false;
        }
    }
    // 스프라이트 반전   
    void FlipX()
    {
        if (isRight == -1)
            spr.flipX = true;
        else if(isRight== 1)
            spr.flipX = false;
    }
    // 좌우 키 입력에 따른 반전
    void ReverseSprite()
    {
        if (!isControlPlayer)
        {
            if (moveDir==-1)
            {
                isRight = -1;
                FlipX();
            }
            else if (moveDir==1)
            {
                isRight = 1;
                FlipX();
            }
        }
    }

    // 움직임 제어 풀기
    void ControlPlayer()
    {
        isControlPlayer = false;
        FlipX();
    }

    // 점프하면 중력 증가
    void SetGravity()
    {
        if (isGround)
            rb.gravityScale = defaultGravity;
        else
            rb.gravityScale = fallGravity;
    }

    void OnDrawGizmos()
    {
        // 땅 검출 광선 발사
        Gizmos.color = Color.green;
        Gizmos.DrawRay(groundCheckPos.position,Vector2.down*groundDistY);
        Gizmos.DrawRay(groundCheckPos.position, Vector2.down*groundDistY);
        // 벽 검출 광선 발사
        Gizmos.color = Color.red;
       // Gizmos.DrawRay(checkPosition.position, Vector2.right * wallDist*isRight);
    }
    
}
