using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float inputXDir;
    Rigidbody2D rb;
    bool isRight = true;

    public float moveXSpeed = 10f;
    public float jumpForce = 10f;

    [Header("Ground Check")]
    [SerializeField] Transform groundPos;
    public Vector2 groundSize;
    public LayerMask groundLayer;
    public bool isGround;

    [Header("Wall Check")]
    [SerializeField] Transform wallPos;
    public float wallDist;
    public bool isWall;
    public LayerMask wallLayer;
    public bool isWallSlide;
    public float wallSlideSpeed;
    public float moveForceInAir;
    public float airDragMultiplier = 0.95f;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        inputXDir = Input.GetAxisRaw("Horizontal");
        if(isRight && inputXDir < 0)
        {
            Flip();
        }
        else if(!isRight && inputXDir > 0)
        {
            Flip();
        }
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            Jump();
        }
        
        CheckWallSlide();
    }

    private void FixedUpdate()
    {
        if (isGround)
            rb.velocity = new Vector2(inputXDir * moveXSpeed, rb.velocity.y);
        else if (!isGround && !isWallSlide && inputXDir != 0)
        {
            Vector2 forceToAdd = new Vector2(moveForceInAir * inputXDir, 0);
            rb.AddForce(forceToAdd);
            if (Mathf.Abs(rb.velocity.x) > moveXSpeed)
            {
                rb.velocity = new Vector2(moveXSpeed * inputXDir, rb.velocity.y);
            }
        }else if(!isGround && !isWallSlide && inputXDir == 0)
        {
            rb.velocity = new Vector2(rb.velocity.x * airDragMultiplier, rb.velocity.y);
        }
            
            
        if (isWallSlide)
        {
            if (rb.velocity.y < -wallSlideSpeed)
            {
                rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed);
            }
        }

        CheckSurround();
    }

    void Flip()
    {
        if (!isWallSlide)
        {
            isRight = !isRight;
            transform.Rotate(0.0f, 180.0f, 0.0f);

        }
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    bool CheckGround()
    {
        if (Physics2D.OverlapBox(groundPos.position, groundSize, 0f, groundLayer))
            return true;
        else
            return false;
    }

    void CheckSurround()
    {
        isGround = CheckGround();
        isWall = Physics2D.Raycast(wallPos.position, transform.right, wallDist, wallLayer);
        
    }

    void CheckWallSlide()
    {
        if (isWall && !isGround && rb.velocity.y < 0)
            isWallSlide = true;
        else
            isWallSlide = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(groundPos.position, groundSize);
        Gizmos.DrawLine(wallPos.position, new Vector3(wallPos.position.x + wallDist, wallPos.position.y, wallPos.position.z));
    }
}
