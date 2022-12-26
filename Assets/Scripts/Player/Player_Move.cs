using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Move : MonoBehaviour
{
    Animator anim;
    Rigidbody2D rb;
    SpriteRenderer spr;
    [SerializeField]
    public Transform groundChk;
    public Transform wallChk;
    public Transform jumpBoxChk;
    public LayerMask groundLayer;
    public LayerMask wallLayer;
    public LayerMask jumpBoxLayer;
    public float groundDist;
    public float wallDist;
    public float jumpBoxDist;
    public float jumpPower;
    public float walkSpeed;
    public GameObject box;

    float defaultGravity; //�⺻ �߷�
    float moveDir; //Horizontal
    float jumpDir; //Jump
    float isRight; //1:������, -1:����
    bool isGround; //���� �پ��ִ°�?
    bool isWall; //���� �پ��ִ°�?
    bool isJumpBox; //���� �ڽ��� �ؿ� �ִ°�?
    bool isWallJump; //��
    bool boxOpen; 
    bool ladderCollide; //��ٸ��� �پ��ִ°�?
    bool isLadder; //��ٸ��� �޴޷��ִ°�?
    bool isLadderJump; //��ٸ����� �����ϴ°�?

    private void Awake()
    {
      
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spr = GetComponent<SpriteRenderer>();
    }

    void Start() //�ʱ� ���� ����
    {
        isRight = 1;
        boxOpen = false;
        isLadder = false;
        defaultGravity = rb.gravityScale;
    }

    void Update()
    {
        jumpDir = Input.GetAxis("Jump");
        isGround = Physics2D.Raycast(groundChk.position, Vector2.down, groundDist, groundLayer);
        isWall = Physics2D.Raycast(wallChk.position, Vector2.right*isRight, wallDist, wallLayer);
        isJumpBox = Physics2D.Raycast(jumpBoxChk.position, Vector2.down, jumpBoxDist, jumpBoxLayer);
        if (!isWallJump){
            moveDir = Input.GetAxisRaw("Horizontal");
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                isRight = -1;
                FlipX();
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                isRight = 1;
                FlipX();
            }
            
            
        } //�¿� ��ȯ + �¿� ������

        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
                isLadder = true;
            if (Input.GetKeyDown(KeyCode.X))
                if (boxOpen)
                    box.SetActive(false);
        } // Ű �Է�

        {
            if (Mathf.Abs(moveDir) > 0.1f)
                anim.SetBool("isRun", true);
            else
                anim.SetBool("isRun", false);
            if (rb.velocity.y > 0.1f)
                anim.SetBool("isFall", false);
            else 
                anim.SetBool("isFall", true);
            if (isGround)
                anim.SetBool("isGround", true);
            else
                anim.SetBool("isGround", false);
            if (isWall)
                anim.SetBool("isWall", true);
            else
                anim.SetBool("isWall", false);
        } //�ִϸ��̼� ǥ��
    }
    void FixedUpdate()
    {
        if (!isWallJump)
        {
            Walk();
            Jump();
            WallSlide();
        }
        if(!isLadderJump)
            OnLadder();
    }
    private void OnLadder()
    {
        if (ladderCollide)
        {
            isLadderJump = false;
            if (isLadder)
            {
                HangingLadder();
                if (Input.GetAxis("Jump") != 0)
                {
                    isLadderJump = true;
                    Invoke("FreezLadderJump", 0.3f);
                    rb.velocity = new Vector2(isRight * walkSpeed, jumpDir * jumpPower);
                }
            }
        }
        else
        {
            rb.gravityScale = defaultGravity;
        }
    }
    private void WallSlide()
    {
        if (isWall)
        {
            isWallJump = false;
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.6f);
            if (Input.GetAxis("Jump")!=0)
            {
                isWallJump = true;
                Invoke("FreezX", 0.3f);
                isRight *= -1;
                FlipX();
                rb.velocity = new Vector2(walkSpeed * isRight, jumpPower * 0.8f);
            }
        }
    } //��Ÿ�� + ������
    private void Walk()
    {   
        if(isGround)
            rb.velocity = new Vector2(moveDir * walkSpeed, 0);
        else
            rb.velocity = new Vector2(moveDir * walkSpeed, rb.velocity.y);
    } //�ȱ�
    private void Jump()
    {
        if (isGround)
            rb.velocity = new Vector2(rb.velocity.x, jumpDir * jumpPower);
    } //����
    private void FlipX()
    {
        if (isRight == -1)
            spr.flipX = true;
        else if(isRight== 1)
            spr.flipX = false;
    } //�¿���ȯ
    private void FreezX()
    {
        isWallJump = false;
        FlipX();
    } //������ ���� Ǯ��
    private void FreezLadderJump()
    {
        isLadderJump = false;
    }
    private void HangingLadder()
    {
        float ver = Input.GetAxis("Vertical");
        rb.velocity = new Vector2(0, ver * jumpPower);
        rb.gravityScale = 0f;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(groundChk.position,Vector2.down*groundDist);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(wallChk.position, Vector2.right * wallDist*isRight);
    } //���⼱

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "JumpBox" &&isJumpBox)
            rb.AddForce(new Vector2(0, 20), ForceMode2D.Impulse);
        if (collision.gameObject.tag == "Spike")
            rb.AddForce(new Vector2(10*isRight, 5), ForceMode2D.Impulse);  
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
            ladderCollide = true;
        if (collision.CompareTag("Box"))
            boxOpen = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            ladderCollide = false;
            isLadder = false;
        }
        if (collision.CompareTag("Box"))
            boxOpen = false;
    }
}
