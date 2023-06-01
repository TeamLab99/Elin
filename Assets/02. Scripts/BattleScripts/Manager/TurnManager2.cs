using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class TurnManager2 : Singleton<TurnManager2>
{
    [SerializeField] [Tooltip("시작 카드 개수를 정합니다.")] int startCardCount = 5;

    WaitForSeconds delay025 = new WaitForSeconds(0.25f);
    WaitForSeconds delay05 = new WaitForSeconds(0.5f);

    public static Action OnAddCard;

    public IEnumerator StartGameCo()
    {
        yield return delay05;
        for (int i = 0; i < startCardCount; i++)
        {
            OnAddCard?.Invoke();
            yield return delay025;
        }
    }
}
