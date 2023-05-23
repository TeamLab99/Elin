using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

//카드에 대한 정보를 담은 스크립트이다
public class UI_Card : UI_Base,IPointerClickHandler
{
    //카드에는 이름, 코스트, 설명이 텍스트로 들어간다.
    enum Texts
    {
        NameText,
        CostText,
        Description
    }

    public TextMeshProUGUI _name;
    public TextMeshProUGUI _cost;
    public TextMeshProUGUI _description;

    public void Awake()
    {
        Bind<TextMeshProUGUI>(typeof(Texts));

        _name = GetText((int)Texts.NameText);
        _cost = GetText((int)Texts.CostText);
        _description = GetText((int)Texts.Description);
    }

    public override void Init()
    {
        
    }

    void Start()
    {
        Init();
    }

    //카드의 정보를 수정해주는 함수
    public void SetInfo(DeckCard deckCard)
    {
        _name.text = deckCard.cardName;
        _cost.text = deckCard.cost.ToString();
        _description.text = deckCard.description;  
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("올렸음");
    }
}
