using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy_Move : MonoBehaviour
{
    Rigidbody2D rb;
    Animator anim;
    SpriteRenderer spr;
    public int nextMove;
    public GameObject exclamation;

    private bool findPlayer;
    public float speed;
    public float distance;
    public Transform player;
    
    
    private void Awake()
    {
        spr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        Think(); 
    }
    private void Update()
    {
    }
    private void FixedUpdate()
    {
        if (!findPlayer)
        {
            rb.velocity = new Vector2(nextMove, rb.velocity.y);
        } // 플레이어를 발견하지 못했을 때
        else
        {
            rb.velocity = new Vector2(0, 0);
            TowardPlayer();
            FlipDir();
        } // 플레이어를 발견했을 때
        // Vector2 frontVec = new Vector2(rb.position.x + nextMove, rb.velocity.y);
        //RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Platform"));
        //  if (rayHit.collider != null)
        //    nextMove *= -1;
    }

   void Think() 
    {
        nextMove = Random.Range(-1, 2);
        Invoke("Think", 5f);
    } // 인공지능 

    void TowardPlayer()
    {
        if (Mathf.Abs(transform.position.x - player.position.x) > distance && findPlayer)
        {
            transform.Translate(new Vector2(-1, 0) * Time.deltaTime * speed);
        }
    } // 플레이어를 향해 움직이는 것 구현
    
    void FlipDir()
    {
        if (transform.position.x - player.position.x < 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
    } // 적 스프라이트 반대로 뒤집기

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            exclamation.SetActive(true);
            findPlayer = true;
        }
    } // 플레이어 검출
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            exclamation.SetActive(false);
            findPlayer = false;
        }
    } // 플레이어가 범위 밖으로 나갔을 시
}
