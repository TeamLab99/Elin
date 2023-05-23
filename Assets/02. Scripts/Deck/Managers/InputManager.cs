using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//키보드 및 마우스 입력을 위한 매니저이나 현재는 사용하지 않는다.
//Invoke를 이용해서 작동하는 방식
public class InputManager
{
    public Action KeyAction = null;
    public Action<MouseEvent> MouseAction = null;

    bool _pressed = false;
    public void OnUpdate()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (Input.anyKey && KeyAction != null)
            KeyAction.Invoke();

        if (MouseAction != null)
        {
            if (Input.GetMouseButton(0))
            {
                MouseAction.Invoke(MouseEvent.Press);
                _pressed = true;
            }
            else
            {
                if (_pressed)
                    MouseAction.Invoke(MouseEvent.Click);
                _pressed = false;
            }
        }
    }

    public void Clear()
    {
        KeyAction = null;
        MouseAction = null;
    }
}
