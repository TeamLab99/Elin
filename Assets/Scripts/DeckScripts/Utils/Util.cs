using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//자주 사용하는 함수와 그와 관련된 구문들을 모아서 하나의 함수로 만든 것
public class Util
{
    //없으면 get 있다면 add하는 작업을 한 번에 할 수 있도록 함수로 묶은 것
    public static T GetOrAddComponent<T>(GameObject go) where T : UnityEngine.Component
    {
        go.TryGetComponent(out T component);

        if (component == null)
            component = go.AddComponent<T>();
        return component;
    }

    //child를 찾는 작업을 개선한 것
    //recursive는 차일드의 차일드까지 다 찾을 것인지 묻는 것
    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        Transform transform = FindChild<Transform>(go, name, recursive);
        if (transform == null)
            return null;

        return transform.gameObject;
    }
    
    //제네릭을 이용
    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        if (go == null)
            return null;

        if (recursive == false)
        {
            for (int i = 0; i < go.transform.childCount; i++)
            {
                Transform transform = go.transform.GetChild(i);
                if (string.IsNullOrEmpty(name) || transform.name == name)
                {
                    transform.TryGetComponent(out T component);

                    if (component != null)
                        return component;
                }
            }
        }
        else
        {
            foreach (T component in go.GetComponentsInChildren<T>())
            {
                if (string.IsNullOrEmpty(name) || component.name == name)
                    return component;
            }
        }

        return null;
    }

}
