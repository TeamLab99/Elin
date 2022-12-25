using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Collection : UI_Scene
{  
    enum GameObjects
    {
        Content
    }

    void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Dictionary<int, DeckCard> _dict = Managers.Data.CardDict;
        
        Bind<GameObject>(typeof(GameObjects));

        GameObject content = Get<GameObject>((int)GameObjects.Content);
       
        foreach (Transform child in content.transform)
            Managers.Resource.Destroy(child.gameObject);

        foreach (KeyValuePair<int, DeckCard> cardinfo in _dict)
        {
            GameObject card = Managers.UI.MakeCard<UI_Card>(content.transform, "UI_Card").gameObject;
            UI_Card cardInDeck = card.GetOrAddComponent<UI_Card>();
            cardInDeck.SetInfo(cardinfo.Value);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
