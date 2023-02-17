using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool_Manager : MonoBehaviour
{
    // 프리팹들을 보관할 변수
    public GameObject[] monPrefabs; // 몬스터 프리팹들
    public GameObject[] extraPrefabs; // 파티클(이펙트), 함정들을 담는 리스트
    // 풀 담당을 하는 리스트들
    List<GameObject>[] monPools; // 몬스터 프리팹(게임오브젝트)들을 담는 리스트
    List<GameObject>[] extraPools; // 파티클이나 함정을 담는 리스트 (상자는 담을지 말지 고민중)

    private void Awake()
    {
        monPools = new List<GameObject>[monPrefabs.Length];
        extraPools = new List<GameObject>[extraPrefabs.Length];
        for (int i = 0; i < monPools.Length; i++)
        {
            monPools[i] = new List<GameObject>();

        }
        for (int i = 0; i < extraPools.Length; i++)
        {
            extraPools[i] = new List<GameObject>();
        }
    }

    public GameObject Get(int idx)
    {
        GameObject select = null;
        for(int i=0; i<monPools[idx].Count; i++)
        {
            if (!monPools[idx][i].activeSelf)
            {
                monPools[idx][i].SetActive(true);
                return monPools[idx][i];
            }
        }
        if (!select) // select가 여전히 null인 상태라면 새로 생성해야 한다.
        {
            select = Instantiate(monPrefabs[idx], transform); // 새롭게 생성하고 select 변수에 할당시킨다.
            monPools[idx].Add(select); // 리스트에 추가시켜준다.
        }
        return select;
    }

    public GameObject GetExtra(int idx)
    {
        GameObject select = null;
        for (int i = 0; i < extraPools[idx].Count; i++)
        {
            if (!extraPools[idx][i].activeSelf)
            {
                extraPools[idx][i].SetActive(true);
                return extraPools[idx][i];
            }
        }
        if (!select) // select가 여전히 null인 상태라면 새로 생성해야 한다.
        {
            select = Instantiate(extraPrefabs[idx], transform); // 새롭게 생성하고 select 변수에 할당시킨다.
            extraPools[idx].Add(select); // 리스트에 추가시켜준다.
        }
        return select;
    }
}
