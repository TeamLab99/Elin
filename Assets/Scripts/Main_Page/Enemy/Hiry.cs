using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hiry : Enemy
{
    float uninfectionSpeed=3f;
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
        rb.velocity = new Vector2(dirX*speed, rb.velocity.y);
    }


    protected override void Think()
    {
        dirX = Random.Range(-1, 2);
        Invoke("Think", 5f);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            target = collision.gameObject;
            StartCoroutine("DeBuff");
            // isDeBuff 변수를 줘서 한번만 걸리도록 수정할 수도 있다.
            // 일단은 즉발 디버프로 설정했다.
        }
    }
    IEnumerator DeBuff()
    {
        DataBase_Manager.instance.pm.xSpeed = 5;
        yield return new WaitForSeconds(3f);
        DataBase_Manager.instance.pm.xSpeed = 10;
        yield break;
    }
   
}
