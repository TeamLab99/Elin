using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    Animator anim;
    Rigidbody2D rb;
    SpriteRenderer spr;

    // 레이어 체크 변수
    private bool isGround;
    public float groundDist;
    public Transform groundCheck; 
    public LayerMask groundLayer;
    
    // 점프 관련 변수
    public float jumpPower;
    public float jumpStartTime;
    private float jumpTime;
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

    private void Update()
    {
        isGround = Physics2D.Raycast(groundCheck.position, Vector2.down, groundDist, groundLayer);
    }

    private void FixedUpdate()
    {
        Jump();
    }

    public void Jump()
    {
        if ( Input.GetButtonDown("Jump"))
        {
            isJumping = true;
            jumpTime = jumpStartTime;
            rb.velocity = Vector2.up * jumpPower;
        }
        if (Input.GetButton("Jump") && isJumping)
        {
            if (jumpTime > 0)
            {
                rb.velocity = Vector2.up * jumpPower;
                jumpTime -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
            if (Input.GetButtonUp("Jump"))
                isJumping = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(groundCheck.position, Vector2.down * groundDist);
    }
}

