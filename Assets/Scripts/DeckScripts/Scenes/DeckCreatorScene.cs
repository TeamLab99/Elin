using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckCreatorScene : BaseScene
{
    protected override void Init()
    {
        base.Init();
        //덱크리에이터 씬에 들어온 것
        SceneType = Define.Scene.DeckCreator;

        //풀링 등은 이 부분에서 해주면 될 것 같다.
    }

    public override void Clear()
    {

    }
}
