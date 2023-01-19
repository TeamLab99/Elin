using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    Animator anim;
    Rigidbody2D rb;
    SpriteRenderer spr;

    // 이동 관련 변수
    public float xSpeed;
    private float xInput;

    // 레이어 체크 변수
    private bool isGround;
    public float groundDist;
    public Transform groundCheck; 
    public LayerMask groundLayer;

    // 점프 관련 변수
    public float ySpeed;

    // 슈퍼점프 관련 변수
    public float spuerSpeed; 
    private float yMaxSpeed;
    public float jumpTime;
    private float jumpTimeCounter;
    private bool isSuperJump;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spr = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        yMaxSpeed = 5;
    }

    private void FixedUpdate()
    {
        Walk();

    }

    public void Walk()
    {
        xInput = Input.GetAxisRaw("Horizontal"); // X축 이동을 입력 받는다. (좌,우를 나타낸다)
        rb.velocity = new Vector2(xInput * xSpeed, rb.velocity.y); // 입력받은 X축 이동에 따른 속도 변화
    }

    public void Jump()
    {
        if(Input.GetKeyDown(KeyCode.Space))
            rb.velocity = new Vector2(rb.velocity.x, ySpeed); // 입력받은 X축 이동에 따른 속도 변화
    }

    private void Update()
    {
        isGround = Physics2D.OverlapCircle(groundCheck.position, groundDist, groundLayer);
        SuperJump();
        Jump();
        //isGround = Physics2D.Raycast(groundCheck.position, Vector2.down, groundDist, groundLayer);
    }


    public void SuperJump()
    {
        if (isGround && Input.GetKeyDown(KeyCode.C))
        {
            isSuperJump = true;
            yMaxSpeed += spuerSpeed;
            jumpTimeCounter = jumpTime;
            //rb.velocity = Vector2.up * spuerSpeed;
        }
        if (Input.GetKey(KeyCode.C) && isSuperJump)
        {
            if (jumpTimeCounter > 0)
            {
                yMaxSpeed += spuerSpeed;
                //rb.velocity = Vector2.up * spuerSpeed;
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isSuperJump = false;
            }
        }
        if (Input.GetKeyUp(KeyCode.C)&&isGround)
        {
            rb.velocity = Vector2.up * yMaxSpeed;
            isSuperJump = false;
            yMaxSpeed = 5;
        }
    }
}

