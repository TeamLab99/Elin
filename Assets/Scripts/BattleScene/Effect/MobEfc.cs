using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MobEfc : MonoBehaviour
{
    Sequence mySequence;

    void Start()
    {
        mySequence = DOTween.Sequence()
        .SetAutoKill(false) //추가
        .OnStart(() =>
        {
            transform.localScale = Vector3.zero;
            GetComponent<SpriteRenderer>().color = new Color32(202, 56, 173, 0);
        })
        .Append(transform.DOScale(0.3f, 0.5f))
        .Join(GetComponent<SpriteRenderer>().DOFade(1, 0.5f))
        .Append(GetComponent<SpriteRenderer>().DOFade(0, 0.5f))
        .OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }

    void OnEnable()
    {
        mySequence.Restart();
    }
}