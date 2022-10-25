using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface ILoader<Key, Value>
{
    Dictionary<Key, Value> MakeDict();
}

public class DataManager
{
    //������ Ű�� ������ ���� ������ ���߿��� ID�� �����Ͽ� �� �Ϲ����̰� ������ ã���� �� �ִ�.
    public Dictionary<int, DeckCard> StatDict { get; private set; } = new Dictionary<int, DeckCard>();

    public void Init()
    {
        StatDict = LoadJson<CardData, int, DeckCard>("StatData2").MakeDict();
    }

    Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        TextAsset textAsset = Managers.Resource.Load<TextAsset>($"Data/{path}");
        return JsonUtility.FromJson<Loader>(textAsset.text);
    }
}
