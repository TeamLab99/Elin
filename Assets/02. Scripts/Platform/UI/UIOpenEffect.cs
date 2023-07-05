using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIOpenEffect : MonoBehaviour
{
    [SerializeField] GameObject allObjects;
    [SerializeField] RectTransform rect;

    public float targetWidth = 1200;
    public float duration = 3f;
    private float currentWidth;
    private float timer;
    private bool increaseWidth = true;

    private void Update()
    {
        if (increaseWidth)
        {
            timer += Time.deltaTime;
            currentWidth = Mathf.Lerp(0f, targetWidth, timer / duration);
            rect.sizeDelta = new Vector2(currentWidth, rect.sizeDelta.y);
            if (currentWidth >= targetWidth)
            {
                increaseWidth = false;
                allObjects.SetActive(true);
            }
        }
    }

    private void OnEnable()
    {
        currentWidth = 0f;
        timer = 0f;
        increaseWidth = true;
    }
}
