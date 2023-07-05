using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlatformUIAnimation : MonoBehaviour
{
    [SerializeField] Vector3 targetPos;

    private void Start()
    {
        BattleGameManager.PlatformUIControl+= ActiveOn;
    }
    private void OnDestroy()
    {
        BattleGameManager.PlatformUIControl -= ActiveOn;
    }
    public void ActiveOn()
    {
        transform.DOMove(targetPos, 1f).OnComplete(() => gameObject.SetActive(false));
    }
}
