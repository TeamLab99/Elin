using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("컴포넌트")]
    Rigidbody2D rb;
    [SerializeField] Animator anim;
    [SerializeField] SpriteRenderer spr;

    [Header("땅 검출")]
    [SerializeField] Transform footPos;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Vector2 boxSize = new Vector2(1f, 0.3f);
    private bool isGround;
  
    [Header("움직임")]
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
    private bool keyDownArrow;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (canMove)
        {
            MoveKeyInput();
        }
        FlipPlayer();
        CheckGround();
        PlayerAnimation();
    }

    private void MoveKeyInput()
    {
        moveDir = Input.GetAxisRaw("Horizontal");
        keyDownJump = Input.GetKeyDown(KeyCode.Space);
        keyJump = Input.GetKey(KeyCode.Space);
        keyUpJump = Input.GetKeyUp(KeyCode.Space);
        keyDownArrow = Input.GetKey(KeyCode.DownArrow);
    }

    private void PlayerAnimation()
    {
        anim.SetBool("Ground", isGround);
        anim.SetFloat("YVelocity", rb.velocity.y);
        if (moveDir == 0)
            anim.SetBool("Walk", false);
        else
            anim.SetBool("Walk", true);
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
        Jump();
    }

    private void Walk()
    {
        if (isGround && moveDir != 0)
        {
            anim.SetBool("Walk", true);
        }else if(isGround && moveDir ==0)
            anim.SetBool("Walk", false);
        rb.velocity = new Vector2(xSpeed * moveDir, rb.velocity.y);
    }

    private void Jump()
    {
        if (keyDownJump && isGround)
            rb.velocity = new Vector2(rb.velocity.x, 10);
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
        anim.SetTrigger("Dead");
        Debug.Log("죽었습니다.");
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(footPos.position, boxSize);
    }

    public void JumpMushroom()
    {
        rb.velocity = new Vector2(rb.velocity.x, 40);
    }

}
