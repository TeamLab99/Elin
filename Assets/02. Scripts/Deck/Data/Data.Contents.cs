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
    public int index;
    public string cardName;
    public string element;
    public string type;
    public int cost;
    public float percent;
    public string attackCode;
    public string buffCode;
    public string debuffCode;
    public float attackProbability;
    public float attackPercent;
    public float buffProbability;
    public float buffMaintainTime;
    public float debuffProbability;
    public float debuffMaintainTime;
    public int amount;
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
            dict.Add(card.index, card);         
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
    public int index;
}

public class DeckData : ILoader<int, UnlockCard>
{
    public List<UnlockCard> deckCards = new List<UnlockCard>();

    public Dictionary<int, UnlockCard> MakeDict()
    {
        Dictionary<int, UnlockCard> dict = new Dictionary<int, UnlockCard>();
        foreach (UnlockCard card in deckCards)
        {
            dict.Add(card.index, card);
        }
        return dict;
    }
}
#endregion

//전체 아이템에 대한 정보를 받아옴
#region Item
[Serializable]
public class Items
{
    public int id;
    public string itemName;
    public string itemDescription;
    public int itemCount;
    public int itemSpriteId;
    //enum을 그냥 0, 1, 2, 3 이렇게 해도 되지만 직관적이지 않다.
    //enum을 string으로 바꿔주는 converter가 있는 거 같은데 사용법은 잘 모르겠다.
    //public Define.ItemType itemType;
}

public class ItemData : ILoader<int, Items>
{
    public List<Items> items = new List<Items>();

    public Dictionary<int, Items> MakeDict()
    {
        Dictionary<int, Items> dict = new Dictionary<int, Items>();
        foreach (Items item in items)
        {
            dict.Add(item.id, item);
        }
        return dict;
    }
}
#endregion

//캐릭터의 인벤 정보를 받아오는 내용
#region InvenItem
[Serializable]
public class InvenItem
{
    public int id;
}

public class InvenData : ILoader<int, InvenItem>
{
    public List<InvenItem> inven = new List<InvenItem>();

    public Dictionary<int, InvenItem> MakeDict()
    {
        Dictionary<int, InvenItem> dict = new Dictionary<int, InvenItem>();
        foreach (InvenItem item in inven)
        {
            dict.Add(item.id, item);
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