using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//카드들을 담고 있는 카드 콜렉션 창에 대한 스크립트이다.
public class UI_Collection : UI_Base
{
    int cardNum;            //내가 가진 카드 숫자
    GameObject content;     //카드들의 UI의 부모
    Sorting sorting; // 내가 원하는 기준으로 정렬하기 위한 함수
    Dictionary<int, DeckCard> _dict;  //전체 카드를 담을 딕셔너리

    //enum으로 바인딩 하기 위해 선언
    enum GameObjects
    {
        Content
    }

    //카드를 얻는 시점에서 싱글톤으로 사용할 예정 / 필요가 없다면 삭제
    public static UI_Collection Collection { get; private set; }

    private void Awake()
    {
        Collection = this;
    }

    void Start()
    {
        Init();
    }

    public override void Init()
    {
        //이 위치에 전체 카드 데이터를 담을 딕셔너리를 선언
        _dict = Managers.Data.CardDict;

        //정렬은 기본값으로
        sorting = Sorting.Default;

        //바인딩
        Bind<GameObject>(typeof(GameObjects));
        content = Get<GameObject>((int)GameObjects.Content);

        //카드를 그린다
        drawCard();
    }

    public void drawCard()
    {
        //호출될 때 CardNum을 0으로 해주는 이유는 카드 수가 가변적이기 때문이다.
        //이전과 현재의 카드의 수가 다를 수 있음
        //이런 문제 때문에 카드를 얻으면 drawCard를 새로 호출해야함
        cardNum = 0;

        //내 덱에 대한 딕셔너리는 호출될 때마다 새로 받아옴
        //위에서 언급한 가변성과 관련 있음
        Dictionary<int, UnlockCard> _deck = Managers.Data.DeckDict;

        content.TryGetComponent<RectTransform>(out RectTransform rectTransform);

        //drawCard가 호출될 때마다 기존에 존재하던 UI들을 다 파괴하고 다시 그리기 때문에 이와 같이 코딩함
        //이게 비효율적인지는 모르겠음, 노트북에서도 렉이 걸리진 않았기 때문
        Transform child = null;
        int childCount = content.transform.childCount;

        //foreach문 대신 for문을 사용하기 위해 딕셔너리의 항목 수와
        //키를 저장하는 배열을 만들어서 이것을 통해 foreach문의 역할을 수행함
        //for문이 더 빠르다 해서 바꿨는데 검색 때문에 별 차이가 없을까 걱정임
        int cardCount = _dict.Count;
        int deckCount = _deck.Count;

        int[] cardKeys = new int[cardCount];
        int[] deckKeys = new int[deckCount];

        _dict.Keys.CopyTo(cardKeys, 0);
        _deck.Keys.CopyTo(deckKeys, 0);

        //자식들을 전부 풀 루트로 보냄
        for (int i= 0; i < childCount; i++)
        {
            child = content.transform.GetChild(0);
            Managers.Resource.Destroy(child.gameObject);
        }
           

        //sorting에 따라 조금씩 다른 작업을 함
        //기본값은 전체 카드를 다 보여주고
        //불, 물, 바람, 흙은 해당하는 속성의 카드만 보여줌
        //또한 카드를 한 장 생성하면 cardNum을 1씩 더하고 마지막 카드까지 그려지면 총 카드 수가 CardNum이 되는데
        //이를 이용해서 content의 크기를 조절함
        switch(sorting)
        {
            case Sorting.Default:
                for(int i=0; i<cardCount; i++)
                {
                    DeckCard cardinfo = _dict[cardKeys[i]];
                    for(int j=0; j<deckCount; j++)
                    {
                        UnlockCard deckinfo = _deck[deckKeys[j]];
                        if (cardinfo.id == deckinfo.id)
                        {
                            GameObject card = Managers.UI.MakeCard<UI_Card>(rectTransform, "UI_Card").gameObject;
                            card.TryGetComponent<UI_Card>(out UI_Card cardInDeck);
                            cardInDeck.SetInfo(cardinfo);
                            cardNum++;
                        }
                    }
                }
                break;

            case Sorting.Fire:
                for (int i = 0; i < cardCount; i++)
                {
                    DeckCard cardinfo = _dict[cardKeys[i]];
                    for (int j = 0; j < deckCount; j++)
                    {
                        UnlockCard deckinfo = _deck[deckKeys[j]];
                        if (cardinfo.id == deckinfo.id)
                        {
                            if (cardinfo.element == "fire")
                            {
                                GameObject card = Managers.UI.MakeCard<UI_Card>(rectTransform, "UI_Card").gameObject;
                                UI_Card cardInDeck = card.GetOrAddComponent<UI_Card>();
                                cardInDeck.SetInfo(cardinfo);
                                cardNum++;
                            }
                        }
                    }
                }
                break;

            case Sorting.Water:
                for (int i = 0; i < cardCount; i++)
                {
                    DeckCard cardinfo = _dict[cardKeys[i]];
                    for (int j = 0; j < deckCount; j++)
                    {
                        UnlockCard deckinfo = _deck[deckKeys[j]];
                        if (cardinfo.id == deckinfo.id)
                        {
                            if (cardinfo.element == "water")
                            {
                                GameObject card = Managers.UI.MakeCard<UI_Card>(rectTransform, "UI_Card").gameObject;
                                UI_Card cardInDeck = card.GetOrAddComponent<UI_Card>();
                                cardInDeck.SetInfo(cardinfo);
                                cardNum++;
                            }
                        }
                    }
                }
                break;

            case Sorting.Wind:
                for (int i = 0; i < cardCount; i++)
                {
                    DeckCard cardinfo = _dict[cardKeys[i]];
                    for (int j = 0; j < deckCount; j++)
                    {
                        UnlockCard deckinfo = _deck[deckKeys[j]];
                        if (cardinfo.id == deckinfo.id)
                        {
                            if (cardinfo.element == "wind")
                            {
                                GameObject card = Managers.UI.MakeCard<UI_Card>(rectTransform, "UI_Card").gameObject;
                                UI_Card cardInDeck = card.GetOrAddComponent<UI_Card>();
                                cardInDeck.SetInfo(cardinfo);
                                cardNum++;
                            }
                        }
                    }
                }
                break;

            case Sorting.Earth:
                for (int i = 0; i < cardCount; i++)
                {
                    DeckCard cardinfo = _dict[cardKeys[i]];
                    for (int j = 0; j < deckCount; j++)
                    {
                        UnlockCard deckinfo = _deck[deckKeys[j]];
                        if (cardinfo.id == deckinfo.id)
                        {
                            if (cardinfo.element == "earth")
                            {
                                GameObject card = Managers.UI.MakeCard<UI_Card>(rectTransform, "UI_Card").gameObject;
                                UI_Card cardInDeck = card.GetOrAddComponent<UI_Card>();
                                cardInDeck.SetInfo(cardinfo);
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

        //content의 세로 크기를 카드 수에 따라 조절함
        //카드가 가로로 4장까지 정렬되므로 다음과 같은 수식으 적용
        content.GetComponent<RectTransform>().sizeDelta = new Vector2(content.GetComponent<RectTransform>().rect.width, 356 * (int)Math.Ceiling((double)cardNum / 4));
    }

    //이 부분은 버튼을 누르면 sorting 변수의 값을 바꾸고 새로 그리는 부분
    #region SetSorting
    public void SetFire()
    {
        sorting = Sorting.Fire;
        drawCard();
    }
    public void SetWater()
    {
        sorting = Sorting.Water;
        drawCard();
    }
    public void SetWind()
    {
        sorting = Sorting.Wind;
        drawCard();
    }
    public void SetEarth()
    {
        sorting = Sorting.Earth;
        drawCard();
    }
    public void SetDefault()
    {
        //이 부분은 카드를 새로 추가해본 부분으로 잘 되었다.
        //Managers.Data.getNewCard(10);
        //Managers.Resource.saveData();
        sorting = Sorting.Default;
        drawCard();
    }
    #endregion
}
