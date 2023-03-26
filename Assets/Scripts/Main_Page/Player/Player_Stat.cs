using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Stat : MonoBehaviour
{
    public static Player_Stat instance;
    public Text[] textState;
    // 현재 능력치
    private int hp=100;
    private int mp=100;
    public int atk=5;
    public int def=0;

    public int currentHp=50; 
    public int currentMp=50;
    
    public void TakeDamage()
    {
        hp -= 10;
        mp -= 10;
    }
    void Start()
    {
        instance = this;
    }
    void Update()
    {
        textState[0].text = currentHp.ToString();
        textState[1].text = currentMp.ToString();
        textState[2].text = atk.ToString();
        textState[3].text = def.ToString();
    }

    /*
      private bool isHit; // 데미지를 받았는지 확인하는 변수
         void OnDamaged(Vector2 targetPos)
    {
        isHit = true;
        gameObject.layer = 12;
        spr.color = new Color(1, 1, 1,0.4f);
        StartCoroutine("CoolDownSpike");
    }
    IEnumerator CoolDownSpike()
    {
        yield return new WaitForSeconds(1f);
        spr.color = new Color(1, 1, 1, 1f);
        isHit = false;
        gameObject.layer = 3;
        yield break;
    }
      private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Spike")
        {
            OnDamaged(collision.transform.position);
        }
    } // 충돌
     */
}
