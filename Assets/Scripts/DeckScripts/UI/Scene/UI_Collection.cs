using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UI_Collection : UI_Scene
{
    int cardNum;
    GameObject content;
    Define.Sorting sorting; // 내가 원하는 기준으로 정렬하기 위한 함수
    Dictionary<int, DeckCard> _dict;  //전체 카드를 담을 딕셔너리

    enum GameObjects
    {
        Content
    }

    //카드를 얻는 시점에서 싱글톤으로 사용할 예정
    public static UI_Collection Collection { get; private set; }

    private void Awake()
    {
        Collection = this;
    }

    void Start()
    {
        Init();
        cardNum = 0;
        sorting = Define.Sorting.Default;
    }

    public override void Init()
    {
        base.Init();

        //이 위치에 전체 카드 데이터를 담을 딕셔너리를 선언
        _dict = Managers.Data.CardDict;

        Bind<GameObject>(typeof(GameObjects));
        content = Get<GameObject>((int)GameObjects.Content);

        drawCard();
    }

    public void drawCard()
    {
        cardNum = 0;
        Dictionary<int, UnlockCard> _deck = Managers.Data.DeckDict;

        foreach (Transform child in content.transform)
            Managers.Resource.Destroy(child.gameObject);

        switch(sorting)
        {
            case Define.Sorting.Default:
                foreach (KeyValuePair<int, DeckCard> cardinfo in _dict)
                {
                    /*
                    GameObject card = Managers.UI.MakeCard<UI_Card>(content.transform, "UI_Card").gameObject;
                    UI_Card cardInDeck = card.GetOrAddComponent<UI_Card>();
                    cardInDeck.SetInfo(cardinfo.Value);
                    cardNum++;
                    */

                    foreach (KeyValuePair<int, UnlockCard> deckinfo in _deck)
                    {
                        if (cardinfo.Value.id == deckinfo.Value.id)
                        {
                            
                            GameObject card = Managers.UI.MakeCard<UI_Card>(content.transform, "UI_Card").gameObject;
                            UI_Card cardInDeck = card.GetOrAddComponent<UI_Card>();
                            cardInDeck.SetInfo(cardinfo.Value);
                            cardNum++;
                            
                        }
                    }
                    
                }
                break;

            case Define.Sorting.Fire:
                foreach (KeyValuePair<int, DeckCard> cardinfo in _dict)
                {
                    /*
                    if (cardinfo.Value.element == "fire")
                    {
                        GameObject card = Managers.UI.MakeCard<UI_Card>(content.transform, "UI_Card").gameObject;
                        UI_Card cardInDeck = card.GetOrAddComponent<UI_Card>();
                        cardInDeck.SetInfo(cardinfo.Value);
                        cardNum++;
                    }
                    */
                    
                    foreach (KeyValuePair<int, UnlockCard> deckinfo in _deck)
                    {
                        if (cardinfo.Value.id == deckinfo.Value.id)
                        {
                            if (cardinfo.Value.element == "fire")
                            {
                                GameObject card = Managers.UI.MakeCard<UI_Card>(content.transform, "UI_Card").gameObject;
                                UI_Card cardInDeck = card.GetOrAddComponent<UI_Card>();
                                cardInDeck.SetInfo(cardinfo.Value);
                                cardNum++;
                            }
                        }
                    }
                    
                }
                break;

            case Define.Sorting.Water:
                foreach (KeyValuePair<int, DeckCard> cardinfo in _dict)
                {
                    /*
                    if (cardinfo.Value.element == "water")
                    {
                        GameObject card = Managers.UI.MakeCard<UI_Card>(content.transform, "UI_Card").gameObject;
                        UI_Card cardInDeck = card.GetOrAddComponent<UI_Card>();
                        cardInDeck.SetInfo(cardinfo.Value);
                        cardNum++;
                    }
                    */
                    
                    foreach (KeyValuePair<int, UnlockCard> deckinfo in _deck)
                    {
                        if (cardinfo.Value.id == deckinfo.Value.id)
                        {
                            if (cardinfo.Value.element == "water")
                            {
                                GameObject card = Managers.UI.MakeCard<UI_Card>(content.transform, "UI_Card").gameObject;
                                UI_Card cardInDeck = card.GetOrAddComponent<UI_Card>();
                                cardInDeck.SetInfo(cardinfo.Value);
                                cardNum++;
                            }
                        }
                    }
                    
                }
                break;

            case Define.Sorting.Wind:
                foreach (KeyValuePair<int, DeckCard> cardinfo in _dict)
                {
                    /*
                    if (cardinfo.Value.element == "wind")
                    {
                        GameObject card = Managers.UI.MakeCard<UI_Card>(content.transform, "UI_Card").gameObject;
                        UI_Card cardInDeck = card.GetOrAddComponent<UI_Card>();
                        cardInDeck.SetInfo(cardinfo.Value);
                        cardNum++;
                    }
                    */
                    
                    foreach (KeyValuePair<int, UnlockCard> deckinfo in _deck)
                    {
                        if (cardinfo.Value.id == deckinfo.Value.id)
                        {
                            if (cardinfo.Value.element == "wind")
                            {
                                GameObject card = Managers.UI.MakeCard<UI_Card>(content.transform, "UI_Card").gameObject;
                                UI_Card cardInDeck = card.GetOrAddComponent<UI_Card>();
                                cardInDeck.SetInfo(cardinfo.Value);
                                cardNum++;
                            }
                        }
                    }
                    
                }
                break;

            case Define.Sorting.Earth:
                foreach (KeyValuePair<int, DeckCard> cardinfo in _dict)
                {
                    /*
                    if (cardinfo.Value.element == "earth")
                    {
                        GameObject card = Managers.UI.MakeCard<UI_Card>(content.transform, "UI_Card").gameObject;
                        UI_Card cardInDeck = card.GetOrAddComponent<UI_Card>();
                        cardInDeck.SetInfo(cardinfo.Value);
                        cardNum++;
                    }
                    */
                    
                    foreach (KeyValuePair<int, UnlockCard> deckinfo in _deck)
                    {
                        if (cardinfo.Value.id == deckinfo.Value.id)
                        {
                            if (cardinfo.Value.element == "earth")
                            {
                                GameObject card = Managers.UI.MakeCard<UI_Card>(content.transform, "UI_Card").gameObject;
                                UI_Card cardInDeck = card.GetOrAddComponent<UI_Card>();
                                cardInDeck.SetInfo(cardinfo.Value);
                                cardNum++;
                            }
                        }
                    }
                    
                }
                break;


            default:
                Debug.Log("sorting 오류");
                break;

        }

        content.GetComponent<RectTransform>().sizeDelta = new Vector2(content.GetComponent<RectTransform>().rect.width, 356 * (int)Math.Ceiling((double)cardNum / 4));
    }

    #region SetSorting
    public void SetFire()
    {
        sorting = Define.Sorting.Fire;
        drawCard();
    }
    public void SetWater()
    {
        sorting = Define.Sorting.Water;
        drawCard();
    }
    public void SetWind()
    {
        sorting = Define.Sorting.Wind;
        drawCard();
    }
    public void SetEarth()
    {
        sorting = Define.Sorting.Earth;
        drawCard();
    }
    public void SetDefault()
    {
        //Managers.Data.getNewCard(10);
        sorting = Define.Sorting.Default;
        drawCard();
    }
    #endregion
}
