using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuffManager : MonoBehaviour
{
    #region 선언

    public static BuffManager Inst { get; private set; } // 싱글턴
    void Awake() => Inst = this;

    public GameObject[] effects;
    public TMP_Text[] timeTxt;
    public TMP_Text[] amountTxt;
    public Sprite[] sprites;
    public int[] isActive;

    bool isAvoid;
    bool isDefense;
    int avoidNum;
    int defense;

    public float maxTime = 3f;
    static float curTime;

    private void Update()
    {
        if (isDefense)
        {
            amountTxt[1].text = defense.ToString();
        }
    }

    #endregion

    public void DefenseOn()
    {
        isDefense = true;
        effects[1].SetActive(true);
        amountTxt[1].gameObject.SetActive(true);
        defense += 5;
    }

    public void DefenseOff()
    {
        isDefense = false;
        effects[1].SetActive(false);
        amountTxt[1].gameObject.SetActive(false);
        defense = 0;
    }

    public bool GetisDefense()
    {
        return isDefense;
    }

    #region 회피
    /// <summary>
    /// 업데이트 문 대신 씀, 처음에 Start 때 curTime을 초기화 시켜주지만
    /// 이후 사용 할 때는 maxTime으로 초기화를 따로 해줘야 실행 가능
    /// 회피 카드를 연속 사용해서 사용시간을 갱신할 때는 curTime = maxTime 만 해주면 됨
    /// </summary>
    /// <returns></returns>
    IEnumerator AvoidTimer()
    {
        while (true)
        {
            curTime -= Time.deltaTime;
            timeTxt[avoidNum].text = ((int)curTime).ToString();
            //Debug.Log((int)curTime);

            if (curTime <= 0)
            {
                curTime = 0;
                AvoidOff();
                //회피 효과 끝
                break;
            }
            yield return null;
        }
    }

    public void AvoidOn()
    {
        isAvoid = true;
        curTime = maxTime;
        effects[0].SetActive(true);
        timeTxt[0].gameObject.SetActive(true);
        StartCoroutine(AvoidTimer());
    }

    public void AvoidOff()
    {
        effects[0].SetActive(false);
        timeTxt[0].gameObject.SetActive(false);
        isAvoid = false;
    }

    public void AvoidTimeUpdate()
    {
        curTime = maxTime;
    }

    public bool GetisAvoid()
    {
        return isAvoid;
    } 
    #endregion
}
