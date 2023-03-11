using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Move : MonoBehaviour
{
    public static Player_Move instance;

    // GetComponent 관련 변수
    Animator anim;
    Rigidbody2D rb;
    SpriteRenderer spr;
    
    // 검출 관련 변수 
    public Transform groundFrontCheck; // 앞 다리 위치
    public Transform groundBackCheck; // 뒷 다리 위치
    public Transform wallCheck; // 벽 체크 위치
    public LayerMask groundLayer; // 땅 레이어
    public LayerMask wallLayer; // 벽 레이어
    public float groundDist; // 땅과의 거리
    public float wallDist; // 벽과의 거리
    private bool isWall; // 벽에 붙어있는가?
    private bool isGround; // 두 다리 중 하나라도 땅에 붙어 있는가?
    private bool isFrontGruond; // 앞 다리가 땅에 붙어 있는가?
    private bool isBackGruond; // 뒷 다리가 땅에 붙어 있는가?
    private bool isControlPlayer; // 플레이어의 움직임을 제어하는가?
    //private bool isInteraction=false; // 상호작용 하는가?

    public Transform particleEffect;
    public GameObject particle;

    // 걷기 관련 변수들
    public float xSpeed; // 좌우 이동 속도
    private float moveDir; // 방향키를 입력 받는 
    private float isRight=1; // 오른쪽을 보면 1, 왼쪽을 보면 0
    private float defaultGravity; // 기본 중력

    // 점프와  슈퍼점프 관련 변수들 
    public float ySpeed; // 점프 속도
    public float chargeSpeed; // 점프 속도 충전 
    private float yMaxSpeed=0; // 최대 점프 속도
    public float jumpTime; // 점프 충전 시간
    private float jumpTimeCounter; // 점프 충전 
    private bool isSuperJump; // 슈퍼 점프를 하는 중인가?
    private bool isChargeJump; // 슈퍼 점프를 할 수 있는가?

    // 상태를 나타내는 변수들
    private bool isHit=false;
    private GameObject deBuff;

    private Vector2 frontDir;
    private GameObject scanObject;
    public GameObject dialogueBox;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spr = GetComponent<SpriteRenderer>();
        frontDir = Vector2.right;
    }
    private void Start() 
    {
        instance = this;
        defaultGravity = rb.gravityScale;
    }
    void Update()
    {
       // InputKey();
        Animation();
        CheckingMap();
        ReverseSprite();
        InputSuperJump();
        if (scanObject != null && Input.GetKeyDown(KeyCode.Q))
            dialogueBox.SetActive(true);
        else if(scanObject==null)
            dialogueBox.SetActive(false);
    }
    void FixedUpdate()
    {
        if (!isHit)
        {
            if (!isControlPlayer)
            {
                Walk();
                Jump();
                WallSlide();
                SuperJump();
            }
            IncreaseGravity();
        }
        if (isRight == 1)
            frontDir = Vector2.right;
        else if (isRight == -1)
            frontDir = Vector2.left;
        RaycastHit2D rayNpc = Physics2D.Raycast(rb.position, frontDir, 1f, LayerMask.GetMask("NPC"));
        if (rayNpc.collider != null)
            scanObject = rayNpc.collider.gameObject;
        else
            scanObject = null;
    }
    // 땅과 벽을 검출한다.
    private void CheckingMap()
    {
        isWall = Physics2D.Raycast(wallCheck.position, Vector2.right * isRight, wallDist, wallLayer); // 바라보는 방향에 벽이 있는지 확인
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
    // 벽 점프 
    private void WallJump()
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
    private void WallSlide()
    {
        if (isWall)
        {
            isControlPlayer = false;
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.6f);
            WallJump();
        }
    } 
    // 걷기
    private void Walk()
    {
        moveDir = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveDir * xSpeed, rb.velocity.y);    
    }
    // 점프
    private void Jump()
    {
        if (Input.GetAxis("Jump") != 0 && isGround && !isWall)
        {
            rb.velocity = new Vector2(rb.velocity.x, ySpeed);
            //Instantiate(particle, particleEffect.transform.position, transform.rotation);
        }
            
    }
    // 애니메이션 
    private void Animation()
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
    // 슈퍼 점프 입력 (선행)
    private void InputSuperJump()
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
    private void SuperJump()
    {
        if (isChargeJump)
        {
            rb.velocity = new Vector2(0, ySpeed + yMaxSpeed * 0.1f);
            yMaxSpeed = 0;
            isChargeJump = false;
        }
    }
    // 스프라이트 반전   
    private void FlipX()
    {
        if (isRight == -1)
            spr.flipX = true;
        else if(isRight== 1)
            spr.flipX = false;
    }
    // 좌우 키 입력에 따른 반전
    private void ReverseSprite()
    {
        if (!isControlPlayer)
        {
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
    // 움직임 제어 풀기
    private void ControlPlayer()
    {
        isControlPlayer = false;
        FlipX();
    }
    // 낙하 시 중력 증가
    private void IncreaseGravity()
    {
        if (rb.velocity.y < 0)
            rb.gravityScale = 3;
    }
    // 플레이어 검출선 그리기
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(groundFrontCheck.position,Vector2.down*groundDist);
        Gizmos.DrawRay(groundBackCheck.position, Vector2.down * groundDist);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(wallCheck.position, Vector2.right * wallDist*isRight);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Spike")
        {
            OnDamaged(collision.transform.position);
        }
    } // 충돌

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("DeBuff")){
            deBuff = collision.gameObject;
            deBuff.SetActive(false);
            xSpeed = 7;
            Invoke("CoolDownDeBuff", 3f);
        } 
    }  

    void OnDamaged(Vector2 targetPos)
    {
        isHit = true;
        gameObject.layer = 12;
        spr.color = new Color(1, 1, 1,0.4f);
        StartCoroutine("CoolDownSpike");
    }
    IEnumerator CoolDownSpike()
    {
        yield return new WaitForSeconds(1f);
        spr.color = new Color(1, 1, 1, 1f);
        isHit = false;
        gameObject.layer = 3;
        yield break;
    }

    void CoolDownDeBuff()
    {
        xSpeed = 10;
        ySpeed = 10;
    }
}
