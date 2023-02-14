using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Enemy : MonoBehaviour
{
    // 공통 변수는 Enemy 스크립트에서 묶어서 관리
    protected int dirX;
    protected int enemyID;
    protected int enemyType;
    protected bool isInfection;

    protected Rigidbody2D rb;
    protected SpriteRenderer spr;
    protected Animator anim;
    protected void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        spr = gameObject.GetComponent<SpriteRenderer>();
        anim = gameObject.GetComponent<Animator>();
        Think();
    }

    public void Init(SpawnData data)
    {
        enemyID = data.enemyID;
        enemyType = enemyID / 10;
        isInfection = data.isInfection;
    }

    //protected abstract void Dead();

    // 자식들이 같은 기능을 가지고 있다면 부모 클래스에서 추상적으로 선언한다.
    // 부모클래스에선 추상클래스는 아무것도 선언되어 있지 않고, 자식 클래스에선 무조건 재선언 되어야 한다.
    // 추상 클래스를 하나라도 선언하면 abstract를 붙이고, 이를 재선언 한다면 override를 붙여서 선언해줘야 한다.
    protected abstract void Find();
    protected abstract void Think();

}
