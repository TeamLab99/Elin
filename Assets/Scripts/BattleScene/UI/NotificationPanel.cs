using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

/// <summary>
/// 턴의 시작을 알리는 NotificationPanel 팝업을 띄우는 스크립트
/// EndTurnBtn 스크립트 처럼 현재 사용 중인 스크립트가 아니다.
/// </summary>

public class NotificationPanel : MonoBehaviour
{
    [SerializeField] TMP_Text notificationTMP;

    // 전달받은 string값과, RGB 컬러 값으로 변경하고 애니메이션을 보여주는 함수 
    public void Show(string message, Color32 color)
    {
        notificationTMP.text = message;
        notificationTMP.faceColor = color;
        Sequence sequence = DOTween.Sequence() // DOTween 애니메이션을 시퀀스형태로 차례대로 재생시켜준다.
            .Append(transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.InOutQuad)) // 크기 변경 애니메이션. InOutQuad는 움직임 그래프 종류 중 하나이다.
            .AppendInterval(0.9f) // 이 후 딜레이 0.9초
            .Append(transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InOutQuad)); // Vector3.zero = (0, 0, 0) 크기를 0으로 만든다.
    }

    void Start() => ScaleZero(); // 시작 시, 크기는 0인 상태가 된다,

    /// <summary>
    /// 컴포넌트 이름 우측에 마우스를 우클릭 했을 때 등장하는 메뉴들에 추가를 해줬다.
    /// </summary>
    [ContextMenu("ScaleOne")]
    void ScaleOne() => transform.localScale = Vector3.one;

    [ContextMenu("ScaleZero")] 
    public void ScaleZero() => transform.localScale = Vector3.zero;

}
