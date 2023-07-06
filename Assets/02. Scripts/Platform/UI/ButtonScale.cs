using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonScale : MonoBehaviour,IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 originalScale;
    private float scaleIncreasment = 1.2f;

    private void Awake()
    {
        originalScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = originalScale * scaleIncreasment;
    }
   
    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = originalScale;
    }
}
