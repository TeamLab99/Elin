using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hiry : Enemy
{
    float uninfectionSpeed=3f;
    float infectionSpeed = 6f;
    float speed=0f;

    // 벽과 땅을 검출
    bool isFind = false;
    bool putDeBuff = false;
    public Transform checkTf;
    public LayerMask checkLayer;
    float checkDist = 1f;
    bool isGround = false;

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
        isGround = Physics2D.Raycast(checkTf.position + Vector3.right * dirX, Vector2.down, checkDist, checkLayer); // 전방에 낭떠러지가 있는지 확인 

        if (!isGround)
        {
            CancelInvoke("Think");
            dirX *= -1;
            Invoke("Think", Random.Range(0, 4));
        }
    }

    void FixedUpdate()
    {
        if (!isFind)
            rb.velocity = new Vector2(dirX * speed, rb.velocity.y);
        else
        {
            float k = (target.transform.position.x - gameObject.transform.position.x);
            if (!isAggressive)
            {
                if (k > 0)
                    dirX = -1;
                else if (k < 0)
                    dirX = 1;
            }
            else
            {
                CancelInvoke("Think");
                dirX = 0;
            }
            rb.velocity = Vector2.right * dirX * 3;
        }
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
            isFind = true;
            target = collision.gameObject;
            anim.SetBool("Find", true);
            // isDeBuff 변수를 줘서 한번만 걸리도록 수정할 수도 있다.
            // 일단은 즉발 디버프로 설정했다.
            if (!putDeBuff)
            {
                SpawnObstruction();
                Invoke("ChargeDeBuff",1f);
            }
            putDeBuff = true;
        }
    }
    void ChargeDeBuff()
    {
        putDeBuff = false;
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            anim.SetBool("Find", false);
            Invoke("CoolDown", 2f);
            // isDeBuff 변수를 줘서 한번만 걸리도록 수정할 수도 있다.
            // 일단은 즉발 디버프로 설정했다.
        }
    }
    void SpawnObstruction()
    {
        GameObject deBuff = DataBase_Manager.instance.pool.GetExtra(1); // 디버프
        deBuff.transform.position = target.transform.position+Vector3.right*3;        
        StartCoroutine("Disapear",deBuff);
    }

    IEnumerator Disapear(GameObject go)
    {
        yield return new WaitForSeconds(1f);
        go.SetActive(false);
        yield break;
    }

    void CoolDown()
    {
        isFind = false;
        Invoke("Think",1f);
    }
}
