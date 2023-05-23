using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

//UI에서 사용하는 다양한 함수들이 정의되어 있다.
public abstract class UI_Base : MonoBehaviour
{
    //바인딩에 사용될 딕셔너리
    //enum을 key로 오브젝트 리스트를 value로 가짐
    protected Dictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, UnityEngine.Object[]>();

    //추상화
    public abstract void Init();

    //바인드 함수, 제네릭 이용
    //주로 enum을 받아서 실제 게임 오브젝트와 연결해주는 역할을 함
    protected void Bind<T>(Type type) where T : UnityEngine.Object
    {
        //enum을 받아옴
        string[] names = Enum.GetNames(type);
        //enum 길이에 맞게 빈 배열을 하나 만들고
        UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];
        //그 배열을 딕셔너리의 value로 추가
        _objects.Add(typeof(T), objects);

        //반복문으로 enum의 수만큼 돌면서 findchild를 이용해 오브젝트를 찾음 -> 바인딩
        for (int i = 0; i < names.Length; i++)
        {
            if (typeof(T) == typeof(GameObject))
                objects[i] = Util.FindChild(gameObject, names[i], true);
            else
                objects[i] = Util.FindChild<T>(gameObject, names[i], true);

            if (objects[i] == null)
                Debug.Log($"Failde to bind({names[i]})");
        }
    }

    //배열에서 내가 원하는 값을 가져옴
    protected T Get<T>(int idx) where T : UnityEngine.Object
    {
        UnityEngine.Object[] objects = null;
        if (_objects.TryGetValue(typeof(T), out objects) == false)
            return null;

        return objects[idx] as T;
    }

    //다양한 형식에 대한 get을 함수로 만들었음
    protected GameObject GetObject(int idx) { return Get<GameObject>(idx); }

    //원래는 legacy텍스트에 대한 것이었는데 내가 텍스트매쉬프로를 쓰면서 조금 수정함
    protected TextMeshProUGUI GetText(int idx) { return Get<TextMeshProUGUI>(idx); }
    //protected Text GetText(int idx) { return Get<Text>(idx); }
    protected Button GetButton(int idx) { return Get<Button>(idx); }
    protected Image GetImage(int idx) { return Get<Image>(idx); }

    //이벤트를 바인딩해주는 함수
    //지금 사용하지는 않음
    public static void BindEvent(GameObject go, Action<PointerEventData> action, UIEvent type = UIEvent.Click)
    {

        UI_EventHandler evt = Util.GetOrAddComponent<UI_EventHandler>(go);

        switch (type)
        {
            case UIEvent.Click:
                evt.OnClickHandler -= action;
                evt.OnClickHandler += action;
                break;
            case UIEvent.Drag:
                evt.OnDragHandler -= action;
                evt.OnDragHandler += action;
                break;
        }
        evt.OnDragHandler += ((PointerEventData data) => { evt.gameObject.transform.position = data.position; });
    }
}
