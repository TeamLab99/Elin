using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public List<DeckCard> Cards = new List<DeckCard>();

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