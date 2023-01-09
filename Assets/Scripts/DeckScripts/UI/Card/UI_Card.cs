using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

//카드에 대한 정보를 담은 스크립트이다
public class UI_Card : UI_Base
{
    //카드에는 이름, 코스트, 설명이 텍스트로 들어간다.
    enum Texts
    {
        NameText,
        CostText,
        Description
    }

    string _name;
    string _cost;
    string _description;
    public override void Init()
    {
        //텍스트매쉬프로의 형태로 각각 텍스트를 바인딩해준다.
        Bind<TextMeshProUGUI>(typeof(Texts));

        GetText((int)Texts.NameText).text = _name;
        GetText((int)Texts.CostText).text = _cost;
        GetText((int)Texts.Description).text = _description;
    }

    void Start()
    {
        Init();
    }

    //카드의 정보를 수정해주는 함수
    public void SetInfo(DeckCard deckCard)
    {
        _name = deckCard.cardName;
        _cost = deckCard.cost.ToString();
        _description = deckCard.description;
    }

}
