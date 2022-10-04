using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region Card

[Serializable]
public class DeckCard
{
    //serializedField 넣으면 public 없어도 가능, 변수의 이름과 형식이 파일과 일치해야함
    public int level;
    public int hp;
    public int attack;
}

[Serializable]
public class CardData : ILoader<int, DeckCard>
{
    public List<DeckCard> cards = new List<DeckCard>();

    public Dictionary<int, DeckCard> MakeDict()
    {
        Dictionary<int, DeckCard> dict = new Dictionary<int, DeckCard>();
        foreach (DeckCard card in cards)
            dict.Add(card.level, card);
        return dict;
    }
}

#endregion