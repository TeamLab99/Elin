using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager2 : Singleton<CardManager2>
{
    Dictionary<int, DeckCard> deckCards;
    Dictionary<int, UnlockCard> unlockCards;

    List<DeckCard> shuffleCards;

    void SetupItemBuffer()
    {
        shuffleCards = new List<DeckCard>();
        deckCards = Managers.Data.CardDict;
        unlockCards = Managers.Data.DeckDict;

        foreach (KeyValuePair<int, DeckCard> item in deckCards)
        {
            foreach (KeyValuePair<int, UnlockCard> deck in unlockCards)
            {
                if (item.Key == deck.Value.index)
                {
                    for (int i = 0; i < item.Value.percent; i++)
                    {
                        shuffleCards.Add(item.Value);
                    }
                }
            }
        }

        for (int i = 0; i < shuffleCards.Count; i++)
        {
            int rand = Random.Range(i, shuffleCards.Count);
            DeckCard temp = shuffleCards[i];
            shuffleCards[i] = shuffleCards[rand];
            shuffleCards[rand] = temp;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        SetupItemBuffer();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
