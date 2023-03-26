using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Move : MonoBehaviour
{

    // GetComponent 관련 변수
    Animator anim;
    Rigidbody2D rb;
    SpriteRenderer spr;

    // 검출 관련 변수 
    [Header("검출 관련 변수 : 개발자 관련 변수")]
    public Transform groundFrontCheck; // 앞 다리 위치
    public Transform groundBackCheck; // 뒷 다리 위치
    public Transform checkPosition; // 벽,상호작용 객체 체크 위치
    public LayerMask groundLayer; // 땅 레이어
    public LayerMask wallLayer; // 벽 레이어
    public float groundDist; // 땅과의 거리
    public float wallDist; // 벽과의 거리
    private bool isWall; // 벽에 붙어있는가?
    private bool isGround; // 두 다리 중 하나라도 땅에 붙어 있는가?
    private bool isFrontGruond; // 앞 다리가 땅에 붙어 있는가?
    private bool isBackGruond; // 뒷 다리가 땅에 붙어 있는가?
    private bool isControlPlayer; // 플레이어의 움직임을 제어하는가?

    [Header("이동 관련 변수")]
   
    public float xSpeed; // 좌우 이동 속도
    public float ySpeed; // 점프 속도
    private float moveDir; // 방향키를 입력 받는다.
    private float jumpDir; // 점프의 입력을 받늗다.

    private float isRight=1; // 오른쪽을 보면 1, 왼쪽을 보면 0
    private float defaultGravity=2f; // 기본 중력
    private float fallGravity=2.5f; // 낙하 중력

    public bool canMove=true; // 캐릭터의 움직임을 조절하는 변수
   

    // 점프와  슈퍼점프 관련 변수들 
    public float chargeSpeed; // 점프 속도 충전 
    private float yMaxSpeed=0; // 최대 점프 속도
    public float jumpTime; // 점프 충전 시간
    private float jumpTimeCounter; // 점프 충전 
    private bool isSuperJump; // 슈퍼 점프를 하는 중인가?
    private bool isChargeJump; // 슈퍼 점프를 할 수 있는가?

    // 상태를 나타내는 변수들
   
    // 스캔 대상 체크
    public Player_Interact playerInteract;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spr = GetComponent<SpriteRenderer>();
    }

    // getaxis = -1.0~1.0f
    // getaxisraw = -1,0,1 3가지만 반환
    // getbuttondown = 버튼을 누를때 한번 true 발생
    // getbuttonup = 버튼을 눌렀다 땔 경우 true 발생
    // getbutton = 버튼을 누르고 있을 때 계속해서 true 발생
    // tirggerenter2d = 충돌 시, 한번만 호출
    // tirggerstay2d  = 충돌 시, 지속적으로 호출
    // exit = 충돌에서 벗어나면 한번 호출
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

    // 땅과 벽을 검출한다.
    void CheckingMap()
    {
        isWall = Physics2D.Raycast(checkPosition.position, Vector2.right * isRight, wallDist, wallLayer); // 바라보는 방향에 벽이 있는지 확인
        isFrontGruond = Physics2D.Raycast(groundFrontCheck.position, Vector2.down, groundDist, groundLayer); // 앞 다리가 땅에 있는지 확인
        isBackGruond = Physics2D.Raycast(groundBackCheck.position, Vector2.down, groundDist, groundLayer); // 뒷 다리가 땅에 있는지 확인
        if (isFrontGruond || isBackGruond) // 두 다리 중 하나라도 걸쳐 있다면 땅위에 있음
        {
            isGround = true;
            rb.gravityScale = defaultGravity;
        }
        else
            isGround = false;
    }

    void InputKey()
    {
        moveDir = Input.GetAxisRaw("Horizontal");
        jumpDir = Input.GetAxisRaw("Jump");
    }
    // 벽 점프 
    void WallJump()
    {
        if (Input.GetAxis("Jump") != 0)
        {
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
        moveDir = Input.GetAxisRaw("Horizontal");
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
        if (Mathf.Abs(moveDir) ==1)
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
        Gizmos.DrawRay(groundFrontCheck.position,Vector2.down*groundDist);
        Gizmos.DrawRay(groundBackCheck.position, Vector2.down * groundDist);
        // 벽 검출 광선 발사
        Gizmos.color = Color.red;
        Gizmos.DrawRay(checkPosition.position, Vector2.right * wallDist*isRight);
    }
}
