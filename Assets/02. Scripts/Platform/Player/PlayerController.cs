using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    public Animator anim;

    [Header("땅 검출")]
    private bool isGrounded;
    [SerializeField] float groundCheckDistance;
    [SerializeField] LayerMask groundLayer;
    
    [Header("움직임")]
    private float xInput;
    private bool facingRight = true;
    [SerializeField] private float xSpeed; 
    [SerializeField] private float ySpeed;
    [SerializeField] private ParticleSystem playerHitParticle;
 
    [Header("Control Player")]
    private bool playerDead = false;
    public bool canControll { get; set; } = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
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
        if (isGrounded)
            rb.velocity = new Vector2(rb.velocity.x, 10);
    }

    private void CollisionChecks()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, groundLayer);
    }
    private void PlayerAnimation()
    {
        bool isMoving = rb.velocity.x != 0; //isMoving = (rb.velocity.x != 0) ? true : false;
        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("isMoving", isMoving);
        anim.SetBool("isGrounded", isGrounded);
    }

    private void FlipPlayer()
    {
        if (rb.velocity.x > 0 && !facingRight)
            Flip();
        else if (rb.velocity.x < 0 && facingRight)
            Flip();
    }
    void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }

    public void ControlPlayer(bool _canMove) // PlayerMoveAction 이벤트의 함수
    {
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
            //anim.SetTrigger("Dead");
            ControlPlayer(false);
             playerDead = true;
        }
    }

    public void ShowDeadUI()
    {
        PlayerRespawnManager.instance.ShowGameOverUI();
    }

    public void Respawn(Transform _respawnPosition)
    {
        anim.SetTrigger("Respawn");
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
