using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Collection : UI_Scene
{
    public int[] howManyCard; // 이 카드가 몇 장인가, 나중에 UI_Card로 통합할 수도 있음
    int startID;
    int offSet;
    
    enum GameObjects
    {
        CollectionUIPanel
    }

    void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        startID = 0;

        Dictionary<int, DeckCard> _dict = Managers.Data.CardDict;

        //foreach를 사용해서 리스트를 만들고 그 길이를 이용해서 howManyCard 리스트를 초기화 ?
        //별로 같은데 다른 방법으로는 애초에 카드 데이터에 카드를 몇 장 가지고 있는지까지 넣는 것
        //아래의 foreach문에서 그냥 UI_Card에 넣어주면 된다.
        
        Bind<GameObject>(typeof(GameObjects));

        GameObject collectionUIPanel = Get<GameObject>((int)GameObjects.CollectionUIPanel);
        foreach (Transform child in collectionUIPanel.transform)
            Managers.Resource.Destroy(child.gameObject);

        foreach (KeyValuePair<int, DeckCard> cardinfo in _dict)
        {
            GameObject card = Managers.UI.MakeCard<UI_Card>(collectionUIPanel.transform, "UI_Card").gameObject;
            UI_Card cardInDeck = card.GetOrAddComponent<UI_Card>();
            cardInDeck.SetInfo(cardinfo.Value);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
