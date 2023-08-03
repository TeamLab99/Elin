using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BattleGaugeIconAnimation : MonoBehaviour
{
    [SerializeField] bool isActive;
    [SerializeField] RectTransform point;
    Image icon;

    RectTransform rectTr;
    float curRectPos;
    WaitForSeconds seconds = new WaitForSeconds(0.5f);
    Vector2 originalPos;
    Vector3 pointRot;
    Vector3 quaternion;

    Sequence sequence;

    void Start()
    {
        BattleCardManager.EffectPlayBack += TImerControl;
        rectTr = GetComponent<RectTransform>();
        originalPos = GetComponent<RectTransform>().anchoredPosition;
        pointRot = new Vector3(0, 0, 20);
        icon = GetComponent<Image>();
        quaternion = rectTr.rotation.eulerAngles;
        StartCoroutine(ChangeRotation());
    }
    private void OnDestroy()
    {
        BattleCardManager.EffectPlayBack -= TImerControl;
    }

    IEnumerator ChangeRotation()
    {
        while (true)
        {
            rectTr.DOPunchRotation(pointRot, 0.5f, 3);
            yield return seconds;
        }
    }

    public void Animation(float time)
    {
        rectTr.anchoredPosition = new Vector2(Mathf.Lerp(point.anchoredPosition.x, originalPos.x, time),0);
    }


    public void Reset()
    {
        GetComponent<RectTransform>().anchoredPosition = originalPos;
    }

    public void TImerControl(bool isStop)
    {
        if (isStop)
        {
            sequence.Pause();
        }
        else
        {
            sequence.Play();
        }
    }

    void StopCoroutine()
    {
        StopCoroutine(ChangeRotation());
    }

    public void SetIcon(Sprite sprite)
    {
        icon.sprite = sprite;
    }
}
