using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region Card

[Serializable]
public class DeckCard
{
    //serializedField ������ public ��� ����, ������ �̸��� ������ ���ϰ� ��ġ�ؾ���
    public int id;
    public string cardName;
    public int cost;
    public string description;
}

[Serializable]
public class CardData : ILoader<int, DeckCard>
{
    public List<DeckCard> deckCards = new List<DeckCard>();

    public Dictionary<int, DeckCard> MakeDict()
    {
        Dictionary<int, DeckCard> dict = new Dictionary<int, DeckCard>();
        foreach (DeckCard card in deckCards)
        {
            dict.Add(card.id, card);         
        }
        return dict;
    }
}

#endregion