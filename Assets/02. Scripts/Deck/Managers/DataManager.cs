using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


//인터페이스를 정의한 것
public interface ILoader<Key, Value>
{
    Dictionary<Key, Value> MakeDict();
}

public class DataManager
{
    // 캐싱의 개념으로 DataManager가 카드, 덱에 대한 데이터를 딕셔너리 형태로 들고 있음
    // 아이템도 이렇게 가능
    // 새로운 카드를 얻는다면 그 클래스는 UnlockCard로 해서 DeckDict에 id를 키로 해서 Add해주면 됨
    public Dictionary<int, DeckCard> CardDict { get; private set; } = new Dictionary<int, DeckCard>();
    public Dictionary<int, UnlockCard> DeckDict { get; private set; } = new Dictionary<int, UnlockCard>();
    
    //착용한 보석에 대해 가지고 있는 변수
    public EGems equipGem = EGems.none;

    public void Init()
    {
        //Resources의 Data 폴더 산하에서 해당하는 이름에 맞는 json 데이터를 읽어온다.
        CardDict = LoadJson<CardData, int, DeckCard>("CardData").MakeDict();
        DeckDict = LoadJson<DeckData, int, UnlockCard>("DeckData").MakeDict();
    }

    Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        //json파일은 텍스트 형태로 읽어온다.
        TextAsset textAsset = Managers.Resource.Load<TextAsset>($"Data/{path}");
        return JsonUtility.FromJson<Loader>(textAsset.text);
    }

    //카드를 새로 얻으면 DeckDict에 추가하는 함수
    public void getNewCard(int _id)
    {
        UnlockCard _card = new UnlockCard();
        _card.index = _id;

        //내가 들고 있는 덱 딕셔너리에 추가하는 함수
        DeckDict.Add(_id, _card);
    }
}
