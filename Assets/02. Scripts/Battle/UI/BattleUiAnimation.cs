using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BattleUiAnimation : MonoBehaviour
{
    [SerializeField] EUiAnimation eUi;
    [SerializeField] float targetPosXY;
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
            case EUiAnimation.Buff:
                BuffOnEnableAnimation();
                break;
            default:
                break;
        }
    }

    private void OnDisable()
    {
        gameObject.GetComponent<RectTransform>().position = originRectPos;
    }

    public void DisableAnimation()
    {
        switch (eUi)
        {
            case EUiAnimation.Gauge:
                GaugeOnDisableAnimation();
                break;
            case EUiAnimation.Cost:
                CostOnDisableAnimation();
                break;
            case EUiAnimation.Buff:
                BuffOnDisableAnimation();
                break;
            default:
                break;
        }
    }

    void GaugeOnEnableAnimation()
    {
        gameObject.GetComponent<RectTransform>().DOAnchorPosY(targetPosXY, playTime);
    }
    
    void GaugeOnDisableAnimation()
    {
        gameObject.GetComponent<RectTransform>().DOAnchorPosY(originRectPos.y, playTime);
    }
    

    void CostOnEnableAnimation()
    {
        gameObject.GetComponent<RectTransform>().DOAnchorPosY(targetPosXY, playTime);
    }
    
    void CostOnDisableAnimation()
    {
        gameObject.GetComponent<RectTransform>().DOAnchorPosY(originRectPos.y, playTime);
    }

    void BuffOnEnableAnimation()
    {
        gameObject.GetComponent<RectTransform>().DOAnchorPosX(targetPosXY, playTime);
    }
    
    void BuffOnDisableAnimation()
    {
        gameObject.GetComponent<RectTransform>().DOAnchorPosX(originRectPos.x, playTime);
    }
}
