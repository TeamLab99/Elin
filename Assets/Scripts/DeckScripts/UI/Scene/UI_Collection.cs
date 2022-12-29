using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UI_Collection : UI_Scene
{
    int cardNum;
    GameObject content;
    //Dictionary<int, DeckCard> _dict;  //전체 카드를 담을 딕셔너리

    enum GameObjects
    {
        Content
    }

    void Start()
    {
        Init();
        cardNum = 0;
    }

    public override void Init()
    {
        base.Init();

        //이 위치에 전체 카드 데이터를 담을 딕셔너리를 선언
        //_dict = Managers.Data.CardDict;

        Bind<GameObject>(typeof(GameObjects));
        content = Get<GameObject>((int)GameObjects.Content);
        drawCard(content);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void drawCard(GameObject go)
    {
        //이 딕셔너리는 전체 카드에 대한 딕셔너리가 아니라 현재 가지고 있는 카드에 대한 딕셔너리 (혹은 리스트)로 대체
        Dictionary<int, DeckCard> _dict = Managers.Data.CardDict;

        foreach (Transform child in go.transform)
            Managers.Resource.Destroy(child.gameObject);

        foreach (KeyValuePair<int, DeckCard> cardinfo in _dict)
        {
            GameObject card = Managers.UI.MakeCard<UI_Card>(go.transform, "UI_Card").gameObject;
            UI_Card cardInDeck = card.GetOrAddComponent<UI_Card>();
            cardInDeck.SetInfo(cardinfo.Value);
            cardNum++;
        }

        go.GetComponent<RectTransform>().sizeDelta = new Vector2(go.GetComponent<RectTransform>().rect.width, 356 * (int)Math.Ceiling((double)cardNum / 4));
    }
}
