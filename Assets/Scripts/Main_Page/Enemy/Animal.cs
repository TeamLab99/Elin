using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : Enemy
{
    public Transform checkTf;
    public LayerMask checkLayer;

    bool isFind = false;
    bool isGround = false;

    float checkDist = 1f;
    float speed = 0f;
    float uninfectionSpeed = 3f;
    float infectionSpeed = 6f;

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
                gameObject.transform.localScale = new Vector3(dirX, 1, 1);
                break;
        }
       
        isGround = Physics2D.Raycast(checkTf.position + Vector3.right*dirX, Vector2.down, checkDist, checkLayer); // 전방에 낭떠러지가 있는지 확인 

        if (!isGround)
        {
            CancelInvoke("Think");
            dirX *= -1;
            Invoke("Think", Random.Range(0,4));
        }
           
    }

    void FixedUpdate()
    {
        if(!isFind)
            rb.velocity = new Vector2(dirX*speed, rb.velocity.y);
        else
        {
            float k = (target.transform.position.x - gameObject.transform.position.x);
            if (isAggressive)
            {
                if (k > 0)
                    dirX = 1;
                else if (k < 0)
                    dirX = -1;
            }
            else
            {
                if (k > 0)
                    dirX = -1;
                else if (k < 0)
                    dirX = 1;
            }
            rb.velocity = Vector2.right * dirX * 3;
        }
    }

   
    protected override void Think()
    {
        dirX = Random.Range(-1, 2);
        Invoke("Think", 6f);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && isInfection)
        {
            isFind = true;
            target = collision.gameObject;
            CancelInvoke("Think");
            StopCoroutine("CoolDown");
            anim.SetBool("Find", true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && isInfection)
            StartCoroutine("CoolDown");
    }
    IEnumerator CoolDown()
    {
        isFind = false;
        yield return new WaitForSeconds(3f);
        anim.SetBool("Find", false);
        Think();
        yield break; 
    }
}
