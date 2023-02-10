using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Stat : MonoBehaviour
{
    public static Player_Stat instance;
    public Text textHp;
    // 현재 능력치
    private int hp=50;
    /*private int mp=50;
    private int attack=5;
    private int dffence=0;

    public int maxHp; 
    public int maxMp;
    */
    public void TakeDamage()
    {
        hp -= 10;
        Debug.Log(hp);
        textHp.text = hp.ToString();

    }
    void Start()
    {
        instance = this;
    }
    void Update()
    {
        
    }
}
