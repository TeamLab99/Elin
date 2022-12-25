using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class UI_Card : UI_Base
{
    TextMeshProUGUI nameText;
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
        Bind<TextMeshProUGUI>(typeof(Texts));
        //Bind<Text>(typeof(Texts));

        GetText((int)Texts.NameText).text = _name;
        GetText((int)Texts.CostText).text = _cost;
        GetText((int)Texts.Description).text = _description;
    }

   
    void Start()
    {
        Init();
    }

    public void SetInfo(DeckCard deckCard)
    {
        _name = deckCard.cardName;
        _cost = deckCard.cost.ToString();
        _description = deckCard.description;
    }

}
