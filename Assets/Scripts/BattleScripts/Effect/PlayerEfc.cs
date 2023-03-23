using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerEfc : MonoBehaviour
{
    Sequence mySequence;

    void Start()
    {
        mySequence = DOTween.Sequence()
        .SetAutoKill(false) //추가
        .OnStart(() =>
        {
            transform.localScale = Vector3.one/1.5f;
        })
        .Append(transform.DOMove(transform.position + Vector3.up*4.5f, 0.4f))
        .Join(GetComponent<SpriteRenderer>().DOFade(1, 0.3f))
        .Append(GetComponent<SpriteRenderer>().DOFade(0, 0.3f))
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
