using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//씬에 대한 스크립트이다, 아직까지는 큰 기능은 없다
public abstract class BaseScene : MonoBehaviour
{
    public Define.Scene SceneType { get; protected set; } = Define.Scene.Unknown;

    // Start is called before the first frame update
    void Awake()
    {
        Init();
    }

    protected virtual void Init()
    {
        Object obj = GameObject.FindObjectOfType(typeof(EventSystem));
        if (obj == null)
            //새로운 씬에 들어왔다고 했을 때 SceneMangerEx에 의해서 Init되는데 그 때 이벤트 시스템이 존재하지 않을 경우
            //이벤트 시스템을 씬에 인스턴시에트해줌
            Managers.Resource.Instantiate("DeckCreator/EventSystem").name = "@EventSystem";
    }

    public abstract void Clear();
}
