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
    public float jumpTime;
    private float jumpTimeCounter;
    private bool isJumping;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spr = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        
    }

    private void FixedUpdate()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(xInput * xSpeed, rb.velocity.y);
    }

    private void Update()
    {
        isGround = Physics2D.OverlapCircle(groundCheck.position, groundDist, groundLayer);

        if (xInput > 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (xInput < 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        
        if(isGround && Input.GetKeyDown(KeyCode.Space))
        {
            isJumping = true;
            jumpTimeCounter = jumpTime;
            rb.velocity = Vector2.up * ySpeed;
        }
        if (Input.GetKey(KeyCode.Space) && isJumping)
        {
            if (jumpTimeCounter > 0)
            {
                rb.velocity = Vector2.up * ySpeed;
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            isJumping = false;
        }
            
        //isGround = Physics2D.Raycast(groundCheck.position, Vector2.down, groundDist, groundLayer);
    }


    public void Jump()
    {
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(groundCheck.position, Vector2.down * groundDist);
    }
}

