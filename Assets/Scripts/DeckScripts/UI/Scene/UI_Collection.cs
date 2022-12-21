using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Collection : UI_Scene
{
    public int[] howManyCard; // �� ī�尡 �� ���ΰ�, ���߿� UI_Card�� ������ ���� ����
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

        //foreach�� ����ؼ� ����Ʈ�� ����� �� ���̸� �̿��ؼ� howManyCard ����Ʈ�� �ʱ�ȭ ?
        //���� ������ �ٸ� ������δ� ���ʿ� ī�� �����Ϳ� ī�带 �� �� ������ �ִ������� �ִ� ��
        //�Ʒ��� foreach������ �׳� UI_Card�� �־��ָ� �ȴ�.
        
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
