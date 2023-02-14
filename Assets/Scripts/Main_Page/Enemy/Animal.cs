using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : Enemy
{

    float uninfectionSpeed = 3f;
    float infectionSpeed = 6f;
    float speed;
    bool mad;
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
            case 1:
                anim.SetBool("Walk", true);
                gameObject.transform.localScale = new Vector3(-1, 1, 1);
                break;
            case -1:
                anim.SetBool("Walk", true);
                gameObject.transform.localScale = new Vector3(1, 1, 1);
                break;
        }
    }
    void FixedUpdate()
    {
        if(!mad)
            rb.velocity = new Vector2(dirX*speed, rb.velocity.y);
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
        {
            anim.SetBool("Find", true);
            Vector3 dir = (collision.transform.position - gameObject.transform.position).normalized;
            DashToPlayer(dir);
        }
    }

    public void DashToPlayer(Vector3 dir)
    {
        CancelInvoke("Think");
        dir.y = 0;
        mad = true;
        rb.velocity = dir * speed * 2;
        Debug.Log(dir.x);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && isInfection)
            StartCoroutine("CoolDown");
    }
    IEnumerator CoolDown()
    {
        yield return new WaitForSeconds(3f);
        anim.SetBool("Find", false);
        Think();
        mad = false;
        yield break; 
    }
}
