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
    }

    void LateUpdate()
    {
        if (!isLive)
            return;
        spr.flipX = rb.velocity.x < 0;
    }
    void Think()
    {
        next = Random.Range(-1, 2);
        rb.velocity = new Vector2(next, 0);
        Invoke("Think", 3f);
    }

    void OnEnable()
    {
        isLive = true;
        health = maxHealth;
    }

    public void Init(SpawnData data)
    {
        anim.runtimeAnimatorController = animCon[data.spriteType];
        speed = data.speed;
        maxHealth = data.health;
        health = data.health;

    }

    private void OnTriggerEnter2D(Collider2D collision) // 트리거 충돌
    {
        if (collision.CompareTag("Player"))
        {
            gameObject.SetActive(false);
        }
    }
}
