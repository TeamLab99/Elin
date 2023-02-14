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

    protected override void Find()
    {
        Debug.Log("주변에 디버프 현상이 생깁니다.");
    }

    protected override void Think()
    {
        dirX = Random.Range(-1, 2);
        Invoke("Think", 5f);
    }
}
