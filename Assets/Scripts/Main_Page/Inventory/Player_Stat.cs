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
}
