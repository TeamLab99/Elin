using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlatformUIAnimation : MonoBehaviour
{
    [SerializeField] Vector3 targetPos;
    Rect originRect;

    private void Start()
    {
        BattleGameManager.PlatformUIControl+= ActiveOn;
        originRect = GetComponent<RectTransform>().rect;
    }
    private void OnDestroy()
    {
        BattleGameManager.PlatformUIControl -= ActiveOn;
    }
    public void ActiveOn()
    {
        GetComponent<RectTransform>().DOAnchorPos(targetPos, 2f).OnComplete(() => gameObject.SetActive(false));
    }
}
