using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BattleUiAnimation : MonoBehaviour
{
    [SerializeField] EUiAnimation eUi;
    [SerializeField] float targetPosY;
    [SerializeField] float playTime;
    Vector3 originRectPos;

    private void Awake()
    {
        originRectPos = gameObject.GetComponent<RectTransform>().position;
    }

    private void OnEnable()
    {
        switch (eUi)
        {
            case EUiAnimation.Gauge:
                GaugeOnEnableAnimation();
                break;
            case EUiAnimation.Cost:
                CostOnEnableAnimation();
                break;
            default:
                break;
        }
    }

    private void OnDisable()
    {
        gameObject.GetComponent<RectTransform>().position = originRectPos;
    }

    void GaugeOnEnableAnimation()
    {
        gameObject.GetComponent<RectTransform>().DOAnchorPosY(targetPosY, playTime);
    }

    void CostOnEnableAnimation()
    {
        gameObject.GetComponent<RectTransform>().DOAnchorPosY(targetPosY, playTime);
    }
}
