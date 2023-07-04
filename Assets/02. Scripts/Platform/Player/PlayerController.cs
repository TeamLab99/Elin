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
    [SerializeField] Vector2 boxSize;
    private bool isGround;
  
    [Header("움직임")]
    [SerializeField] private float xSpeed; 
    [SerializeField] private float ySpeed;
    [SerializeField] private float jumpGravity = 1.5f;
    [SerializeField] private float fallGravity = 2.5f;
    [SerializeField] private ParticleSystem playerHitParticle;

    private float moveDir;
    private float jumpTime  = 0f;
    private float chargeTime = 0.2f;
    private bool isJump = false;
    private Vector2 hitForce = new Vector2(0, 8);

    [Header("Control Player")]
    private bool canMove = true;
    private bool playerDead = false;
    private bool keyDownJump;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
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
        PlayerAnimation();
    }

    private void MoveKeyInput()
    {
        moveDir = Input.GetAxisRaw("Horizontal");
        keyDownJump = Input.GetKeyDown(KeyCode.Space);
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
        {
            isGround = true;
            SetJumpGravity();
        }
            
        else
            isGround = false;
    }

    private void FixedUpdate()
    {
        Walk();
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

    public void Hit()
    {
        rb.AddForce(hitForce, ForceMode2D.Impulse);
        playerHitParticle.Play();
    }

    public void Dead()
    {
        if (!playerDead)
        {
            anim.SetTrigger("Dead");
            ControlPlayer(false);
             playerDead = true;
        }
    }

    public void Respawn(Transform _respawnPosition)
    {
        anim.SetTrigger("Respawn");
        canMove = true;
        playerDead = false;
        gameObject.transform.position = _respawnPosition.position;
    }

    public void JumpMushroom()
    {
        rb.velocity = new Vector2(rb.velocity.x, 40);
    }

    public void SetJumpGravity()
    {
        rb.gravityScale = jumpGravity;
    }
    public void SetFallGravity()
    {
        rb.gravityScale = fallGravity;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(footPos.position, boxSize);
    }
}
