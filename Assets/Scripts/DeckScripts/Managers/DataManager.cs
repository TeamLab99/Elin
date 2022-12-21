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
    public Dictionary<int, DeckCard> CardDict { get; private set; } = new Dictionary<int, DeckCard>();

    public void Init()
    {
        CardDict = LoadJson<CardData, int, DeckCard>("CardData").MakeDict();
    }

    Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        TextAsset textAsset = Managers.Resource.Load<TextAsset>($"Data/{path}");
        return JsonUtility.FromJson<Loader>(textAsset.text);
    }
}
