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
    //지금은 키로 레벨을 물고 있지만 나중에는 ID로 관리하여 더 일반적이게 빠르게 찾아줄 수 있다.
    public Dictionary<int, DeckCard> CardDict { get; private set; } = new Dictionary<int, DeckCard>();
    public Dictionary<int, UnlockCard> DeckDict { get; private set; } = new Dictionary<int, UnlockCard>();

    public void Init()
    {
        CardDict = LoadJson<CardData, int, DeckCard>("CardData").MakeDict();
        DeckDict = LoadJson<DeckData, int, UnlockCard>("DeckData").MakeDict();
    }

    Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        TextAsset textAsset = Managers.Resource.Load<TextAsset>($"Data/{path}");
        return JsonUtility.FromJson<Loader>(textAsset.text);
    }
}
