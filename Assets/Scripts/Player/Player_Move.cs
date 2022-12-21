using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Move : MonoBehaviour
{
    Animator anim;
    Rigidbody2D rb;
    SpriteRenderer spr;
    public Transform groundChk;
    public Transform wallChk;
    public LayerMask groundLayer;
    public LayerMask wallLayer;
    public float groundDist;
    public float wallDist;

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

    void Start()
    {
        isRight = 1;
    }

    
    void Update()
    {
        moveDir = Input.GetAxisRaw("Horizontal");
        jumpDir = Input.GetAxis("Jump");
        isGround = Physics2D.Raycast(groundChk.position, Vector2.down, groundDist, groundLayer);
        isWall = Physics2D.Raycast(wallChk.position, Vector2.right*isRight, wallDist, wallLayer);
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            isRight = -1;
        else if (Input.GetKeyDown(KeyCode.RightArrow))
            isRight = 1;
    }

    void FixedUpdate()
    {
        if (!isWallJump)
        {
            Walk();
            FlipX();
        }
        Jump();
        WallSlide();
    }
    private void FreezX()
    {
        isWallJump=false;
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
                rb.velocity = new Vector2(rb.velocity.x * 5 * isRight, rb.velocity.y * 8);
            }
        }
    }
    private void Walk()
    {
        rb.velocity = new Vector2(moveDir * 10, rb.velocity.y);
    }
    private void Jump()
    {
        if (isGround)
            rb.velocity = new Vector2(rb.velocity.x, jumpDir * 10);
    }
    private void FlipX()
    {
        if (isRight == -1)
            spr.flipX = true;
        else if(isRight== 1)
            spr.flipX = false;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(groundChk.position,Vector2.down*groundDist);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(wallChk.position, Vector2.right * wallDist*isRight);
    }
}
