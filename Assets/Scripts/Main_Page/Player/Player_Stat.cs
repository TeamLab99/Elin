using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Stat : MonoBehaviour
{
    public static Player_Stat instance;
    public Text[] textState;
    // 현재 능력치

    private float hp=100;
    private float mp =100;
    public float atk =5;
    public float def =0;

    public float currentHp=50; 
    public float currentMp=50;
    public float maxHp = 110;
    public float maxMp = 110;


    public void TakeDamage()
    {
        hp -= 10;
        mp -= 10;
    }

    public void Heal(float heal)
    {
        if (heal + currentHp > maxHp)
            currentHp = maxHp;
        else
            currentHp += heal;
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
}
