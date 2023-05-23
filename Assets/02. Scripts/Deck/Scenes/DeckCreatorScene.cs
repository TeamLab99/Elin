using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckCreatorScene : BaseScene
{
    [SerializeField]
    GameObject card;

    protected override void Init()
    {
        base.Init();

        //덱크리에이터 씬에 들어온 것
        SceneType = Scene.DeckCreator;

        //풀링 등은 이 부분에서 해주면 될 것 같다.
        //생각해보니 UI는 RectTransform이다. 이건 그냥 Transform
        //그 차이를 개선해야 풀링이 가능하다.
        Managers.Pool.CreatePool(card, 20);
    }

    public override void Clear()
    {

    }
}
