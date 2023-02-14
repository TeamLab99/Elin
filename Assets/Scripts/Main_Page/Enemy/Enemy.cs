using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public bool isLive = true;
    public float speed = 3f;
    public float dirX;
    public bool isInfection;
    public int enemyId;
    public int enemyType;

    private float defaultSpeed;

    public LayerMask groundLayer; // 땅 레이어
    public LayerMask wallLayer; // 벽 레이어

    Rigidbody2D rb;
    SpriteRenderer spr;
    Animator anim;

    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        spr = gameObject.GetComponent<SpriteRenderer>();
        anim = gameObject.GetComponent<Animator>();
    }
 
    void AIThink()
    {
        dirX = Random.Range(-1, 2);
        Invoke("AIThink", 5f);
    }

    void Update()
    {
        if (enemyType !=2) // 동물, 하이리
        {
            switch (dirX)
            {
                case -1:
                    gameObject.transform.localScale = new Vector3(1,1,1);
                    anim.SetBool("Walk", true);
                    break;
                case 0:
                    anim.SetBool("Walk", false);
                    break;
                case 1:
                    gameObject.transform.localScale = new Vector3(-1, 1, 1);
                    anim.SetBool("Walk", true);
                    break;
            }
        }

    }
    private void FixedUpdate()
    {
        Vector2 wallCheck = new Vector2(rb.position.x, rb.position.y);
        Debug.DrawRay(wallCheck, Vector3.right*dirX, new Color(1, 0, 0));
        RaycastHit2D wallRayHit = Physics2D.Raycast(wallCheck, Vector3.right*dirX, 1, wallLayer);

        Vector2 groundCheck = new Vector2(rb.position.x + dirX, rb.position.y);
        Debug.DrawRay(groundCheck, Vector3.down, new Color(0, 1, 0));
        RaycastHit2D groundRayHit = Physics2D.Raycast(groundCheck, Vector3.down, 1, groundLayer);

        rb.velocity = new Vector2(dirX * speed, rb.velocity.y);
        if (wallRayHit.collider && enemyType!=2)
        {
           // dirX *= -1;
        }
        if (groundRayHit.collider == null && enemyType != 2)
        {
            CancelInvoke("AIThink");
            ReThink();
        }
    }

    void OnEnable()
    {
        isLive = true;
    }


    void ReThink()
    {
        dirX *= -1; 
        Invoke("AIThink",Random.Range(2,5));
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && isInfection)
        {
            Vector3 playerPos = collision.transform.position;
            CancelInvoke("AIThink");
            if (playerPos.x - transform.position.x > 0)
            {
                dirX = 1;
            }
            else
            {
                dirX = -1;
            }
            speed = defaultSpeed * 2;
            anim.SetBool("Find", true);
            Invoke("AIThink", 3f);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && isInfection)
        {
            anim.SetBool("Find", false);
            speed = defaultSpeed;
        }
            
    }

    public void Init(SpawnData data)
    {
        speed = data.speed;
        isInfection = data.isInfection;
        enemyId = data.enemyId;
        enemyType = enemyId/10;
        defaultSpeed = speed;
        if (enemyType != 2)
            AIThink();
    }
}
