using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool_Manager : MonoBehaviour
{
    // 프리팹들을 보관할 변수
    public GameObject[] prefabs;
    // 풀 담당을 하는 리스트들
    List<GameObject>[] pools;

    void Awake()
    {
        pools = new List<GameObject>[prefabs.Length];
        for(int i=0; i<pools.Length; i++)
        {
            pools[i] = new List<GameObject>();
        }
    }

    public GameObject Get(int idx)
    {
        GameObject select = null;
        // 선택한 풀의 비활성화 된 게임 오브젝트에 접근한다.
        for(int i=0; i<pools.Length; i++)
        {
            foreach (GameObject item in pools[i])
            {
                if (!item.activeSelf)
                {
                    select = item;
                    select.SetActive(true);
                    return select;
                }
            }
        }

        if (!select) // select가 여전히 null인 상태라면 새로 생성해야 한다.
        {
            select = Instantiate(prefabs[idx], transform); // 새롭게 생성하고 select 변수에 할당시킨다.
            pools[idx].Add(select); // 리스트에 추가시켜준다.
        }
        return select;
    }
}
