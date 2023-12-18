using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlatformUIAnimation : MonoBehaviour
{
    [SerializeField] Vector3 targetPos;
    Vector3 originRect;

    private void Start()
    {
        BattleManager.PlatformUIControlForBattle+= MoveAndOff;
        BattleManager.PlatformUIControlForDialouge+= ActiveControl;
        originRect = GetComponent<RectTransform>().transform.position;
    }
    private void OnDestroy()
    {
        BattleManager.PlatformUIControlForBattle -= MoveAndOff;
        BattleManager.PlatformUIControlForDialouge -= ActiveControl;
    }
    public void MoveAndOff()
    {
        GetComponent<RectTransform>().DOAnchorPos(targetPos, 2f).OnComplete(() =>
        {
            gameObject.SetActive(false);
            GetComponent<RectTransform>().transform.position = originRect;
        });
    }

    public void ActiveControl()
    {
        if (gameObject.activeSelf)
            gameObject.SetActive(false);
        else
            gameObject.SetActive(true);
    }
}
