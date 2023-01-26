using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{

    public static PlayerStat instance;
    public GameObject expText;
    public GameObject parent;
    public int characterLevel; 
    public int[] needExp;
    public int currentExp;
    public int hp;
    public int currentHp;
    public int deffence;
    
    void Start()
    {
        instance = this;
    }

   public void Hit(int enemyAttack) // 캐릭터 피격 시 호출
    {
        int damage;
        if (deffence >= enemyAttack)
            damage = 1;
        else
            damage = enemyAttack - deffence;
        currentHp -= damage;
        if (currentHp <= 0)
            Debug.Log("게임 오버!");
        Vector3 vector = this.transform.position;
        vector.y += 60;
        GameObject clone = Instantiate(expText, vector, Quaternion.Euler(Vector3.zero));
        //clone.GetComponent<FloatingText>
    }
    void Update()
    {
        
    }
}
