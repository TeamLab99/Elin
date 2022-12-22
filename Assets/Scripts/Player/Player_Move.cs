using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Move : MonoBehaviour
{
    Animator anim;
    Rigidbody2D rb;
    SpriteRenderer spr;
    [SerializeField]
    public Transform groundChk;
    public Transform wallChk;
    public LayerMask groundLayer;
    public LayerMask wallLayer;
    public float groundDist;
    public float wallDist;
    public float jumpPower;
    public float walkSpeed;

    float moveDir;
    float jumpDir;
    float isRight;
    bool isGround;
    bool isWall;
    bool isWallJump;

    private void Awake()
    {
      
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spr = GetComponent<SpriteRenderer>();
    }

    void Start() //초기 변수 제어
    {
        isRight = 1;
    }

    void Update()
    {
        jumpDir = Input.GetAxis("Jump");
        isGround = Physics2D.Raycast(groundChk.position, Vector2.down, groundDist, groundLayer);
        isWall = Physics2D.Raycast(wallChk.position, Vector2.right*isRight, wallDist, wallLayer);
        if(!isWallJump){
            moveDir = Input.GetAxisRaw("Horizontal");
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
        } //좌우 전환 + 좌우 움직임
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
            WallSlide();
        }
    }
   
    private void WallSlide()
    {
        if (isWall)
        {
            isWallJump = false;
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.6f);
            if (Input.GetAxis("Jump")!=0)
            {
                isWallJump = true;
                Invoke("FreezX", 0.3f);
                isRight *= -1;
                FlipX();
                rb.velocity = new Vector2(walkSpeed*isRight, jumpPower*0.8f);
            }
        }
    } //벽타기 + 벽점프
    private void Walk()
    {   
        if(isGround)
            rb.velocity = new Vector2(moveDir * walkSpeed, 0);
        else
            rb.velocity = new Vector2(moveDir * walkSpeed, rb.velocity.y);
    } //걷기
    private void Jump()
    {
        if (isGround)
            rb.velocity = new Vector2(rb.velocity.x, jumpDir * jumpPower);
    } //점프
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
        Gizmos.DrawRay(groundChk.position,Vector2.down*groundDist);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(wallChk.position, Vector2.right * wallDist*isRight);
    } //검출선
}
