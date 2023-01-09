using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// json에서 받아올 데이터의 형식과 딕셔너리를 만드는 함수를 정의함
// 새로운 타입의 데이터를 불러오기 한다고 하면 그냥 새로운 클래스를 여기에 추가하면 됨
// ex ) 아이템, 게임 데이터

#region Card
[Serializable]
public class DeckCard
{
    //serializedField 넣으면 public 없어도 가능, 변수의 이름과 형식이 파일과 일치해야함
    public int id;
    public string cardName;
    public string element;
    public int cost;
    public string description;
}

[Serializable]
public class CardData : ILoader<int, DeckCard>
{
    // 받아온 정보를 모두 리스트에 저장하는데 이 리스트의 이름은 json에 적힌 이름과 같아야 한다
    public List<DeckCard> Cards = new List<DeckCard>();

    //json으로부터 받아온 정보를 기반으로 딕셔너리를 구성한다
    public Dictionary<int, DeckCard> MakeDict()
    {
        Dictionary<int, DeckCard> dict = new Dictionary<int, DeckCard>();
        foreach (DeckCard card in Cards)
        {
            dict.Add(card.id, card);         
        }
        return dict;
    }
}
#endregion

//덱에 대한 클래스
#region Deck
[Serializable]
public class UnlockCard
{
    public int id;
}

public class DeckData : ILoader<int, UnlockCard>
{
    public List<UnlockCard> deckCards = new List<UnlockCard>();

    public Dictionary<int, UnlockCard> MakeDict()
    {
        Dictionary<int, UnlockCard> dict = new Dictionary<int, UnlockCard>();
        foreach (UnlockCard card in deckCards)
        {
            dict.Add(card.id, card);
        }
        return dict;
    }
}
#endregion

//덱을 세이브 할 때 사용할 클래스
#region DeckSaveData
[Serializable]
public class DeckSaveData
{
    public UnlockCard[] deckCards;
}
#endregion