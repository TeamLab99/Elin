using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//UI에서 발생한 이벤트를 처리하는 핸들러이다.
//지금 사용 중이지는 않다
//InputManager와 비슷하게 invoke를 사용함
public class UI_EventHandler : MonoBehaviour, IDragHandler, IPointerClickHandler
{
    public Action<PointerEventData> OnClickHandler = null;
    public Action<PointerEventData> OnDragHandler = null;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (OnClickHandler != null)
            OnClickHandler.Invoke(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (OnDragHandler != null)
            OnDragHandler.Invoke(eventData);
    }
}
