using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    //UI들도 어떤 루트에 모아둘 예정이었다.
    //하지만 카드는 content의 자식으로 들어가야하므로 현재 사용 중이지는 않다.
    public GameObject Root
    {
        get
        {
            GameObject root = GameObject.FindWithTag("UIRoot");
            if (root == null)
                root = new GameObject { name = "@UI_Root", tag = "UIRoot" };
            return root;
        }
    }

    //화면 상에 띄울 카드를 만드는 함수
    //UIManager라고는 하지만 사실상 사용하는 건 이 함수 밖에 없다.
    public T MakeCard<T>(Transform parent = null, string name = null) where T : UI_Base
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        //Resources/Prefabs/DeckCreator 산하에서 해당하는 이름의 프리팹을 찾아서 인스턴시에트한다. 
        GameObject go = Managers.Resource.Instantiate($"DeckCreator/{name}");

        if (parent != null)
            go.transform.SetParent(parent);

        return Util.GetOrAddComponent<T>(go);
    }

    public void Clear()
    {

    }
}
