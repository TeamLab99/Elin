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
        BattleGameManager.PlatformUIControlForBattle+= MoveAndOff;
        BattleGameManager.PlatformUIControlForDialouge+= ActiveControl;
        originRect = GetComponent<RectTransform>().rect;
    }
    private void OnDestroy()
    {
        BattleGameManager.PlatformUIControlForBattle -= MoveAndOff;
        BattleGameManager.PlatformUIControlForDialouge -= ActiveControl;
    }
    public void MoveAndOff()
    {
        GetComponent<RectTransform>().DOAnchorPos(targetPos, 2f).OnComplete(() => gameObject.SetActive(false));
    }

    public void ActiveControl()
    {
        if (gameObject.activeSelf)
            gameObject.SetActive(false);
        else
            gameObject.SetActive(true);
    }
}
