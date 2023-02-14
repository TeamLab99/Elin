using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : Enemy
{

    float uninfectionSpeed = 3f;
    float infectionSpeed = 6f;
    float speed;

    void Start()
    {
        if (isInfection)
            speed = infectionSpeed;
        else
            speed = uninfectionSpeed;
    }

    void Update()
    {
        switch (dirX)
        {
            case 0:
                anim.SetBool("Walk", false);
                break;
            default:
                anim.SetBool("Walk", true);
                break;
        }    
    }
    void FixedUpdate()
    {
        rb.velocity = new Vector2(dirX*speed, 0);
    }

    protected override void Find()
    {
        Debug.Log("플레이어쪽으로 돌진합니다.");
    }
    protected override void Think()
    {
        dirX = Random.Range(-1, 2);
        Invoke("Think", 6f);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && isInfection)
            anim.SetBool("Find", true);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && isInfection)
            anim.SetBool("Find", false);
    }
}
