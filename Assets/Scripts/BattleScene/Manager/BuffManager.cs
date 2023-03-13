using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuffManager : MonoBehaviour
{
    public static BuffManager Inst { get; private set; } // 싱글턴
    void Awake() => Inst = this;

    public GameObject[] effects;
    public TMP_Text[] timeTxt;
    public Sprite[] sprites;
    public int[] isActive;

    bool isAvoid;
    int avoidNum;
    int defenseNum;

    public float maxTime=3f;
    static float curTime;

    private void Start()
    {
        curTime = maxTime;
    }

    private void Update()
    {
        if (isAvoid)
        {
            curTime -= Time.deltaTime;

            timeTxt[avoidNum].text = ((int)curTime).ToString();

            if (curTime <= 0)
            {
                // 회피 효과 끝
            }
        }
    }
}
