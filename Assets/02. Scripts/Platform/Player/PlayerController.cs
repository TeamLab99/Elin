using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer spr;
    public Animator anim;

    [Header("땅 검출")]
    private bool grounded;
    [SerializeField] float groundCheckDistance;
    [SerializeField] LayerMask groundLayer;
    
    [Header("움직임")]
    private float xInput;
    private bool facingRight=true;
    public bool FacingRight { get { return facingRight; } }
    [SerializeField] private float xSpeed; 
    [SerializeField] private float ySpeed;
    [SerializeField] private ParticleSystem playerHitParticle;
 
    [Header("Control Player")]
    private bool playerDead = false;
    public bool canControll { get; set; } = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        Managers.Input.PlayerMoveAction -= ControlPlayer;
        Managers.Input.PlayerMoveAction += ControlPlayer;
    }

    private void Update()
    {
        if (canControll)
        {
            MoveKeyInput();
            Movement();
        }
        CollisionChecks();
        PlayerAnimation();
        FlipPlayer();
    }

    private void MoveKeyInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        if (Input.GetKeyDown(KeyCode.Space))
            Jump();
    }
    private void Movement()
    {
        rb.velocity = new Vector2(xInput * xSpeed, rb.velocity.y);
    }
    private void Jump()
    {
        if (grounded)
            rb.velocity = new Vector2(rb.velocity.x, 10);
    }

    private void CollisionChecks()
    {
        grounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, groundLayer);
    }
    private void PlayerAnimation()
    {
        bool walk = Mathf.Abs(rb.velocity.x) >= 0.1f; //walk = (rb.velocity.x != 0) ? true : false;
        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("walk", walk);
        anim.SetBool("grounded", grounded);
    }

    private void FlipPlayer()
    {
        if (rb.velocity.x > 0.1f)
            spr.flipX = true;
        else if (rb.velocity.x < -0.1f)
            spr.flipX = false;
        CheckDir();
    }
    void CheckDir()
    {
        if (spr.flipX)
            facingRight = true;
        else
            facingRight = false;
    }

    public void ControlPlayer(bool _canMove) // PlayerMoveAction 이벤트의 함수
    {
        anim.CrossFade("Idle", 0f);
        canControll = _canMove;
        
        if (!canControll)
        {
            xInput = 0;
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }

    public void Hit()
    {
        playerHitParticle.Play();
    }

    public void Dead()
    {
        if (!playerDead)
        {
            anim.SetTrigger("dead");
            ControlPlayer(false);
            playerDead = true;
        }
    }

    public void Respawn(Transform _respawnPosition)
    {
        anim.SetTrigger("live");
        canControll = true;
        playerDead = false;
        gameObject.transform.position = _respawnPosition.position;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - groundCheckDistance));
    }
}
