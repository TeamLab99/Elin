using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : MonoBehaviour
{
    //private bool isLive = true; // 생존 여부
    private float speed; // 속도
    private float defaultSpeed = 2f; // 기본 스피드
    private float dirX; // 몬스터의 방향 
    private bool isInfection; // 감염 여부
    private int enemyId; // 10번대 동물 , 20번대 식물, 30번대 하이리
    private int enemyType; // 1 : 동물, 2 : 식물, 3: 하이리
    

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
 
    void Think()
    {
        dirX = Random.Range(-1, 2);
        Invoke("Think", 3f);
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

    void CheckRay()
    {
        Vector2 wallCheck = new Vector2(rb.position.x, rb.position.y);
        Debug.DrawRay(wallCheck, Vector3.right * dirX, new Color(1, 0, 0));
        RaycastHit2D wallRayHit = Physics2D.Raycast(wallCheck, Vector3.right * dirX, 1, wallLayer);

        Vector2 groundCheck = new Vector2(rb.position.x + dirX, rb.position.y);
        Debug.DrawRay(groundCheck, Vector3.down, new Color(0, 1, 0));
        RaycastHit2D groundRayHit = Physics2D.Raycast(groundCheck, Vector3.down, 1, groundLayer);

        if (wallRayHit.collider && enemyType != 2)
        {
            // dirX *= -1;
        }
        if (groundRayHit.collider == null && enemyType != 2)
        {
            CancelInvoke("Think");
            ReThink();
        }
    }
    private void FixedUpdate()
    {
        CheckRay();
        rb.velocity = new Vector2(dirX * speed, rb.velocity.y);
   
    }

    void OnEnable()
    {
        //isLive = true;
    }


    void ReThink()
    {
        dirX *= -1; 
        Invoke("Think",Random.Range(2,5));
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && isInfection)
        {
            Vector3 playerPos = collision.transform.position;
            CancelInvoke("Think");
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
            Invoke("Think", 3f);
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
        enemyId = data.enemyID;
        enemyType = enemyId/10;
        defaultSpeed = speed;
    }
}
