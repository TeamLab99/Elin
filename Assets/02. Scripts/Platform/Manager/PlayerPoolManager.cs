using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPoolManager : Singleton<PlayerPoolManager>
{
    // 프리팹들을 보관할 변수
    public GameObject[] projectilePrefabs;
    public GameObject[] alterEgoPrefabs;
    public GameObject[] monsterAttackPrefabs;

    List<GameObject>[] projectilePools;
    List<GameObject>[] alterEgoPools;
    List<GameObject>[] monsterAttackPools;

    private void Awake()
    {
        projectilePools = new List<GameObject>[projectilePrefabs.Length];
        alterEgoPools = new List<GameObject>[alterEgoPrefabs.Length];
        monsterAttackPools = new List<GameObject>[monsterAttackPrefabs.Length];
        for (int i = 0; i < projectilePools.Length; i++)
        {
            projectilePools[i] = new List<GameObject>();

        }
        for (int i = 0; i < alterEgoPools.Length; i++)
        {
            alterEgoPools[i] = new List<GameObject>();
        }
        for (int i = 0; i < monsterAttackPools.Length; i++)
        {
            monsterAttackPools[i] = new List<GameObject>();
        }
    }

    public GameObject GetProjectile(int idx)
    {
        GameObject select = null;
        for(int i=0; i< projectilePools[idx].Count; i++)
        {
            if (!projectilePools[idx][i].activeSelf)
            {
                projectilePools[idx][i].SetActive(true);
                return projectilePools[idx][i];
            }
        }
        if (!select) // select가 여전히 null인 상태라면 새로 생성해야 한다.
        {
            select = Instantiate(projectilePrefabs[idx], transform); // 새롭게 생성하고 select 변수에 할당시킨다.
            projectilePools[idx].Add(select); // 리스트에 추가시켜준다.
        }
        return select;
    }
    public GameObject GetAlterEgo(int idx)
    {
        GameObject select = null;
        for (int i = 0; i < alterEgoPools[idx].Count; i++)
        {
            if (!alterEgoPools[idx][i].activeSelf)
            {
                alterEgoPools[idx][i].SetActive(true);
                return alterEgoPools[idx][i];
            }
        }
        if (!select) // select가 여전히 null인 상태라면 새로 생성해야 한다.
        {
            select = Instantiate(alterEgoPrefabs[idx], transform); // 새롭게 생성하고 select 변수에 할당시킨다.
            alterEgoPools[idx].Add(select); // 리스트에 추가시켜준다.
        }
        return select;
    }

    public GameObject GetMonsterAttack(int idx)
    {
        GameObject select = null;
        for (int i = 0; i < monsterAttackPools[idx].Count; i++)
        {
            if (!monsterAttackPools[idx][i].activeSelf)
            {
                monsterAttackPools[idx][i].SetActive(true);
                return monsterAttackPools[idx][i];
            }
        }
        if (!select) // select가 여전히 null인 상태라면 새로 생성해야 한다.
        {
            select = Instantiate(monsterAttackPrefabs[idx], transform); // 새롭게 생성하고 select 변수에 할당시킨다.
            monsterAttackPools[idx].Add(select); // 리스트에 추가시켜준다.
        }
        return select;
    }
}
