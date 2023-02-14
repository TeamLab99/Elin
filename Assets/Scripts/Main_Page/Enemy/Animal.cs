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
            StartCoroutine("Behavior");
    }
    IEnumerator Behavior()
    {
        yield return new WaitForSeconds(3f);
        anim.SetBool("Find", false);
        yield break; 
    }
}
