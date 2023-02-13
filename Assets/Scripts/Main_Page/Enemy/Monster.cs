using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public bool isLive = true;
    public float speed = 3f;
    public float dirX;
    public bool isInfection;
    public int enemyId;
    public int enemyType;
    
    Rigidbody2D rb;
    SpriteRenderer spr;
    Animator anim;

    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        spr = gameObject.GetComponent<SpriteRenderer>();
        anim = gameObject.GetComponent<Animator>();
        if(enemyType!=2)
            AIThink();
    }
 
    void AIThink()
    {
        dirX = Random.Range(-1, 2);
        rb.velocity = Vector2.right * dirX * speed;
        Invoke("AIThink", 5f);
    }
    void Update()
    {
        if (enemyType !=2) // 동물, 하이리
        {
            switch (dirX)
            {
                case -1:
                    spr.flipX = false;
                    anim.SetBool("Walk", true);
                    break;
                case 0:
                    anim.SetBool("Walk", false);
                    break;
                case 1:
                    spr.flipX = true;
                    anim.SetBool("Walk", true);
                    break;
            }
        }
    }
    void OnEnable()
    {
        isLive = true;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && isInfection)
        {
            speed *= 2;
            anim.SetBool("Run", true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && isInfection)
            anim.SetBool("Run", false);
    }

    public void Init(SpawnData data)
    {
        speed = data.speed;
        isInfection = data.isInfection;
        enemyId = data.enemyId;
        enemyType = enemyId/10;
    }
}
