using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : MonoBehaviour
{
    public Transform bridgeTransform; // 다리의 Transform 컴포넌트를 참조할 변수
    private Quaternion targetRotation = Quaternion.Euler(0f, 0f, 0f); // 목표 회전값을 저장할 변수
    public float rotationSpeed = 3f; // 회전 속도 설정

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
            LayBridge();
    }

    public void LayBridge()
    {
        StartCoroutine(RotateBridge());
    }

    IEnumerator RotateBridge()
    {
        Quaternion startRotation = bridgeTransform.rotation; // 시작 회전값 저장
        float elapsedTime = 0f; // 경과 시간 초기화

        while (elapsedTime < rotationSpeed)
        {
            elapsedTime += Time.deltaTime; // 경과 시간 업데이트
            float t = elapsedTime / rotationSpeed; // 경과 시간의 비율 계산
            bridgeTransform.rotation = Quaternion.Slerp(startRotation, targetRotation, t); // 보간 회전 적용
            yield return null;
        }

        bridgeTransform.rotation = targetRotation; // 최종 목표 회전값 설정
    }
}