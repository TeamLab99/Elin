using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer spr;

    [Header("Ground Check")]
    [SerializeField] Transform footPos;
    [SerializeField] Transform checkPos;
    [SerializeField] LayerMask groundLayer;

    private bool isGround;
    private Vector2 boxSize = new Vector2(1f, 0.3f);

    [Header("Move Var")]
    [SerializeField] private float xSpeed; 
    [SerializeField] private float ySpeed; 
    private float moveDir;

    private float jumpTime  = 0f;
    private float chargeTime = 0.2f;
    private bool isJump = false;

    [Header("Control Player")]
    private bool canMove = true;
    private bool keyDownJump;
    private bool keyJump;
    private bool keyUpJump;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (canMove)
        {
            MoveKeyInput();
            Jump();
        }
        FlipPlayer();
        CheckGround();
    }

    private void MoveKeyInput()
    {
        moveDir = Input.GetAxisRaw("Horizontal");
        keyDownJump = Input.GetKeyDown(KeyCode.Space);
        keyJump = Input.GetKey(KeyCode.Space);
        keyUpJump = Input.GetKeyUp(KeyCode.Space);
    }

    private void Jump()
    {
        if (keyDownJump && isGround)
        {
            isJump = true;
            jumpTime = chargeTime;
            rb.velocity = new Vector2(rb.velocity.x, ySpeed);
        }
        else if (keyJump && isJump)
        {
            if (jumpTime > 0f)
            {
                rb.velocity = new Vector2(rb.velocity.x, ySpeed);
                jumpTime -= Time.deltaTime;
            }
        }
        else if (keyUpJump)
            isJump = false;
    }

    private void FlipPlayer()
    {
        if (moveDir == 1)
            spr.flipX = false;
        else if (moveDir == -1)
            spr.flipX = true;
    }

    private void CheckGround()
    {
        if (Physics2D.OverlapBox(footPos.position, boxSize, 0f, groundLayer))
            isGround = true;
        else
            isGround = false;
    }

    private void FixedUpdate()
    {
        Walk();
    }

    private void Walk()
    {
        rb.velocity = new Vector2(xSpeed * moveDir, rb.velocity.y);
    }

    public void ControlPlayer(bool _canMove)
    {
        canMove = _canMove;
        if (!canMove)
        {
            moveDir = 0;
            while (true)
            {
                if (isGround)
                {
                    rb.velocity = Vector3.zero;
                    break;
                }
            }
        }
    }

    public void Dead()
    {
        Debug.Log("죽었습니다.");
    }

    void OnDrawGizmos()
    {
        //Gizmos.color = Color.green;
        //Gizmos.DrawRay(groundCheckPos.position,Vector2.down*groundDistY);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(footPos.position, boxSize);
    }

}
