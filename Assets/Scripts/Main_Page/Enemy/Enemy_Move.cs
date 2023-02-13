using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy_Move : MonoBehaviour
{
    public float next;
    public float speed;
    public float health;
    public float maxHealth;
    public RuntimeAnimatorController[] animCon;
    private bool isLive = false;
    public Rigidbody2D target;

    Rigidbody2D rb;
    SpriteRenderer spr;
    Animator anim;
    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        spr = gameObject.GetComponent<SpriteRenderer>();
        anim = gameObject.GetComponent<Animator>();
        Think();
    }
        
    void FixedUpdate()
    {
        if (!isLive)
            return;
        Vector2 frontVec = new Vector2(rb.position.x + next * 2, rb.position.y);
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Ground"));
        if (rayHit)
        {
            next *= -1;
            //CancelInvoke("Think");
        }
        Vector2 dirVec = target.position - rb.position;
        Vector2 nextVec = dirVec.normalized * 3 * Time.fixedDeltaTime;
        //rb.MovePosition(rb.position + nextVec);
        rb.velocity = new Vector2(nextVec.x, 0f);
        //rb.velocity = Vector2.zero;
    }

    void LateUpdate()
    {
        if (!isLive)
            return;
        spr.flipX = rb.velocity.x < 0;
    }

    void Think()
    {
        /*next = Random.Range(-1, 2);
        rb.velocity = new Vector2(next, 0);
        Invoke("Think", 3f);*/
    
    }

    void OnEnable()
    {
        isLive = true;
        health = maxHealth;
        target = DataBase_Manager.instance.pm.GetComponent<Rigidbody2D>();
    }

    public void Init(SpawnData data)
    {
        /*anim.runtimeAnimatorController = animCon[data.spriteType];
        speed = data.speed;
        maxHealth = data.health;
        health = data.health;
        Debug.Log(data.enemyId);*/
    }

    private void OnTriggerEnter2D(Collider2D collision) // 트리거 충돌
    {
        if (collision.CompareTag("Player"))
        {
            rb.velocity *= 2;
        }
    }
}
