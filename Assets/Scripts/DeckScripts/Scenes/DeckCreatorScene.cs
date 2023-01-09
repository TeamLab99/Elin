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

    }

    public override void Clear()
    {

    }
}
