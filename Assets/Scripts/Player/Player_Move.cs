using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Move : MonoBehaviour
{
    Animator anim;
    Rigidbody2D rb;
    public Transform groundChk;
    public Transform wallChk;
    public LayerMask groundLayer;
    public LayerMask wallLayer;

    public float groundDist;
    public float wallDist;

    float moveDir;
    float jumpDir;
    bool isRight;
    bool isGround;
    bool isWall;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        
    }

    
    void Update()
    {
        moveDir = Input.GetAxisRaw("Horizontal");
        jumpDir = Input.GetAxis("Jump");
        isGround = Physics2D.Raycast(groundChk.position, Vector2.down, groundDist, groundLayer);
    }

    void FixedUpdate()
    {
        Walk();
        Jump();
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
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(groundChk.position,Vector2.down*groundDist);
        Gizmos.DrawRay(wallChk.position, Vector2.right * wallDist*moveDir);
    }
}
