using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//카드들을 담고 있는 카드 콜렉션 창에 대한 스크립트이다.
public class UI_Collection : UI_Base
{
    int cardNum;            //내가 가진 카드 숫자
    GameObject content;     //카드들의 UI의 부모
    Define.Sorting sorting; // 내가 원하는 기준으로 정렬하기 위한 함수
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
        sorting = Define.Sorting.Default;

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

        //drawCard가 호출될 때마다 기존에 존재하던 UI들을 다 파괴하고 다시 그리기 때문에 이와 같이 코딩함
        //이게 비효율적인지는 모르겠음, 노트북에서도 렉이 걸리진 않았기 때문
        foreach (Transform child in content.transform)
            Managers.Resource.Destroy(child.gameObject);

        //sorting에 따라 조금씩 다른 작업을 함
        //기본값은 전체 카드를 다 보여주고
        //불, 물, 바람, 흙은 해당하는 속성의 카드만 보여줌
        //또한 카드를 한 장 생성하면 cardNum을 1씩 더하고 마지막 카드까지 그려지면 총 카드 수가 CardNum이 되는데
        //이를 이용해서 content의 크기를 조절함
        switch(sorting)
        {
            case Define.Sorting.Default:
                foreach (KeyValuePair<int, DeckCard> cardinfo in _dict)
                {                 
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

        //content의 세로 크기를 카드 수에 따라 조절함
        //카드가 가로로 4장까지 정렬되므로 다음과 같은 수식으 적용
        content.GetComponent<RectTransform>().sizeDelta = new Vector2(content.GetComponent<RectTransform>().rect.width, 356 * (int)Math.Ceiling((double)cardNum / 4));
    }

    //이 부분은 버튼을 누르면 sorting 변수의 값을 바꾸고 새로 그리는 부분
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
        //이 부분은 카드를 새로 추가해본 부분으로 잘 되었다.
        Managers.Data.getNewCard(10);
        Managers.Resource.saveData();
        sorting = Define.Sorting.Default;
        drawCard();
    }
    #endregion
}
