using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 자신의 턴을 마무리 할 때 누르는 EndTurnBtn 의 기능을 담은 스크립트
/// 이전 하스스톤 예제에서 사용했던 기능이지만 현재는 사용 안하고 있다.
/// </summary>

public class EndTurnBtn : MonoBehaviour
{
    [SerializeField] Sprite active;
    [SerializeField] Sprite inactive;
    [SerializeField] TMP_Text btnText;


    private void Start()
    {
        Setup(false);
        TurnManager.OnTurnStarted += Setup; // 턴 매니저에서 OnTurnStarted 이벤트가 실행되면 Setup에 true를 넘겨주면 실행
    }

    private void OnDestroy()
    {
        TurnManager.OnTurnStarted -= Setup; // 위와 반대상황
    }

    public void Setup(bool isActive)
    {
        GetComponent<Image>().sprite = isActive ? active : inactive; // 나의 턴이면 isActive true로 전달되어 활성화 이미지로 변경, 상대 턴이면 false로 전달되어 비활성화 이미지로 변경
        GetComponent<Button>().interactable = isActive; // isActive에 따라 Button 컴포넌트 활성화/비활성화
        btnText.color = isActive ? new Color32(255, 195, 90, 255) : new Color32(55, 55, 55, 255); // 텍스트 색깔도 다르게 변경
    }
}
